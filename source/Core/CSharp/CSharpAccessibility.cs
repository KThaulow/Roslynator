﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Comparers;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp
{
    public static class CSharpAccessibility
    {
        public static Accessibility GetDefaultAccessibility(MemberDeclarationSyntax declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            switch (declaration.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetDefaultAccessibility((ConstructorDeclarationSyntax)declaration);
                case SyntaxKind.DestructorDeclaration:
                    return GetDefaultAccessibility((DestructorDeclarationSyntax)declaration);
                case SyntaxKind.MethodDeclaration:
                    return GetDefaultAccessibility((MethodDeclarationSyntax)declaration);
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultAccessibility((PropertyDeclarationSyntax)declaration);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultAccessibility((IndexerDeclarationSyntax)declaration);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultAccessibility((EventDeclarationSyntax)declaration);
                case SyntaxKind.EventFieldDeclaration:
                    return GetDefaultAccessibility((EventFieldDeclarationSyntax)declaration);
                case SyntaxKind.FieldDeclaration:
                    return Accessibility.Private;
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return Accessibility.Public;
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                    return GetDefaultAccessibility((BaseTypeDeclarationSyntax)declaration);
                case SyntaxKind.DelegateDeclaration:
                    return GetDefaultAccessibility((DelegateDeclarationSyntax)declaration);
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.Public;
            }

            Debug.Fail(declaration.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            return GetDefaultAccessibility((TypeDeclarationSyntax)classDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            if (delegateDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)enumDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)interfaceDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            if (methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(StructDeclarationSyntax structDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)structDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            if (baseTypeDeclaration == null)
                throw new ArgumentNullException(nameof(baseTypeDeclaration));

            if (baseTypeDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultAccessibility(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                throw new ArgumentNullException(nameof(accessorDeclaration));

            SyntaxNode declaration = accessorDeclaration.Parent?.Parent;

            switch (declaration?.Kind())
            {
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultAccessibility((PropertyDeclarationSyntax)declaration);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultAccessibility((IndexerDeclarationSyntax)declaration);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultAccessibility((EventDeclarationSyntax)declaration);
            }

            Debug.Assert(declaration == null, declaration.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(MemberDeclarationSyntax declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            switch (declaration.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetDefaultExplicitAccessibility((ConstructorDeclarationSyntax)declaration);
                case SyntaxKind.DestructorDeclaration:
                    return GetDefaultExplicitAccessibility((DestructorDeclarationSyntax)declaration);
                case SyntaxKind.MethodDeclaration:
                    return GetDefaultExplicitAccessibility((MethodDeclarationSyntax)declaration);
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultExplicitAccessibility((PropertyDeclarationSyntax)declaration);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultExplicitAccessibility((IndexerDeclarationSyntax)declaration);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultExplicitAccessibility((EventDeclarationSyntax)declaration);
                case SyntaxKind.EventFieldDeclaration:
                    return GetDefaultExplicitAccessibility((EventFieldDeclarationSyntax)declaration);
                case SyntaxKind.FieldDeclaration:
                    return Accessibility.Private;
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return Accessibility.Public;
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                    return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)declaration);
                case SyntaxKind.DelegateDeclaration:
                    return GetDefaultExplicitAccessibility((DelegateDeclarationSyntax)declaration);
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.NotApplicable;
            }

            Debug.Fail(declaration.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            return GetDefaultExplicitAccessibility((TypeDeclarationSyntax)classDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultExplicitAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            if (delegateDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)enumDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            if (eventDeclaration.ExplicitInterfaceSpecifier != null)
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultExplicitAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.ExplicitInterfaceSpecifier != null
                || indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)interfaceDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            if (methodDeclaration.Modifiers.Contains(SyntaxKind.PartialKeyword)
                || methodDeclaration.ExplicitInterfaceSpecifier != null
                || methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultExplicitAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.ExplicitInterfaceSpecifier != null
                || propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(StructDeclarationSyntax structDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)structDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            if (baseTypeDeclaration == null)
                throw new ArgumentNullException(nameof(baseTypeDeclaration));

            if (baseTypeDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetAccessibility(MemberDeclarationSyntax declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            switch (declaration.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetAccessibility((ConstructorDeclarationSyntax)declaration);
                case SyntaxKind.MethodDeclaration:
                    return GetAccessibility((MethodDeclarationSyntax)declaration);
                case SyntaxKind.PropertyDeclaration:
                    return GetAccessibility((PropertyDeclarationSyntax)declaration);
                case SyntaxKind.IndexerDeclaration:
                    return GetAccessibility((IndexerDeclarationSyntax)declaration);
                case SyntaxKind.EventDeclaration:
                    return GetAccessibility((EventDeclarationSyntax)declaration);
                case SyntaxKind.EventFieldDeclaration:
                    return GetAccessibility((EventFieldDeclarationSyntax)declaration);
                case SyntaxKind.FieldDeclaration:
                    return GetAccessibility((FieldDeclarationSyntax)declaration);
                case SyntaxKind.ClassDeclaration:
                    return GetAccessibility((ClassDeclarationSyntax)declaration);
                case SyntaxKind.StructDeclaration:
                    return GetAccessibility((StructDeclarationSyntax)declaration);
                case SyntaxKind.InterfaceDeclaration:
                    return GetAccessibility((InterfaceDeclarationSyntax)declaration);
                case SyntaxKind.EnumDeclaration:
                    return GetAccessibility((EnumDeclarationSyntax)declaration);
                case SyntaxKind.DelegateDeclaration:
                    return GetAccessibility((DelegateDeclarationSyntax)declaration);
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.Public;
            }

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                throw new ArgumentNullException(nameof(classDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(classDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(classDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            SyntaxTokenList modifiers = constructorDeclaration.Modifiers;

            if (modifiers.Contains(SyntaxKind.StaticKeyword))
                return Accessibility.Private;

            Accessibility accessibility = GetExplicitAccessibility(modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(constructorDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(delegateDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(delegateDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                throw new ArgumentNullException(nameof(enumDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(enumDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(enumDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            if (eventDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            Accessibility accessibility = GetExplicitAccessibility(eventDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(eventDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetExplicitAccessibility(eventFieldDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(eventFieldDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(fieldDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(fieldDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetExplicitAccessibility(indexerDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(indexerDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                throw new ArgumentNullException(nameof(interfaceDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(interfaceDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(interfaceDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            SyntaxTokenList modifiers = methodDeclaration.Modifiers;

            if (modifiers.Contains(SyntaxKind.PartialKeyword))
                return Accessibility.Private;

            if (methodDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetExplicitAccessibility(methodDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(methodDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetExplicitAccessibility(propertyDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(propertyDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                throw new ArgumentNullException(nameof(structDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(structDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(structDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                throw new ArgumentNullException(nameof(accessorDeclaration));

            Accessibility accessibility = GetExplicitAccessibility(accessorDeclaration.Modifiers);

            SyntaxNode declaration = accessorDeclaration.Parent?.Parent;

            if (declaration == null)
                return accessibility;

            Accessibility declarationAccessibility = GetDeclarationAccessibility();

            if (declarationAccessibility == Accessibility.NotApplicable)
                return accessibility;

            return (accessibility.IsMoreRestrictiveThan(declarationAccessibility))
                ? accessibility
                : declarationAccessibility;

            Accessibility GetDeclarationAccessibility()
            {
                switch (declaration.Kind())
                {
                    case SyntaxKind.PropertyDeclaration:
                        return GetAccessibility((PropertyDeclarationSyntax)declaration);
                    case SyntaxKind.IndexerDeclaration:
                        return GetAccessibility((IndexerDeclarationSyntax)declaration);
                    case SyntaxKind.EventDeclaration:
                        return GetAccessibility((EventDeclarationSyntax)declaration);
                }

                Debug.Fail(declaration.Kind().ToString());

                return Accessibility.NotApplicable;
            }
        }

        internal static Accessibility GetExplicitAccessibility(SyntaxTokenList modifiers)
        {
            int count = modifiers.Count;

            for (int i = 0; i < count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            return Accessibility.Public;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedAndInternal;
                            }

                            return Accessibility.Private;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedOrInternal;
                            }

                            return Accessibility.Internal;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                switch (modifiers[j].Kind())
                                {
                                    case SyntaxKind.InternalKeyword:
                                        return Accessibility.ProtectedOrInternal;
                                    case SyntaxKind.PrivateKeyword:
                                        return Accessibility.ProtectedAndInternal;
                                }
                            }

                            return Accessibility.Protected;
                        }
                }
            }

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetExplicitAccessibility(SyntaxNode declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            return SyntaxInfo.ModifiersInfo(declaration).ExplicitAccessibility;
        }

        public static Accessibility GetExplicitAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                throw new ArgumentNullException(nameof(classDeclaration));

            return GetExplicitAccessibility(classDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            return GetExplicitAccessibility(constructorDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return GetExplicitAccessibility(conversionOperatorDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            return GetExplicitAccessibility(delegateDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return GetExplicitAccessibility(destructorDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                throw new ArgumentNullException(nameof(enumDeclaration));

            return GetExplicitAccessibility(enumDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            return GetExplicitAccessibility(eventDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            return GetExplicitAccessibility(eventFieldDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            return GetExplicitAccessibility(fieldDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            return GetExplicitAccessibility(indexerDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                throw new ArgumentNullException(nameof(interfaceDeclaration));

            return GetExplicitAccessibility(interfaceDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            return GetExplicitAccessibility(methodDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return GetExplicitAccessibility(operatorDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            return GetExplicitAccessibility(propertyDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                throw new ArgumentNullException(nameof(structDeclaration));

            return GetExplicitAccessibility(structDeclaration.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(IncompleteMemberSyntax incompleteMember)
        {
            if (incompleteMember == null)
                throw new ArgumentNullException(nameof(incompleteMember));

            return GetExplicitAccessibility(incompleteMember.Modifiers);
        }

        public static Accessibility GetExplicitAccessibility(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                throw new ArgumentNullException(nameof(accessorDeclaration));

            return GetExplicitAccessibility(accessorDeclaration.Modifiers);
        }

        public static bool IsPubliclyVisible(MemberDeclarationSyntax declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            do
            {
                if (declaration.IsKind(SyntaxKind.NamespaceDeclaration))
                    return true;

                Accessibility accessibility = GetAccessibility(declaration);

                if (accessibility == Accessibility.Public
                    || accessibility == Accessibility.Protected
                    || accessibility == Accessibility.ProtectedOrInternal)
                {
                    SyntaxNode parent = declaration.Parent;

                    if (parent != null)
                    {
                        if (parent.IsKind(SyntaxKind.CompilationUnit))
                            return true;

                        declaration = parent as MemberDeclarationSyntax;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            } while (declaration != null);

            return false;
        }

        public static TNode WithExplicitAccessibility<TNode>(
            TNode node,
            Accessibility newAccessibility,
            IModifierComparer comparer = null) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            AccessibilityInfo info = SyntaxInfo.AccessibilityInfo(node);

            if (!info.Success)
                throw new ArgumentException($"'{node.Kind()}' cannot have modifiers.", nameof(node));

            AccessibilityInfo newInfo = info.WithExplicitAccessibility(newAccessibility, comparer);

            return (TNode)newInfo.Node;
        }

        public static bool IsValidAccessibility(SyntaxNode node, Accessibility accessibility, bool ignoreOverride = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            switch (node.Parent?.Kind())
            {
                case SyntaxKind.NamespaceDeclaration:
                case SyntaxKind.CompilationUnit:
                    {
                        return accessibility.Is(Accessibility.Public, Accessibility.Internal);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        if (accessibility.ContainsProtected())
                            return false;

                        break;
                    }
            }

            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.EnumDeclaration:
                    {
                        return true;
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        var eventDeclaration = (EventDeclarationSyntax)node;

                        ModifierKinds kinds = SyntaxInfo.ModifiersInfo(eventDeclaration).GetKinds();

                        return (ignoreOverride || !kinds.Any(ModifierKinds.Override))
                            && (accessibility != Accessibility.Private || !kinds.Any(ModifierKinds.AbstractVirtualOverride))
                            && CheckProtectedInStaticOrSealedClass(node)
                            && CheckAccessorAccessibility(eventDeclaration.AccessorList);
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexerDeclaration = (IndexerDeclarationSyntax)node;

                        ModifierKinds kinds = SyntaxInfo.ModifiersInfo(indexerDeclaration).GetKinds();

                        return (ignoreOverride || !kinds.Any(ModifierKinds.Override))
                            && (accessibility != Accessibility.Private || !kinds.Any(ModifierKinds.AbstractVirtualOverride))
                            && CheckProtectedInStaticOrSealedClass(node)
                            && CheckAccessorAccessibility(indexerDeclaration.AccessorList);
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        var propertyDeclaration = (PropertyDeclarationSyntax)node;

                        ModifierKinds kinds = SyntaxInfo.ModifiersInfo(propertyDeclaration).GetKinds();

                        return (ignoreOverride || !kinds.Any(ModifierKinds.Override))
                            && (accessibility != Accessibility.Private || !kinds.Any(ModifierKinds.AbstractVirtualOverride))
                            && CheckProtectedInStaticOrSealedClass(node)
                            && CheckAccessorAccessibility(propertyDeclaration.AccessorList);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)node;

                        ModifierKinds kinds = SyntaxInfo.ModifiersInfo(methodDeclaration).GetKinds();

                        return (ignoreOverride || !kinds.Any(ModifierKinds.Override))
                            && (accessibility != Accessibility.Private || !kinds.Any(ModifierKinds.AbstractVirtualOverride))
                            && CheckProtectedInStaticOrSealedClass(node);
                    }
                case SyntaxKind.EventFieldDeclaration:
                    {
                        var eventFieldDeclaration = (EventFieldDeclarationSyntax)node;

                        ModifierKinds kinds = SyntaxInfo.ModifiersInfo(eventFieldDeclaration).GetKinds();

                        return (ignoreOverride || !kinds.Any(ModifierKinds.Override))
                            && (accessibility != Accessibility.Private || !kinds.Any(ModifierKinds.AbstractVirtualOverride))
                            && CheckProtectedInStaticOrSealedClass(node);
                    }
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.IncompleteMember:
                    {
                        return CheckProtectedInStaticOrSealedClass(node);
                    }
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    {
                        return accessibility == Accessibility.Public;
                    }
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    {
                        var memberDeclaration = node.Parent?.Parent as MemberDeclarationSyntax;

                        Debug.Assert(memberDeclaration != null, node.ToString());

                        if (memberDeclaration != null)
                        {
                            if (!CheckProtectedInStaticOrSealedClass(memberDeclaration))
                                return false;

                            return accessibility.IsMoreRestrictiveThan(GetAccessibility(memberDeclaration));
                        }

                        return false;
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        return false;
                    }
                default:
                    {
                        Debug.Fail(node.Kind().ToString());
                        return false;
                    }
            }

            bool CheckProtectedInStaticOrSealedClass(SyntaxNode declaration)
            {
                return !accessibility.ContainsProtected()
                    || (declaration.Parent as ClassDeclarationSyntax)?
                        .Modifiers
                        .ContainsAny(SyntaxKind.StaticKeyword, SyntaxKind.SealedKeyword) != true;
            }

            bool CheckAccessorAccessibility(AccessorListSyntax accessorList)
            {
                if (accessorList != null)
                {
                    foreach (AccessorDeclarationSyntax accessor in accessorList.Accessors)
                    {
                        Accessibility accessorAccessibility = GetExplicitAccessibility(accessor.Modifiers);

                        if (accessorAccessibility != Accessibility.NotApplicable)
                            return accessorAccessibility.IsMoreRestrictiveThan(accessibility);
                    }
                }

                return true;
            }
        }
    }
}
