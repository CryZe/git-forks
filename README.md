# The `git forks` Command

This project aims to make it easy for you to sync your local repository with all the forks of the repository available on GitHub. Especially when trying to test out Pull Requests, you want to check out the Branch you are supposed to pull. GitHub suggests you to clone the individual Branch, but that's usually a lot of effort since you have to specify the whole Clone URL.

By using the `git forks` command provided by this project, you can easily check out every Branch known to GitHub of the Repository you are working on. It automatically searches for a Remote in your Local Repository that is located on GitHub and queries all the known Forks through the GitHub API. It then adds Remotes for all these Forks, unless they are already in your Local Repository by either name or the Clone URL. Once it added all the missing Remotes, it automatically fetches all of them, so that your Local Repsitory is synchronized with all the Forks.

## Testing and Merging a Pull Request

Let's say there's a Pull Request you want to test and merge afterwards. You then simply use the `git forks` command to synchronize your Local Repository with all the Forks known to GitHub. Once you've synchronized your Repository, you can check out the Branch of the Pull Request like this:
```
git checkout GitHubUser/branch-of-the-pull-request
```
with `GitHubUser` being the name of the User doing the Pull Request and `branch-of-the-pull-request` being the Branch he's requesting you to pull. Once you checked out the code, you can begin testing it. If you decide to merge it, you can either do it through the Pull Request page or by checking out the Branch he wants you to merge the Pull Request into:
```
git checkout your-branch
```
with `your-branch` being the Branch you want to merge the changes into. Afterwards you merge the Branch of the Pull Request like this:
```
git merge --no-ff GitHubUser/branch-of-the-pull-request
```
You can then proceed to push those changes back to GitHub:
```
git push origin your-branch
```

## Installation

Simply download a compiled version of the `git forks` command or compile it yourself. Once you compiled it, you can put it wherever you want, but you need to make sure it's in one of the folders in your `PATH` environment variable.  We suggest simply putting the compiled files in your `bin` folder of your git installation. Make sure to copy all the files, otherwise some dependencies might be missing.
