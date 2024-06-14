using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Caches;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Solution
{
    public abstract class SPProjectFileItemCollectionDataProvider<T> : IProjectFileDataProvider<ICollection<T>> where T : SPProjectFileCacheItem
    {
        private readonly ISolution mySolution;
        private readonly IProjectFileDataCache myCache;

        protected string ProjectLocation { get; set; }

        protected SPProjectFileItemCollectionDataProvider(Lifetime lifetime, ISolution solution, IProjectFileDataCache cache)
        {
            myCache = cache;
            mySolution = solution;
            cache.RegisterCache(lifetime, this);
        }

        /// <summary>
        /// ReSharper will also call Read method at the appropriate time, and you should use the Binary Reader to recreate the object that you need to cache. 
        /// Again, don’t store anything in class fields here.
        /// </summary>
        /// <param name="projectFileLocation"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual ICollection<T> Read(FileSystemPath projectFileLocation, BinaryReader reader)
        {
            int itemsCount = reader.ReadInt32();
            if (itemsCount == 0)
                return EmptyList<T>.InstanceList;

            List<T> list = new List<T>(itemsCount);

            for (int i = 0; i < itemsCount; ++i)
            {
                int sizeofT = reader.ReadInt32();
                list.Add(DeserializeFromBytes(reader.ReadBytes(sizeofT)));
            }

            return list;
        }

        /// <summary>
        /// ReSharper will call your Write method, passing in the cached object. 
        /// You serialise this to the BinaryWriter however you want to.
        /// </summary>
        /// <param name="projectFileLocation"></param>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        public virtual void Write(FileSystemPath projectFileLocation, BinaryWriter writer, ICollection<T> data)
        {
            var items = data.ToArray();
            writer.Write(items.Length);
            foreach (var item in items)
            {
                byte[] rawdata = SerializeToBytes(item);
                writer.Write(rawdata.Length);
                writer.Write(rawdata);
            }
        }

        public virtual bool CanHandle(FileSystemPath projectFileLocation)
        {
            bool result = false;

            IProject project =
                mySolution.FindProjectItemsByLocation(projectFileLocation)
                    .OfType<IProjectFile>()
                    .Select(projectFile => projectFile.GetProject())
                    .WhereNotNull()
                    .FirstOrDefault();
            if (project != null)
            {
                result = project.IsClassicSolution();
                ProjectLocation = projectFileLocation.Directory.FileAccessPath;
            }

            return result;
        }

        /// <summary>
        /// Return an object that represents your cached value(s).
        /// This could be a boolean, or a class. Don’t store anything in your class fields. ReSharper will cache this value.
        /// If the object instance is different to the previously cached instance, your OnDataChanged method is called.
        /// </summary>
        /// <param name="projectFileLocation"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public abstract ICollection<T> BuildData(FileSystemPath projectFileLocation, XmlDocument doc);

        /// <summary>
        /// You should implement OnDataChanged only if you need to do something when the data changes. 
        /// If there’s no way for the user to change this value while the project is open, 
        /// then I suspect it’ll be fine to ignore the method and just return null.
        /// </summary>
        /// <param name="projectFileLocation"></param>
        /// <param name="oldData"></param>
        /// <param name="newData"></param>
        /// <returns>You can decide what needs to happen in response to this change, and you should return it as an action that will get executed later (after all processing of the file completes).</returns>
        public virtual Action OnDataChanged(FileSystemPath projectFileLocation, ICollection<T> oldData, ICollection<T> newData)
        {
            /*
            if (oldData != null)
            {
                bool oldValue = (bool)oldData;
                bool newValue = (bool)newData;

                if (newValue != oldValue)
                    return () => myLocks.ExecuteWithWriteLock(() =>
                    {
                        IProject project = projectFile.GetProject();
                        Assertion.AssertNotNull(project, "project must not be null");

                        // ProjectModelChangeUtil.OnChange is just a helper function that will batch up changes to the change manager. 
                        // It ensures that all changes are "propagated", e.g. if given a change notification for a project item, 
                        // it makes sure that gets propagated up to the project itself. 
                        // It then uses IProjectModelBatchChangeManager to post the notification, 
                        // which will either add the change to a current transaction, or execute it in its own, new transaction. 
                        // I.e. the change happens as part of a new batch, of gets added to the existing batch. 
                        // The batch manager then posts it to the change manager, which is an event bus that components can subscribe to in order to be notified of changes.
                        // You can see the change manager subscriptions as a graph from the internal menu (ReSharper -> Internal -> View Change Manager Graph). 
                        ProjectModelChangeUtil.OnChange(projectFile.GetSolution().BatchChangeManager,
                            new ProjectItemChange(EmptyList<ProjectModelChange>.InstanceList, project,
                                project.ParentFolder, ProjectModelChangeType.PROPERTIES, project.Location,
                                project.GetPersistentID()));
                    });
            }
            */
            return null;
        }

        public virtual int Version => 3;

        public virtual ICollection<T> GetCacheContent(IProject project)
        {
            return myCache.GetData(this, project, EmptyList<T>.Instance);
        }

        protected byte[] SerializeToBytes(T item)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        protected T DeserializeFromBytes(byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return formatter.Deserialize(stream) as T;
            }
        }
    }
}
