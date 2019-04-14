using System.Collections.Generic;

namespace DotnetGitTemplate
{
    public interface IAppConfig
    {
        IDictionary<string, string[]> Repositories { get; }
    }
}