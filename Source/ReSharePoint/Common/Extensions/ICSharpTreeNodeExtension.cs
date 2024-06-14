using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class ICSharpTreeNodeExtension
    {
        public static string ContainerReadableName(this ICSharpTreeNode element)
        {
            return
                $"{element.GetContainingTypeDeclaration().CLRName}: {element.GetContainingTypeMemberDeclarationIgnoringClosures().DeclaredName}";
        }
    }
}
