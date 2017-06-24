using EnvDTE;
using EnvDTE80;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CLog.UI.Framework.Testing.Helpers
{
    public static class VisualStudioHelper
    {
        #region Fields

        public const string PROJECT_KIND_SOLUTION_FOLDER = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}"; //ProjectKinds.vsProjectKindSolutionFolder

        public const string PROJECT_KIND_CSHARP_PROJECT = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        #endregion

        #region Methods

        public static string[] GetSolutionOutputAssemblies()
        {
            List<Project> projects = GetSolutionProjects();

            string[] assemblies = projects
                .Where(p => p.Kind == PROJECT_KIND_CSHARP_PROJECT)
                .Select(p => GetAssemblyPath(p))
                .ToArray();

            return assemblies;
        }

        public static DTE2 GetActiveIDE()
        {
            // Get an instance of currently running Visual Studio IDE.
            //DTE2 dte2 = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.14.0"); // 2015
            DTE2 dte2 = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.15.0"); // 2017
            // TODO:  find a way to obtain dynamically

            return dte2;
        }

        public static List<Project> GetSolutionProjects()
        {
            Projects projects = null;
            RetryHelper.Retry(5, 100, () => projects = GetActiveIDE().Solution.Projects);

            List<Project> list = new List<Project>();
            IEnumerator item = projects.GetEnumerator();

            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                    continue;

                if (project.Kind == PROJECT_KIND_SOLUTION_FOLDER)
                    list.AddRange(GetSolutionFolderProjects(project));
                else
                    list.Add(project);
            }

            return list;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();

            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                    continue;

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == PROJECT_KIND_SOLUTION_FOLDER)
                    list.AddRange(GetSolutionFolderProjects(subProject));
                else
                    list.Add(subProject);
            }

            return list;
        }

        private static string GetAssemblyPath(Project project)
        {
            if (project.Kind != PROJECT_KIND_CSHARP_PROJECT)
                return null;

            string assemblyPath = null;

            RetryHelper.Retry(5, 100, () =>
            {
                string fullPath = project.Properties.Item("FullPath").Value.ToString();
                string outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
                string outputDir = Path.Combine(fullPath, outputPath);
                string outputFileName = project.Properties.Item("OutputFileName").Value.ToString();

                assemblyPath = Path.Combine(outputDir, outputFileName);
            });

            return assemblyPath;
        }

        #endregion
    }
}
