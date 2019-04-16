namespace DotnetGitTemplate 
{

    public interface IManageRepository
    {
        void Clone(string repo, string branch);
        void Pull(string clone);

        string GetRepoPath(string repo);

        string GetRepoName(string repo);
    }

}