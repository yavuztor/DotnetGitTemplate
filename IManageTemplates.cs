namespace DotnetGitTemplate 
{

    public interface IManageTemplates
    {
        void Install(string templatePath);
        void Uninstall(string templatePath);

        string[] RegisterTemplates(string rootPath);

        void DeregisterTemplates(string rootPath);
    }

}