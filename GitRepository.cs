using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DotnetGitTemplate
{
    public class GitRepository : IManageRepository
    {
        private readonly IAppConfig config;
        private readonly string rootPath;

        public GitRepository(IAppConfig config, string rootPath)
        {
            this.config = config;
            this.rootPath = rootPath;
        }

        public void Clone(string repo, string branch)
        {
            var name = GetRepoName(repo);
            var info = new ProcessStartInfo("git", $"clone --depth 1 -b {branch} -o {name} {repo}");
            info.WorkingDirectory = rootPath;
            Process.Start(info).WaitForExit();
            
        }

        public string GetRepoName(string repo)
        {
            return repo.Split("/").Last().Replace(".git", "");
        }

        public void Pull(string repo)
        {
            var repoPath = GetRepoPath(repo);
            if (!Directory.Exists(repoPath)) return;
            
            var info = new ProcessStartInfo("git", "pull");
            info.WorkingDirectory = repoPath;
            Process.Start(info).WaitForExit();
        }

        public string GetRepoPath(string repo)
        {
            return System.IO.Path.Combine(rootPath, GetRepoName(repo));
        }
    }
}