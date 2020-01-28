# Open Police World
## About
A game created in collaboration between students at ÅIS and NTNU Ålesund.

## Development workflow
1. Using Jira and Confluence for project management, Git with LFS as version control.

### Unity3d
**Unity 2019.2.19f1 as development version**

#### Importing Unity for Git plugin
1. Dowmload the Unity for Git *.unitypackage* somewhere on your computer
1. Add a new empty project in Unity
1. Using **[Assets] -> [Import package..]** then select your package
1. Select **[Window] -> [GitHub]**
1. Then login to github in the right window pane.
1. Under the settings pane on the right, click **Find system Git**, then click **Save....**

#### Importing source code 
1. Install Unity 2019.2.19f1 from Unity Hub
1. Download a git tool to use; for example *gitkraken, sourcetree, or plain git*
1. Clone the repo to a location on your harddisk
1. Open **UnityHub**, Select **Project** in the left menu, then click on the **ADD** 
1. Select the folder where you cloned your git Repo 
1. Check that Git for Unity is working
1. You're now ready
1. Use Git flow for development!

## Git
### Git Flow Settings
`Set this in your git client (like Gitkraken)`
```
How to name your supporting branch prefixes?
Feature branches? [feature/]
Bugfix branches? [bugfix/]
Release branches? [release/]
Hotfix branches? [hotfix/]
Support branches? [support/]
Version tag prefix? []
```

### File locking with git-lfs
[https://confluence.atlassian.com/bitbucketserver/working-with-git-lfs-files-970595880.html#WorkingwithGitLFSFiles-LockingandunlockingGitLFSfilesLockingandunlockingGitLFSfiles](https://confluence.atlassian.com/bitbucketserver/working-with-git-lfs-files-970595880.html#WorkingwithGitLFSFiles-LockingandunlockingGitLFSfilesLockingandunlockingGitLFSfiles)
### How to lock manually
If unity is not picking up changes, you can lock a file with
`git lfs lock sprite.png`

Then the following rules is applied:
 * Each file can only be locked by one person at a time.
 * Locked files can only be unlocked by the person who locked them (see below for how to force unlock files).
 * If your ‘push’ contains locked files that you didn’t lock it will be rejected.
 * If your ‘merge’ contains locked files that you didn’t lock it will be blocked.

 To unlock
 `git lfs unlock sprite.png`
