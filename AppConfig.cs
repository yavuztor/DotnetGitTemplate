using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotnetGitTemplate
{
    public class AppConfig : IAppConfig {
        public IDictionary<string, string[]> Repositories { get; private set; } = new Dictionary<string, string[]>();

        public static AppConfig ReadFrom(string filePath) 
        {
            var config = new AppConfig();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    var parts = line.Split("\t").Select(p => p.Trim());
                    config.Repositories.Add(parts.First(), parts.Skip(1).ToArray());
                }
            }
            return config;
        }

        public static void Save(IAppConfig config, string filepath) 
        {
            using (var writer = File.CreateText(filepath))
            {
                foreach (var repo in config.Repositories.Keys)
                {
                    var line = string.Join("\t", config.Repositories[repo].Prepend(repo));
                    writer.WriteLine(line);
                }
            }
        }
    }
}
