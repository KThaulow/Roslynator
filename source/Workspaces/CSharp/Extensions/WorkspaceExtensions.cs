﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Documentation;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp
{
    public static class WorkspaceExtensions
    {
        internal static Task<Document> RemoveNodeAsync(
            this Document document,
            SyntaxNode node,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return document.RemoveNodeAsync(node, SyntaxRemover.GetOptions(node), cancellationToken);
        }

        public static Task<Document> RemoveMemberAsync(
            this Document document,
            MemberDeclarationSyntax member,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (member == null)
                throw new ArgumentNullException(nameof(member));

            SyntaxNode parent = member.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var compilationUnit = (CompilationUnitSyntax)parent;

                        return document.ReplaceNodeAsync(compilationUnit, compilationUnit.RemoveMember(member), cancellationToken);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var namespaceDeclaration = (NamespaceDeclarationSyntax)parent;

                        return document.ReplaceNodeAsync(namespaceDeclaration, namespaceDeclaration.RemoveMember(member), cancellationToken);
                    }
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)parent;

                        return document.ReplaceNodeAsync(classDeclaration, classDeclaration.RemoveMember(member), cancellationToken);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)parent;

                        return document.ReplaceNodeAsync(structDeclaration, structDeclaration.RemoveMember(member), cancellationToken);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)parent;

                        return document.ReplaceNodeAsync(interfaceDeclaration, interfaceDeclaration.RemoveMember(member), cancellationToken);
                    }
                default:
                    {
                        Debug.Assert(parent == null, parent.Kind().ToString());

                        return document.RemoveNodeAsync(member, SyntaxRemover.DefaultOptions, cancellationToken);
                    }
            }
        }

        internal static Task<Document> RemoveStatementAsync(
            this Document document,
            StatementSyntax statement,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            return document.RemoveNodeAsync(statement, cancellationToken);
        }

        public static async Task<Document> RemoveCommentsAsync(
            this Document document,
            CommentKinds commentKinds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode newRoot = SyntaxRemover.RemoveComments(root, commentKinds)
                .WithFormatterAnnotation();

            return document.WithSyntaxRoot(newRoot);
        }

        public static async Task<Document> RemoveCommentsAsync(
            this Document document,
            TextSpan span,
            CommentKinds commentKinds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode newRoot = SyntaxRemover.RemoveComments(root, span, commentKinds)
                .WithFormatterAnnotation();

            return document.WithSyntaxRoot(newRoot);
        }

        public static async Task<Document> RemoveTriviaAsync(
            this Document document,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode newRoot = SyntaxRemover.RemoveTrivia(root);

            return document.WithSyntaxRoot(newRoot);
        }

        public static async Task<Document> RemoveTriviaAsync(
            this Document document,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode newRoot = SyntaxRemover.RemoveTrivia(root, span);

            return document.WithSyntaxRoot(newRoot);
        }

        public static async Task<Document> RemovePreprocessorDirectivesAsync(
            this Document document,
            PreprocessorDirectiveKinds directiveKinds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SourceText sourceText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

            SourceText newSourceText = RemovePreprocessorDirectives(sourceText, root.DescendantPreprocessorDirectives(), directiveKinds);

            return document.WithText(newSourceText);
        }

        public static async Task<Document> RemovePreprocessorDirectivesAsync(
            this Document document,
            TextSpan span,
            PreprocessorDirectiveKinds directiveKinds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SourceText sourceText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

            SourceText newSourceText = RemovePreprocessorDirectives(sourceText, root.DescendantPreprocessorDirectives(span), directiveKinds);

            return document.WithText(newSourceText);
        }

        internal static async Task<Document> RemovePreprocessorDirectivesAsync(
            this Document document,
            IEnumerable<DirectiveTriviaSyntax> directives,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (directives == null)
                throw new ArgumentNullException(nameof(directives));

            SourceText sourceText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

            SourceText newSourceText = sourceText.WithChanges(GetTextChanges());

            return document.WithText(newSourceText);

            IEnumerable<TextChange> GetTextChanges()
            {
                TextLineCollection lines = sourceText.Lines;

                foreach (DirectiveTriviaSyntax directive in directives)
                {
                    int startLine = directive.GetSpanStartLine();

                    yield return new TextChange(lines[startLine].SpanIncludingLineBreak, "");
                }
            }
        }

        private static SourceText RemovePreprocessorDirectives(
            SourceText sourceText,
            IEnumerable<DirectiveTriviaSyntax> directives,
            PreprocessorDirectiveKinds directiveKinds)
        {
            return sourceText.WithChanges(GetTextChanges());

            IEnumerable<TextChange> GetTextChanges()
            {
                TextLineCollection lines = sourceText.Lines;

                foreach (DirectiveTriviaSyntax directive in directives)
                {
                    if (ShouldRemoveDirective(directive))
                    {
                        int startLine = directive.GetSpanStartLine();

                        yield return new TextChange(lines[startLine].SpanIncludingLineBreak, "");
                    }
                }
            }

            bool ShouldRemoveDirective(DirectiveTriviaSyntax directive)
            {
                switch (directive.Kind())
                {
                    case SyntaxKind.IfDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.If) != 0;
                    case SyntaxKind.ElifDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Elif) != 0;
                    case SyntaxKind.ElseDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Else) != 0;
                    case SyntaxKind.EndIfDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.EndIf) != 0;
                    case SyntaxKind.RegionDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Region) != 0;
                    case SyntaxKind.EndRegionDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.EndRegion) != 0;
                    case SyntaxKind.DefineDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Define) != 0;
                    case SyntaxKind.UndefDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Undef) != 0;
                    case SyntaxKind.ErrorDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Error) != 0;
                    case SyntaxKind.WarningDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Warning) != 0;
                    case SyntaxKind.LineDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Line) != 0;
                    case SyntaxKind.PragmaWarningDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.PragmaWarning) != 0;
                    case SyntaxKind.PragmaChecksumDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.PragmaChecksum) != 0;
                    case SyntaxKind.ReferenceDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Reference) != 0;
                    case SyntaxKind.BadDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Bad) != 0;
                    case SyntaxKind.ShebangDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Shebang) != 0;
                    case SyntaxKind.LoadDirectiveTrivia:
                        return (directiveKinds & PreprocessorDirectiveKinds.Load) != 0;
                }

                Debug.Fail(directive.Kind().ToString());
                return false;
            }
        }

        public static async Task<Document> RemoveRegionAsync(
            this Document document,
            RegionInfo region,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (!region.Success)
                throw new ArgumentException($"{nameof(RegionInfo)} is not initialized.", nameof(region));

            SourceText sourceText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

            int startLine = region.Directive.GetSpanStartLine();
            int endLine = region.EndDirective.GetSpanEndLine();

            TextLineCollection lines = sourceText.Lines;

            TextSpan span = TextSpan.FromBounds(
                lines[startLine].Start,
                lines[endLine].EndIncludingLineBreak);

            var textChange = new TextChange(span, "");

            SourceText newSourceText = sourceText.WithChanges(textChange);

            return document.WithText(newSourceText);
        }

        internal static Task<Document> ReplaceStatementsAsync(
            this Document document,
            StatementsInfo statementsInfo,
            IEnumerable<StatementSyntax> newStatements,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ReplaceStatementsAsync(document, statementsInfo, List(newStatements), cancellationToken);
        }

        internal static Task<Document> ReplaceStatementsAsync(
            this Document document,
            StatementsInfo statementsInfo,
            SyntaxList<StatementSyntax> newStatements,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return document.ReplaceNodeAsync(statementsInfo.Node, statementsInfo.WithStatements(newStatements).Node, cancellationToken);
        }

        internal static Task<Document> ReplaceMembersAsync(
            this Document document,
            MemberDeclarationsInfo info,
            IEnumerable<MemberDeclarationSyntax> newMembers,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return document.ReplaceNodeAsync(
                info.Declaration,
                info.WithMembers(newMembers).Declaration,
                cancellationToken);
        }

        internal static Task<Document> ReplaceMembersAsync(
            this Document document,
            MemberDeclarationsInfo info,
            SyntaxList<MemberDeclarationSyntax> newMembers,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return document.ReplaceNodeAsync(
                info.Declaration,
                info.WithMembers(newMembers).Declaration,
                cancellationToken);
        }

        internal static Task<Document> ReplaceModifiersAsync(
            this Document document,
            ModifiersInfo modifiersInfo,
            IEnumerable<SyntaxToken> newModifiers,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ReplaceModifiersAsync(document, modifiersInfo, TokenList(newModifiers), cancellationToken);
        }

        internal static Task<Document> ReplaceModifiersAsync(
            this Document document,
            ModifiersInfo modifiersInfo,
            SyntaxTokenList newModifiers,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return document.ReplaceNodeAsync(modifiersInfo.Node, modifiersInfo.WithModifiers(newModifiers).Node, cancellationToken);
        }

        internal static async Task<Document> AddNewDocumentationCommentsAsync(
            Document document,
            DocumentationCommentGeneratorSettings settings = null,
            bool skipNamespaceDeclaration = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var rewriter = new AddNewDocumentationCommentRewriter(settings, skipNamespaceDeclaration);

            SyntaxNode newRoot = rewriter.Visit(root);

            return document.WithSyntaxRoot(newRoot);
        }

        internal static async Task<Document> AddBaseOrNewDocumentationCommentsAsync(
            Document document,
            SemanticModel semanticModel,
            DocumentationCommentGeneratorSettings settings = null,
            bool skipNamespaceDeclaration = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var rewriter = new AddBaseOrNewDocumentationCommentRewriter(semanticModel, settings, skipNamespaceDeclaration, cancellationToken);

            SyntaxNode newRoot = rewriter.Visit(root);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
