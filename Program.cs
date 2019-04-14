using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.CommandLineUtils;

namespace DotnetGitTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = AppConfig.ReadFrom(GetConfigurationPath());
            var repoManager = new GitRepository(config, DotnetGitTemplate.GetDataFolder());
            var templateManager = new TemplateManager();
            var core = new DotnetGitTemplate(repoManager, templateManager, config);

            var app = new CommandLineApplication();
            app.Name = "DotnetGitTemplate";
            app.Description = "Work with dotnet templates from git repositories.";
            app.HelpOption("-?|-h|--help");
            
            app.Command("add", cmd => {
                cmd.Description = "Add templates from git repository";
                cmd.HelpOption("-?|-h|--help");
                cmd.Argument("gitrepo", "Git repository that has the dotnet templates", false);
                cmd.OnExecute(() =>
                {
                    string repo = cmd.Arguments[0].Value;
                    core.Add(repo);
                    return 0;
                });
            });

            app.Command("remove", cmd => {
                cmd.Description = "Remove templates originated from the git repository";
                cmd.HelpOption("-?|-h|--help");
                cmd.Argument("gitrepo", "Git repository that has the dotnet templates", false);
                cmd.OnExecute(() => {
                    var repo = cmd.Arguments[0].Value;
                    core.Remove(repo);
                    return 0;
                });
            });

            app.Command("list", cmd => {
                cmd.Description = "List repositories";
                cmd.OnExecute(() => {
                    core.List();
                    return 0;
                });
            });

            app.Command("update", cmd => {
                cmd.Description = "Update templates from all repositories";
                cmd.OnExecute(() => {
                    core.Update();
                    return 0;
                });
            });
            
            try {
                app.Execute(args);
            } catch(Exception e) {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            } finally {
                AppConfig.Save(config, GetConfigurationPath());
            }
        }

        private static string GetConfigurationPath() 
        {
            return Path.Combine(DotnetGitTemplate.GetDataFolder(), "repositories.txt");
        }
    }
}
