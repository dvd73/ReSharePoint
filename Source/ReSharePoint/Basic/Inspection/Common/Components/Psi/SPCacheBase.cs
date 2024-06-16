using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.changes;
using JetBrains.Application.Progress;
using JetBrains.Application.Threading;
using JetBrains.DataFlow;
using JetBrains.Diagnostics;
using JetBrains.DocumentManagers.impl;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Serialization;
using JetBrains.Util;
using JetBrains.Util.Caches;
using JetBrains.Util.Concurrency;
using JetBrains.Util.PersistentMap;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi
{
    public abstract class SPPsiCacheBase<T> : SimpleICache<SPPsiCacheItems<T>> where T : SPPsiCacheItem
    {
        #region Fields
        protected readonly OneToSetMap<IPsiSourceFile, T> ProjectFileToItems;
        protected readonly OneToSetMap<T, IPsiSourceFile> ItemsToProjectFiles;
        protected readonly LockObject lockObject = new LockObject();
        #endregion

        #region Properties
        protected abstract string CacheDirectoryName { get; }
        protected abstract IEqualityComparer<T> ItemsEqualityComparer { get; }
        
        public override string Version => "2";

        [IndexerName("SourceFiles")]
        public virtual IEnumerable<IPsiSourceFile> this[T item]
        {
            get
            {
                lock (lockObject)
                {
                    return ItemsToProjectFiles[item].ToArray();
                }
            }
        }

        public IEnumerable<T> Items => ItemsToProjectFiles.Keys;

        public IEnumerable<IPsiSourceFile> Files => ProjectFileToItems.Keys;

        #endregion

        #region Ctor

        protected SPPsiCacheBase(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, locks, persistentIndexManager, SPPsiCacheItems<T>.Marshaller)
        {
            #if DEBUG
            // TODO: Useful for testing. Remove for release
            ClearOnLoad = true;
            #endif

            Map.Cache = new UnlimitedCache<IPsiSourceFile, SPPsiCacheItems<T>>(persistentIndexManager.PsiSourceFilePersistentEqualityComparer);

            ProjectFileToItems = new OneToSetMap<IPsiSourceFile, T>();
            ItemsToProjectFiles = new OneToSetMap<T, IPsiSourceFile>(ItemsEqualityComparer);
        }

        #endregion

        #region Overrided methods

        public override object Build(IPsiSourceFile sourceFile, bool isStartup)
        {
            if (IsApplicable(sourceFile))
                return BuildData(sourceFile);
            else
            {
                return null;
            }
        }

        public override object Load(IProgressIndicator progress, bool enablePersistence)
        {
            var result = base.Load(progress, enablePersistence);

            if (Map.Cache.Count == 0)
            {
                ItemsToProjectFiles.Clear();
                ProjectFileToItems.Clear();
            }

            return result;
        }

        public override void Merge(IPsiSourceFile sourceFile, object builtPart)
        {
            base.Merge(sourceFile, builtPart);

            if (builtPart != null)
            {
                lock (lockObject)
                {
                    RemoveData(sourceFile);
                    var newItem = builtPart as SPPsiCacheItems<T>;
                    IList<T> newItems = newItem.Items;

                    Assertion.Assert((newItems != null ? 1 : 0) != 0, "newItems != null: {0}", builtPart);

                    AddData(sourceFile, newItems);
                }
            }
        }

        public override void Drop(IPsiSourceFile sourceFile)
        {
            base.Drop(sourceFile);

            lock (lockObject)
            {
                RemoveData(sourceFile);
            }
        }

        #endregion

        #region Implementation details

        protected virtual void RemoveData(IPsiSourceFile sourceFile)
        {
            ICollection<T> items = ProjectFileToItems[sourceFile];
            ProjectFileToItems.RemoveKey(sourceFile);

            foreach (T item in items)
                ItemsToProjectFiles.Remove(item, sourceFile);
        }

        protected virtual void AddData(IPsiSourceFile sourceFile, [NotNull] IList<T> newItems)
        {
            if (!newItems.Any())
                return;

            ProjectFileToItems.AddRange(sourceFile, newItems);

            foreach (T item in newItems)
                ItemsToProjectFiles.Add(item, sourceFile);
        }

        #endregion

        protected abstract SPPsiCacheItems<T> BuildData(IPsiSourceFile sourceFile);
    }

    [Serializable()]
    public class SPPsiCacheItems<T> where T : SPPsiCacheItem
    {
        public static readonly IUnsafeMarshaller<SPPsiCacheItems<T>> Marshaller =
            new UniversalMarshaller<SPPsiCacheItems<T>>(Read, Write);

        public IList<T> Items { get; }

        public SPPsiCacheItems(IList<T> items)
        {
            Items = items;
        }

        public bool IsEmpty => Items.Count == 0;

        private static SPPsiCacheItems<T> Read(UnsafeReader reader)
        {
            var items = reader.ReadCollection(InstantiateItem, count => new List<T>(count));
            return new SPPsiCacheItems<T>(items);
        }

        private static void Write(UnsafeWriter writer, SPPsiCacheItems<T> value)
        {
            writer.Write<T, ICollection<T>>((w, directive) => directive.Write(w), value.Items.ToList());
        }
        private static T InstantiateItem(UnsafeReader reader)
        {
            return (T)Activator.CreateInstance(typeof(T), reader);
        }
    }

    [Serializable()]
    public class SPPsiCacheItem
    {
        public int Offset { get; set; }
        public string SourceFileName { get; set; }
        public string SourceFileFullPath { get; set; }

        public virtual void Write(UnsafeWriter writer)
        {
            writer.Write(Offset);
            writer.Write(SourceFileName);
            writer.Write(SourceFileFullPath);
        }

        public SPPsiCacheItem(UnsafeReader reader)
        {
            Offset = reader.ReadInt();
            SourceFileName = reader.ReadString();
            SourceFileFullPath = reader.ReadString();
        }

        protected SPPsiCacheItem()
        {
            
        }
    }
}
