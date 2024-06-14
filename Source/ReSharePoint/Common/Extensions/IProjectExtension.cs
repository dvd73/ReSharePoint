using System;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Properties;
using JetBrains.ProjectModel.Properties.Managed;
using JetBrains.Util.Dotnet.TargetFrameworkIds;
using ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache;
using ReSharePoint.Common.Attributes;

namespace ReSharePoint.Common.Extensions
{
    public static class IProjectExtension
    {
        public static bool IsApplicableFor(this IProject project, object actionObject, TargetFrameworkId targetFrameworkId)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(targetFrameworkId.Name))
            {
                IManagedProjectConfiguration projectConfiguration =
                    project.ProjectProperties.ActiveConfigurations.GetOrCreateConfiguration(targetFrameworkId) as
                        IManagedProjectConfiguration;

                if (actionObject.GetType().GetCustomAttributes(typeof(ApplicabilityAttribute), true).FirstOrDefault() is
                    ApplicabilityAttribute oAttribute)
                {
                    #region Current project type variables

                    bool hasSPServerReference = project.HasSPServerReference() &&
                                                (projectConfiguration.OutputType == ProjectOutputType.LIBRARY ||
                                                 projectConfiguration.OutputType == ProjectOutputType.CONSOLE_EXE ||
                                                 projectConfiguration.OutputType == ProjectOutputType.WIN_EXE);
                    bool isSPFarmSolution = /*hasSPServerReference &&*/
                        projectConfiguration.OutputType == ProjectOutputType.LIBRARY && !project.IsSandboxed() &&
                        project.ProjectProperties.ProjectTypeGuids.All(t =>
                            Consts.Consts.SPFarmSolutionProjectTypeGuids.Contains(t));
                    bool isSPAppSolution = project.HasSPClientReference() &&
                                           projectConfiguration.OutputType == ProjectOutputType.LIBRARY &&
                                           project.ProjectProperties.ProjectTypeGuids.All(t =>
                                               Consts.Consts.SPAppsSolutionProjectTypeGuids.Contains(t));
                    bool isSandboxProject = project.IsSharepointWorkflow() && project.IsSandboxed();

                    #endregion

                    if (isSPFarmSolution)
                    {
                        result = (oAttribute.ProjectType & IDEProjectType.SPFarmSolution) ==
                                 IDEProjectType.SPFarmSolution;
                    }
                    else if (isSPAppSolution)
                    {
                        result = (oAttribute.ProjectType & IDEProjectType.SPApp) ==
                                 IDEProjectType.SPApp;
                    }
                    else if (isSandboxProject)
                    {
                        result = (oAttribute.ProjectType & IDEProjectType.SPSandbox) ==
                                 IDEProjectType.SPSandbox;
                    }
                    else if (hasSPServerReference)
                    {
                        result = (oAttribute.ProjectType & IDEProjectType.SPServerAPIReferenced) ==
                                 IDEProjectType.SPServerAPIReferenced;
                    }

                }
            }

            return result;
        }

        public static bool IsClassicSolution(this IProject project)
        {
            bool result = false;

            foreach (TargetFrameworkId targetFrameworkId in project.TargetFrameworkIds)
            {
                if (!String.IsNullOrEmpty(targetFrameworkId.Name))
                {
                    IManagedProjectConfiguration projectConfiguration =
                        project.ProjectProperties.ActiveConfigurations.GetOrCreateConfiguration(targetFrameworkId) as
                            IManagedProjectConfiguration;

                    result |= /*project.HasSPServerReference() &&*/
                        projectConfiguration.OutputType == ProjectOutputType.LIBRARY && (
                            project.ProjectProperties.ProjectTypeGuids.All(
                                t => Consts.Consts.SPFarmSolutionProjectTypeGuids.Contains(t)));
                }
            }

            return result;
        }

        public static bool IsSPFarmSolution(this IProject project)
        {
            bool result = false;

            foreach (TargetFrameworkId targetFrameworkId in project.TargetFrameworkIds)
            {
                if (!String.IsNullOrEmpty(targetFrameworkId.Name))
                {
                    IManagedProjectConfiguration projectConfiguration =
                        project.ProjectProperties.ActiveConfigurations.GetOrCreateConfiguration(targetFrameworkId) as
                            IManagedProjectConfiguration;

                    result |= projectConfiguration.OutputType == ProjectOutputType.LIBRARY && !project.IsSandboxed() &&
                              project.ProjectProperties.ProjectTypeGuids.All(
                                  t => Consts.Consts.SPFarmSolutionProjectTypeGuids.Contains(t));
                }
            }

            return result;
        }

        public static bool HasSPClientReference(this IProject project)
        {
            return project.GetAllReferencedAssemblies().Any(r => r.Name == "Microsoft.SharePoint.Client.Runtime");
        }

        public static bool HasSPServerReference(this IProject project)
        {
            return project.GetAllReferencedAssemblies().Any(r => r.Name == "Microsoft.SharePoint");
        }

        public static bool HasSecureStoreServiceReference(this IProject project)
        {
            return project.GetAllReferencedAssemblies().Any(r => r.Name == "Microsoft.Office.SecureStoreService");
        }

        public static bool IsSandboxed(this IProject project)
        {
            bool result = false;

            ISolution solution = project.GetSolution();
            var solutionComponent = solution.GetComponent<SandboxedSolutionProvider>();
            SandboxedOptions options = solutionComponent.GetCacheContent(project);
            result = options != null && options.IsSandboxed;

            //if (project.ProjectFile != null)
            //    using (Stream stream = project.ProjectFile.CreateReadStream())
            //    {
            //        XDocument document = XDocument.Load(stream);
            //        XElement sandboxedSolutionNode = document.Descendants().FirstOrDefault(p => p.Name.LocalName == "SandboxedSolution");

            //        if (sandboxedSolutionNode != null && !String.IsNullOrEmpty(sandboxedSolutionNode.Value))
            //            result = sandboxedSolutionNode.Value.ToLower() == "true";
            //    }

            return result;
        }

        public static string GetOutputAssemblyFullName(this IProject project)
        {
            string result;

            var outputAssemblyInfo = project.GetOutputAssemblyInfo(TargetFrameworkId.Default);
            if (outputAssemblyInfo != null && outputAssemblyInfo.AssemblyNameInfo != null)
                result = outputAssemblyInfo.AssemblyNameInfo.FullName;
            else
                result = project.Guid.ToString();

            return result;
        }
    }
}
