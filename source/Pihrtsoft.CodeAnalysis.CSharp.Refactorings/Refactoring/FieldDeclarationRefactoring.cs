﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactoring
{
    internal static class FieldDeclarationRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration.Modifiers.Contains(SyntaxKind.ConstKeyword))
            {
                if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.ReplaceConstantWithField)
                    && fieldDeclaration.Span.Contains(context.Span))
                {
                    context.RegisterRefactoring(
                        "Replace constant with field",
                        cancellationToken => ReplaceConstantWithFieldRefactoring.RefactorAsync(context.Document, fieldDeclaration, cancellationToken));
                }
            }
            else if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.ReplaceFieldWithConstant)
                && fieldDeclaration.Modifiers.Contains(SyntaxKind.ReadOnlyKeyword)
                && fieldDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword)
                && fieldDeclaration.Span.Contains(context.Span)
                && context.SupportsSemanticModel)
            {
                if (await ReplaceFieldWithConstantRefactoring.CanRefactorAsync(context, fieldDeclaration))
                {
                    context.RegisterRefactoring(
                        "Replace field with constant",
                        cancellationToken => ReplaceFieldWithConstantRefactoring.RefactorAsync(context.Document, fieldDeclaration, cancellationToken));
                }
            }

            if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.MarkMemberAsStatic)
                && fieldDeclaration.Span.Contains(context.Span)
                && MarkMemberAsStaticRefactoring.CanRefactor(fieldDeclaration))
            {
                context.RegisterRefactoring(
                    "Mark field as static",
                    cancellationToken => MarkMemberAsStaticRefactoring.RefactorAsync(context.Document, fieldDeclaration, cancellationToken));

                MarkAllMembersAsStaticRefactoring.RegisterRefactoring(context, (ClassDeclarationSyntax)fieldDeclaration.Parent);
            }
        }
    }
}
