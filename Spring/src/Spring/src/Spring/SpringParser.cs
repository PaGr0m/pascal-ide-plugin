using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.SelectEmbracingConstruct;
using JetBrains.ReSharper.I18n.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.TreeBuilder;
using JetBrains.Text;
using static JetBrains.ReSharper.Plugins.Spring.PascalParser.PascalParser;

namespace JetBrains.ReSharper.Plugins.Spring.Spring
{
    internal class SpringParser : IParser
    {
        private readonly ILexer myLexer;

        public SpringParser(ILexer lexer)
        {
            myLexer = lexer;
        }

        public IFile ParseFile()
        {
            using (var def = Lifetime.Define())
            {
                var builder = new PsiBuilder(myLexer, SpringFileNodeType.Instance, new TokenFactory(), def.Lifetime);
                var fileMark = builder.Mark();

                // ParseBlock(builder);

                var expression = new LateInitPredicate();

                var factor = Alternative(
                    Sequence(
                        Partial(Token, SpringTokenType.SingleSpecialCharacter, "("),
                        expression.AsPredicate,
                        Partial(Token, SpringTokenType.SingleSpecialCharacter, ")")
                    ),
                    Partial(Token, SpringTokenType.Number)
                );

                var term = Sequence(
                    factor,
                    Many(Sequence(MultiplicationOperators, factor))
                );

                var simpleExpression = Sequence(
                    term,
                    Many(Sequence(AddingOperators, term))
                );

                expression.Init(Alternative(
                    simpleExpression,
                    Sequence(simpleExpression, LogicalOperation, simpleExpression)
                ));


                // Predicate<PsiBuilder> expression1 = new LateInitPredicate().AsPredicate;

                // var ifToken = Partial(
                //     Token,
                //     SpringTokenType.Keyword,
                //     "if"
                // );
                // var whitespace = Partial(
                //     Token,
                //     SpringTokenType.Whitespace
                // );
                // var pred = Sequence(ifToken, whitespace, ifToken);
                var flag = expression.AsPredicate(builder);

                if (!flag)
                {
                    builder.Error("Shit");
                }

                // while (!builder.Eof())
                // {
                //     builder.AdvanceLexer();
                // }

                builder.Done(fileMark, SpringFileNodeType.Instance, null);
                var file = (IFile) builder.BuildTree();
                return file;
            }
        }

        class LateInitPredicate
        {
            private Predicate<PsiBuilder> _predicate;

            public void Init(Predicate<PsiBuilder> predicate)
            {
                _predicate = predicate;
            }

            public bool AsPredicate(PsiBuilder builder)
            {
                return _predicate(builder);
            }
        }

        private void ParseBlock(PsiBuilder psiBuilder)
        {
        }
    }

    [DaemonStage]
    class SpringDaemonStage : DaemonStageBase<SpringFile>
    {
        protected override IDaemonStageProcess CreateDaemonProcess(IDaemonProcess process,
            DaemonProcessKind processKind, SpringFile file,
            IContextBoundSettingsStore settingsStore)
        {
            return new SpringDaemonProcess(process, file);
        }

        internal class SpringDaemonProcess : IDaemonStageProcess
        {
            private readonly SpringFile myFile;

            public SpringDaemonProcess(IDaemonProcess process, SpringFile file)
            {
                myFile = file;
                DaemonProcess = process;
            }

            public void Execute(Action<DaemonStageResult> committer)
            {
                var highlightings = new List<HighlightingInfo>();
                foreach (var treeNode in myFile.Descendants())
                {
                    if (treeNode is PsiBuilderErrorElement error)
                    {
                        var range = error.GetDocumentRange();
                        highlightings.Add(
                            new HighlightingInfo(
                                range,
                                new CSharpSyntaxError(error.ErrorDescription, range)
                            )
                        );
                    }
                }

                var result = new DaemonStageResult(highlightings);
                committer(result);
            }

            public IDaemonProcess DaemonProcess { get; }
        }

        protected override IEnumerable<SpringFile> GetPsiFiles(IPsiSourceFile sourceFile)
        {
            yield return (SpringFile) sourceFile.GetDominantPsiFile<SpringLanguage>();
        }
    }

    internal class TokenFactory : IPsiBuilderTokenFactory
    {
        public LeafElementBase CreateToken(TokenNodeType tokenNodeType, IBuffer buffer, int startOffset, int endOffset)
        {
            return tokenNodeType.Create(buffer, new TreeOffset(startOffset), new TreeOffset(endOffset));
        }
    }

    [ProjectFileType(typeof(SpringProjectFileType))]
    public class SelectEmbracingConstructProvider : ISelectEmbracingConstructProvider
    {
        public bool IsAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.LanguageType.Is<SpringProjectFileType>();
        }

        public ISelectedRange GetSelectedRange(IPsiSourceFile sourceFile, DocumentRange documentRange)
        {
            var file = (SpringFile) sourceFile.GetDominantPsiFile<SpringLanguage>();
            var node = file.FindNodeAt(documentRange);
            return new SpringTreeNodeSelection(file, node);
        }

        public class SpringTreeNodeSelection : TreeNodeSelection<SpringFile>
        {
            public SpringTreeNodeSelection(SpringFile fileNode, ITreeNode node) : base(fileNode, node)
            {
            }

            public override ISelectedRange Parent => new SpringTreeNodeSelection(FileNode, TreeNode.Parent);
        }
    }
}