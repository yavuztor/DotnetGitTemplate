using System;
using System.IO;
using System.Linq;

namespace DotnetGitTemplate
{
    public class DotnetGitTemplate
    {
        private readonly IManageRepository repoManager;
        private readonly IManageTemplates templateManager;
        private readonly IAppConfig config;

        public DotnetGitTemplate(IManageRepository repoManager, IManageTemplates templateManager, IAppConfig config)
        {
            this.repoManager = repoManager;
            this.templateManager = templateManager;
            this.config = config;
        }

        public void Add(string repo) 
        {
            Remove(repo);
            var repoPath = repoManager.GetRepoPath(repo);
            repoManager.Clone(repo);
            var templates = templateManager.RegisterTemplates(repoPath);
            config.Repositories.Add(repo, templates);
        }

        public void Remove(string repo)
        {
            if (config.Repositories.ContainsKey(repo)) config.Repositories.Remove(repo);
            var repoPath = repoManager.GetRepoPath(repo);
            if (!Directory.Exists(repoPath)) return;

            templateManager.DeregisterTemplates(repoPath);
            Directory.Delete(repoPath, true);
        }

        public void List()
        {
            foreach (var repo in config.Repositories.Keys)
            {
                Console.WriteLine(repo);
                Console.WriteLine(string.Join("\r\n", config.Repositories[repo].Select(t => $"\t{t}")));
            }
        }

        public void Update()
        {
            var repositories = config.Repositories.Keys.ToArray();
            foreach (var repo in repositories)
            {
                templateManager.DeregisterTemplates(repo);
                templateManager.RegisterTemplates(repo);
            }

        }

        public static string GetDataFolder()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appData, nameof(DotnetGitTemplate));
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }
    }
}