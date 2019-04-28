using System;
using System.IO;

namespace DotnetGitTemplate 
{
    public class HybridRepository : IManageRepository
    {
        private readonly GitRepository gitRepo;

        public HybridRepository(GitRepository gitRepo) {
            this.gitRepo = gitRepo;
        }

        public void Clone(string repo, string branch)
        {
            if (!IsLocal(repo)) gitRepo.Clone(repo, branch);
        }

        private bool IsLocal(string repo)
        {
            return Directory.Exists(repo);
        }

        public string GetRepoPath(string repo)
        {
            return IsLocal(repo) ? repo : gitRepo.GetRepoPath(repo);
        }

        public void Pull(string repo)
        {
            if (!IsLocal(repo)) gitRepo.Pull(repo);
        }

        public void Remove(string repo)
        {
            if (!IsLocal(repo)) gitRepo.Remove(repo);
        }
    }

}