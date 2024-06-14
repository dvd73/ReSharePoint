using System.Linq;
using JetBrains.ReSharper.Psi.Asp.Parsing;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Html.Tree;
using JetBrains.ReSharper.Psi.Web.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class IAspTagExtension
    {
        public static bool AttributeExists(this IAspTag tag, string attName)
        {
            ITagAttribute attribute = tag.GetAttribute(attName);
            return attribute?.ValueElement != null;
        }

        public static bool CheckAttributeValue(this IAspTag tag, string attName, string attValue, bool exactly = false)
        {
            ITagAttribute attribute = tag.GetAttribute(attName);
            return attribute?.ValueElement != null && (exactly && attribute.ValueElement.UnquotedValue.ToLower() == attValue.ToLower() ||
                                                       !exactly && attribute.ValueElement.UnquotedValue.ToLower().Contains(attValue.ToLower()));
        }

        public static void AddAttribute(this IAspTag tag, string attName, string attValue)
        {
            AspElementFactory elementFactory = AspElementFactory.GetInstance(tag.Language);
            ITagAttribute anchor = tag.GetAttributes().LastOrDefault();

            if (anchor != null)
            {
                ITagAttribute attOverwrite = elementFactory.CreateAttributeForTag(tag, attName, attValue);
                tag.AddAttributeAfter(attOverwrite, anchor);
            }
        }

        public static void EnsureAttribute(this IAspTag tag, string attName, string attValue)
        {
            AspElementFactory elementFactory = AspElementFactory.GetInstance(tag.Language);
            ITagAttribute attOverwrite = elementFactory.CreateAttributeForTag(tag, attName, attValue);
            ITagAttribute attribute = tag.GetAttribute(attName);

            if (attribute == null)
            {
                ITagAttribute anchor = tag.GetAttributes().LastOrDefault();

                if (anchor != null)
                {
                    tag.AddAttributeAfter(attOverwrite, anchor);
                }
            }
            else
            {
                IHtmlAttributeValue attributeValue = attribute.ValueElement;
                IHtmlAttributeValue valueElement = attOverwrite.ValueElement;
                if (attributeValue != null)
                {
                    ModificationUtil.ReplaceChild(attributeValue, valueElement);
                }
            }
        }
    }
}

