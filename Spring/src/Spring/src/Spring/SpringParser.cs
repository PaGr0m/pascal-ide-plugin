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
using static JetBrains.ReSharper.Plugins.Spring.PascalParser.TokenGroup;
using static JetBrains.ReSharper.Plugins.Spring.PascalParser.ParserCombinators;

namespace JetBrains.ReSharper.Plugins.Spring.Spring
{
    internal class SpringParser : IParser
    {
        private readonly ILexer _myLexer;
        private readonly LateInitPredicate _program;

        public SpringParser(ILexer lexer)
        {
            _myLexer = lexer;
            _program = new LateInitPredicate();

            InitializeParsers();
        }

        public IFile ParseFile()
        {
            using var def = Lifetime.Define();
            var builder = new PsiBuilder(_myLexer, SpringFileNodeType.Instance, new TokenFactory(), def.Lifetime);

            var fileMark = builder.Mark();
            ParseBlock(builder);
            builder.Done(fileMark, SpringFileNodeType.Instance, null);

            var file = (IFile) builder.BuildTree();
            return file;
        }

        private void ParseBlock(PsiBuilder builder)
        {
            // Skip first whitespaces
            while (
                !builder.Eof() &&
                (builder.GetTokenType().IsWhitespace || builder.GetTokenType().IsComment)
            )
            {
                builder.AdvanceLexer();
            }

            // Parsing
            var parsed = _program.AsPredicate(builder);

            if (!parsed)
            {
                builder.Error("Parse error");
            }

            if (!builder.Eof())
            {
                builder.Error("Unexpected symbol");
            }

            // Read remaining tokens
            while (!builder.Eof())
            {
                builder.AdvanceLexer();
            }
        }

        private void InitializeParsers()
        {
            // Expression
            var expression = new LateInitPredicate();
            var factor = new LateInitPredicate();

            var identifier = Partial(Token, SpringTokenType.Identifier);

            var actualParameterList = Sequence(
                LeftParenthesis,
                Alternative(
                    Sequence(
                        Many(Sequence(expression.AsPredicate, Comma)),
                        expression.AsPredicate,
                        RightParenthesis
                    ),
                    RightParenthesis
                )
            );

            var functionCall = Alternative(
                Sequence(identifier, actualParameterList),
                identifier
            );

            var setGroup = Alternative(
                Sequence(expression.AsPredicate, Range, expression.AsPredicate),
                expression.AsPredicate
            );

            var setConstructor = Sequence(
                LeftSquareBracket,
                Alternative(
                    Sequence(
                        Many(Sequence(setGroup, Comma)),
                        RightSquareBracket
                    ),
                    RightSquareBracket
                )
            );

            var valueTypecast = Sequence(
                identifier,
                LeftParenthesis,
                expression.AsPredicate,
                RightParenthesis
            );

            var term = Sequence(
                factor.AsPredicate,
                Many(Sequence(MultiplicationOperators, factor.AsPredicate))
            );

            var simpleExpression = Sequence(
                term,
                Many(Sequence(AddingOperators, term))
            );

            factor.Init(
                Alternative(
                    Sequence(LeftParenthesis, expression.AsPredicate, RightParenthesis),
                    functionCall,
                    UnsignedConstant,
                    Sequence(Partial(Token, SpringTokenType.Keyword, "not"), factor.AsPredicate),
                    Sequence(Partial(Token, SpringTokenType.SingleSpecialCharacter, "-"), factor.AsPredicate),
                    setConstructor,
                    valueTypecast,
                    Sequence(Partial(Token, SpringTokenType.SingleSpecialCharacter, "@"), factor.AsPredicate)
                )
            );

            expression.Init(Alternative(
                simpleExpression,
                Sequence(simpleExpression, RelationalOperators, simpleExpression)
            ));

            // Statement
            var statement = new LateInitPredicate();

            var assignmentStatement = Sequence(
                identifier,
                AssignmentOperators,
                expression.AsPredicate
            );

            var gotoStatement = Sequence(
                KeywordGoto,
                identifier
            );

            var simpleStatement = Alternative(
                assignmentStatement,
                functionCall,
                gotoStatement
            );

            var compoundStatement = Sequence(
                KeywordBegin,
                Sequence(
                    statement.AsPredicate,
                    Many(Sequence(Semicolon, statement.AsPredicate))
                ),
                KeywordEnd
            );

            var ifStatement = Sequence(
                KeywordIf,
                expression.AsPredicate,
                KeywordThen,
                Alternative(
                    Sequence(statement.AsPredicate, KeywordElse, statement.AsPredicate),
                    statement.AsPredicate
                )
            );

            var caseState = Sequence(
                Sequence(identifier, Many(Sequence(Comma, identifier))),
                Colon,
                statement.AsPredicate
            );

            var elsePart = Sequence(
                Alternative(KeywordElse, KeywordOtherwise),
                statement.AsPredicate,
                Many(Sequence(Semicolon, statement.AsPredicate))
            );

            var caseStatement = Sequence(
                KeywordCase,
                expression.AsPredicate,
                KeywordOf,
                Sequence(caseState, Many(Sequence(Semicolon, caseState))),
                Alternative(
                    Sequence(
                        elsePart,
                        Alternative(
                            Sequence(Semicolon, KeywordEnd),
                            KeywordEnd
                        )
                    ),
                    Alternative(
                        Sequence(Semicolon, KeywordEnd),
                        KeywordEnd
                    )
                )
            );

            var forStatement1 = Sequence(
                KeywordFor,
                identifier,
                KeywordAppropriation,
                expression.AsPredicate,
                Alternative(KeywordTo, KeywordDownto),
                expression.AsPredicate,
                KeywordDo,
                statement.AsPredicate
            );

            var forStatement2 = Sequence(
                KeywordFor,
                identifier,
                KeywordIn,
                expression.AsPredicate,
                KeywordDo,
                statement.AsPredicate
            );

            var forStatement = Alternative(
                forStatement1,
                forStatement2
            );

            var repeatStatement = Sequence(
                KeywordRepeat,
                Sequence(statement.AsPredicate, Many(Sequence(Semicolon, statement.AsPredicate))),
                KeywordUntil,
                expression.AsPredicate
            );

            var whileStatement = Sequence(
                KeywordWhile,
                expression.AsPredicate,
                KeywordDo,
                statement.AsPredicate
            );

            var conditionalStatement = Alternative(
                caseStatement,
                ifStatement
            );

            var repetitiveStatement = Alternative(
                forStatement,
                repeatStatement,
                whileStatement
            );

            var withStatement = Sequence(
                KeywordWith,
                Sequence(identifier, Many(Sequence(Semicolon, identifier))),
                KeywordDo,
                statement.AsPredicate
            );

            var structuredStatement = Alternative(
                compoundStatement,
                conditionalStatement,
                repetitiveStatement,
                withStatement
            );

            statement.Init(Alternative(simpleStatement, structuredStatement));

            _program.Init(Sequence(statement.AsPredicate, Dot));
        }

        private class LateInitPredicate
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