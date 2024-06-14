using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Util;

namespace ReSharePoint.Common.Extensions
{
    public static class IXmlAttributeContainerExtension
    {
        public static bool AttributeExists(this IXmlAttributeContainer tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && !attribute.Value.UnquotedValue.IsEmpty();
        }

        public static bool CheckAttributeValue(this IXmlAttributeContainer tag, string attName, IEnumerable<string> attValues, bool exactly = false)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && (exactly && attValues.Any(attValue => attribute.UnquotedValue.ToLower() == attValue.ToLower()) ||
                                                !exactly &&
                                                attValues.Any(attValue => attribute.UnquotedValue.ToLower().Contains(attValue.ToLower())));
        }

        public static int GetAttributeValueLength(this IXmlAttributeContainer tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            if (attribute?.Value != null)
                return attribute.UnquotedValue.Length;
            else
                return 0;
        }

        public static bool AttributeValueIsGuid(this IXmlAttributeContainer tag, string attName)
        {
            IXmlAttribute attribute = tag.GetAttribute(attName);
            return attribute?.Value != null && Guid.TryParse(attribute.UnquotedValue, out var dummy);
        }
    }
}
