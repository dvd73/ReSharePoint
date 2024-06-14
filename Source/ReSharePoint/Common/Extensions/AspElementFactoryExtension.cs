using System;
using System.Linq;
using JetBrains.ReSharper.Psi.Asp.Parsing;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.Html.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class AspElementFactoryExtension
    {
        public static ITagAttribute CreateAttributeForTag(this AspElementFactory elementFactory, IAspTag src, string attName, string attValue)
        {
            return
                elementFactory.CreateHtmlTag($"<dummy {attName}=\"{attValue}\" />", src)
                    .Attributes.First();
        }
    }
}
