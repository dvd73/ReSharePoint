using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Threading;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Serialization;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache
{
    public abstract class SPXmlEntityCache<T> : SPPsiCacheBase<T> where T : SPXmlEntity
    {
        #region Properties
        protected virtual string XmlSchemaName => "http://schemas.microsoft.com/sharepoint";
        protected abstract string XmlSchemaContainerXPath { get; }
        protected abstract string XmlEntityXPath { get; }
        #endregion

        #region Ctor

        protected SPXmlEntityCache(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager) 
            : base(lifetime, locks, persistentIndexManager)
        {
        }
        #endregion

        #region Methods

        public virtual IEnumerable<IPsiSourceFile> GetDuplicates(IXmlTag element, string attributeName, bool caseSensitive)
        {
            List<IPsiSourceFile> result = new List<IPsiSourceFile>();
            IXmlAttribute attribute = element.GetAttribute(attributeName);
            string attValue = attribute.UnquotedValue.Trim();
            IPsiSourceFile sourceFile = element.GetSourceFile();
            TreeOffset offset = element.GetTreeStartOffset();

            lock (lockObject)
            {
                IEnumerable<T> keys =
                    ItemsToProjectFiles.Keys.Where(
                        key =>
                            String.Equals(key.GetPropertyValue(attributeName), attValue,
                                caseSensitive ? StringComparison.CurrentCulture : StringComparison.InvariantCultureIgnoreCase));

                foreach (T key in keys)
                {
                    ICollection<IPsiSourceFile> files = ItemsToProjectFiles.GetValuesSafe(key);
                    result.AddRange(
                        files.Where(
                            file =>
                                !file.Equals(sourceFile) ||
                                (file.Equals(sourceFile) && !key.Offset.Equals(offset.Offset))));
                }
            }

            if (result.Count == 0)
                return EmptyList<IPsiSourceFile>.InstanceList;
            else
                return result;
        }

        #region Implementation details

        protected override bool IsApplicable(IPsiSourceFile sourceFile)
        {
            bool result = false;

            try
            {
                IFile file = sourceFile.GetDominantPsiFile<XmlLanguage>();
                if (file is IXmlFile xmlFile)
                {
                    IXmlTag validatedTag = xmlFile.GetNestedTags<IXmlTag>(XmlSchemaContainerXPath).FirstOrDefault();

                    if (validatedTag != null)
                    {
                        // check xmlns="http://schemas.microsoft.com/sharepoint/" to be sure this is xml with sharepoint schema
                        result = validatedTag.CheckAttributeValue("xmlns", new[] {XmlSchemaName});

                        if (result)
                            result = xmlFile.GetNestedTags<IXmlTag>(XmlEntityXPath).Any();
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public abstract class SPXmlEntity : SPPsiCacheItem
    {
        public abstract string GetPropertyValue(string attributeName);

        protected SPXmlEntity(UnsafeReader reader)
            : base(reader)
        {
        }

        protected SPXmlEntity() 
            : base()
        {
            
        }
    }
}
