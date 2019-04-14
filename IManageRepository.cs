namespace DotnetGitTemplate 
{

    public interface IManageRepository
    {
        void Clone(string repo);
        void Pull(string clone);

        string GetRepoPath(string repo);

        string GetRepoName(string repo);
    }

}