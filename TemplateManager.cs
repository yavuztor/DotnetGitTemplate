using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace DotnetGitTemplate
{
    public class TemplateManager : IManageTemplates
    {
        public void DeregisterTemplates(string rootPath)
        {
            ScanFolderForTemplate(rootPath, path => Uninstall(path));
        }

        public void Install(string templatePath)
        {
            Process.Start("dotnet", $"new -i {templatePath}").WaitForExit();
        }

        public string[] RegisterTemplates(string rootPath)
        {
            List<string> results = new List<string>();
            ScanFolderForTemplate(rootPath, path => {
                Install(path);
                results.Add(GetDotnetTemplateName(path));
            });
            return results.ToArray();
        }

        public void Uninstall(string templatePath)
        {
            Process.Start("dotnet", $"new -u {templatePath}").WaitForExit();
        }

        private static void ScanFolderForTemplate(string templatePath, Action<string> action)
        {
            if (!Directory.Exists(templatePath)) return;

            string folderPath = Path.Combine(templatePath, ".template.config");
            if (Directory.Exists(folderPath))
            {
                action(templatePath);
            }
            else
            {
                foreach (var subfolder in Directory.GetDirectories(templatePath))
                {
                    ScanFolderForTemplate(subfolder, action);
                }
            }
        }

        private static string GetDotnetTemplateName(string templatePath)
        {
            var regex = new Regex("\"shortName\"\\s*:\\s*\"([^\"]+)\"");
            var jsonPath = Path.Combine(templatePath, ".template.config", "template.json");
            var json = File.ReadAllText(jsonPath);
            var match = regex.Match(json);
            if (match.Success) 
            {
                return match.Groups[1].Value;
            }
            throw new Exception($"Cannot retrieve shortName from {jsonPath}");
        }
    }
}