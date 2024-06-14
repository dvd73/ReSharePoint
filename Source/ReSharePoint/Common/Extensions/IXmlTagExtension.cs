using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Psi.Xml.Util;
using JetBrains.Util;

namespace ReSharePoint.Common.Extensions
{
    public static class IXmlTagExtension
    {
        public static bool AttributeExists(this IXmlTag tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && !attribute.Value.UnquotedValue.IsEmpty();
        }

        public static bool CheckAttributeValue(this IXmlTag tag, string attName, IEnumerable<string> attValues, bool exactly = false)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && (exactly && attValues.Any(attValue => attribute.UnquotedValue.ToLower() == attValue.ToLower()) ||
                                                !exactly &&
                                                attValues.Any(attValue => attribute.UnquotedValue.ToLower().Contains(attValue.ToLower())));
        }

        public static int GetAttributeValueLength(this IXmlTag tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            if (attribute?.Value != null)
                return attribute.UnquotedValue.Length;
            else
                return 0;
        }

        public static bool AttributeValueIsGuid(this IXmlTag tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && Guid.TryParse(attribute.UnquotedValue, out var dummy);
        }

        public static void EnsureAttribute(this IXmlTag tag, string attName, string attValue)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);

            if (attribute == null)
            {
                XmlElementFactory elementFactory = XmlElementFactory.GetInstance(tag);
                IXmlAttribute anchor = tag.GetAttributes().LastOrDefault();

                if (anchor != null)
                {
                    IXmlAttribute attOverwrite = elementFactory.CreateAttributeForTag(tag,
                        $"{attName}=\"{attValue}\"");
                    tag.AddAttributeAfter(attOverwrite, anchor);
                }
            }
            else
            {
                XmlAttributeUtil.SetValue(attribute, attValue);
            }
        }

        public static bool IsFieldDefinition(this IXmlTag tag)
        {
            IXmlTag parentTag = XmlTagContainerNavigator.GetByTag(tag) as IXmlTag;

            return tag.Header.ContainerName == "Field" && (parentTag == null ||
                                                           parentTag.Header.ContainerName != "Row");
        }

        public static void ReplaceTagContent(this IXmlTag tag, string newElementText)
        {
            XmlElementFactory elementFactory = XmlElementFactory.GetInstance(tag);
            var newElement = elementFactory.CreateRootTag(newElementText);
            var oldTextTokens = tag.InnerTextTokens;
            ModificationUtil.DeleteChildRange(oldTextTokens.First(), oldTextTokens.Last());
            var newTextTokens = TreeRange.Create(newElement.InnerTextTokens);
            ModificationUtil.AddChildRangeAfter(tag.Header, newTextTokens);
        }
    }
}
