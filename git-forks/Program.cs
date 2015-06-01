using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using Octokit;

namespace git_forks
{
    class Program
    {
        static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static void Main(string[] args)
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);
            var repositoryFound = false;
            while (!repositoryFound && directory != null)
            {
                repositoryFound = LibGit2Sharp.Repository.IsValid(directory.FullName);
                if (!repositoryFound)
                    directory = directory.Parent;
            }

            if (!repositoryFound)
            {
                Console.WriteLine("The current directory is not a valid git repository.");
                return;
            }

            var localRepository = new LibGit2Sharp.Repository(directory.FullName);
            var remotes = localRepository.Network.Remotes;
            var gitHubRemote = remotes.FirstOrDefault(x => new Uri(x.Url).Host == "github.com");
            if (gitHubRemote == null)
            {
                Console.WriteLine("This repository has no remotes that can be found on GitHub.");
                return;
            }

            var splits = gitHubRemote.Url.Split('/');
            var remoteUser = splits[splits.Length - 2];
            var remoteRepositoryName = splits[splits.Length - 1];
            if (remoteRepositoryName.EndsWith(".git"))
            {
                remoteRepositoryName = remoteRepositoryName.Substring(0, remoteRepositoryName.LastIndexOf(".git"));
            }

            var client = new GitHubClient(new ProductHeaderValue("git-forks"));
            var remoteRepository = client.Repository.Get(remoteUser, remoteRepositoryName).Result;

            while (remoteRepository.Fork)
                remoteRepository = remoteRepository.Parent;

            Console.WriteLine("Downloading List of Forks...");

            var forks = client.Repository.Forks.GetAll(remoteRepository.Owner.Login, remoteRepository.Name, new RepositoryForksListRequest()).Result;

            var addedAny = false;

            foreach (var fork in forks)
            {
                if (!remotes.Any(x => x.Name == fork.Owner.Login || x.Url == fork.CloneUrl))
                {
                    addedAny = true;
                    ClearCurrentConsoleLine();
                    Console.Write($"Adding Fork {fork.Owner.Login}...");
                    remotes.Add(fork.Owner.Login, fork.CloneUrl);
                }
            }

            if (addedAny)
                Console.WriteLine();

            var i = 1;
            var remoteCount = remotes.Count();
            foreach (var remote in remotes)
            {
                ClearCurrentConsoleLine();
                Console.Write($"Fetching {remote.Name} ({i++}/{remoteCount})...");
                localRepository.Fetch(remote.Name);
            }

            Console.WriteLine();
        }
    }
}
