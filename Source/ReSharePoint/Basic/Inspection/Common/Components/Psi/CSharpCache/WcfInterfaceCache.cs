using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Threading;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Serialization;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi.CSharpCache
{
    [PsiComponent]
    [Applicability(
       IDEProjectType.SPFarmSolution  )]
    public class WcfInterfaceCache : SPPsiCacheBase<SPWcfInterface>
    {
        #region Properties
        protected override string CacheDirectoryName => "WcfInterfaces";

        protected override IEqualityComparer<SPWcfInterface> ItemsEqualityComparer => new WcfInterfaceCacheEqualityComparer();

        public static WcfInterfaceCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<WcfInterfaceCache>();
        } 
        #endregion

        #region Ctor
        public WcfInterfaceCache(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, locks, persistentIndexManager)
        {
        }
        #endregion

        #region Methods

        #region Overrided methods
        protected override SPPsiCacheItems<SPWcfInterface> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<SPWcfInterface> jetHashSet = null;
            IFile file = sourceFile.GetDominantPsiFile<CSharpLanguage>();

            if (file != null)
            {
                ICSharpFile xmlFile = file as ICSharpFile;

                foreach (var __ in xmlFile.NamespaceDeclarationsEnumerable)
                {
                    foreach (var _ in __.TypeDeclarationsEnumerable.Where(
                        _ => _ is IInterfaceDeclaration && CheckInterfaceAttributes(_)))
                    {
                        if (jetHashSet == null)
                            jetHashSet = new JetHashSet<SPWcfInterface>(ItemsEqualityComparer);

                        jetHashSet.Add(new SPWcfInterface()
                        {
                            Title = _.CLRName,
                            Offset = _.GetTreeStartOffset().Offset,
                            SourceFileFullPath = sourceFile.GetLocation().Directory.FullPath,
                            SourceFileName = sourceFile.Name
                        });
                    }
                }
            }

            if (jetHashSet == null)
                return null;
            else
                return new SPPsiCacheItems<SPWcfInterface>(jetHashSet.ToArray());
        }

        protected override bool IsApplicable(IPsiSourceFile sourceFile)
        {
            bool result = false;

            try
            {
                IFile file = sourceFile.GetDominantPsiFile<CSharpLanguage>();
                if (file != null)
                {
                    if (file is ICSharpFile xmlFile)
                    {
                        result =
                            xmlFile.NamespaceDeclarationsEnumerable.Any(__ => __.TypeDeclarations.Any(
                                _ => _ is IInterfaceDeclaration && CheckInterfaceAttributes(_)));
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        #endregion

        #region Implementation details

        private bool CheckInterfaceAttributes(ICSharpTypeDeclaration cSharpTypeDeclaration)
        {
            return cSharpTypeDeclaration.AttributesEnumerable.Any(_ => _.Name.ShortName == "ServiceContract");
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class SPWcfInterface : SPPsiCacheItem
    {
        public string Title { get; set; }

        public SPWcfInterface(UnsafeReader reader)
            : base(reader)
        {
            Title = reader.ReadString();
        }

        public SPWcfInterface() : base()
        {
            
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Title);
        }
    }

    public class WcfInterfaceCacheEqualityComparer : IEqualityComparer<SPWcfInterface>
    {
        public bool Equals(SPWcfInterface x, SPWcfInterface y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Title.Trim(), y.Title.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(SPWcfInterface obj)
        {
            return (obj.Title.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
