## Description
reSP is a free plugin for [JetBrain ReSharper](https://plugins.jetbrains.com/plugin/11684-resp) tool to assist SharePoint developer.

Using ReSharper you have an online static validation of code, markup, XML and Javascript. Once installed the reSP plugin pay your attention to the problems in SharePoint API (SSOM, JSOM) usage, potential performance issues, invalid ASP and XML and provide productivity tools like IntelliSense and live templates. 

Code inspection is performed automatically in design time for all opened files. Also, it can be applied to a whole solution or project. 

![inspect image](/assets/inspect.png 'you can inspect both the current file and the entire project or solution')


reSP plugin comes as a single bundle with two different editions:

* Basic - includes validation rules.
* Professional - includes productivity features like code completion, live templates etc.

## Supported platforms
* JetBrains ReSharper 8.2 or high 
* Visual Studio 2010+
* SharePoint project types:
   * Farm solution
   * Sandboxed
   * Console application

## How to install
Use ReSharper Extension Manager to install reSP plugin from repository. Just try to find by SHAREPOINT tag.

![getting started image](/assets/getting-started.gif 'how to install reSP')

## To debug plugin 
1. Be sure you install ReSharper for experimental instance (custom hives).
To install to an experimental instance, run the ReSharper installer, select the Options button, and enter the name of the instance as reSP. Note: this instance of VS must not have reSP installed!
2. Check command line arguments: /ReSharper.Internal /rootsuffix reSP3. 
3. In the *.nuspec file change string in path Release -> Debug: bin\Release|Debug\net48\.
4. Run 'VS custom hive reSP': compile and Build plugin project in the Debug mode.
5. Execute BuildPackage.cmd and get .nupkg locally.
6. Remove existing reSP package from Visual Studio and then install it again from the local path (check before that plugin's local path is a Source in the R# plugin settings).
7. Run regular VS but do not open any solution, just pending on the welcome window.
8. Run 'VS custom hive reSP' and attach to the devenv.exe process. Set a breakpoint if reguired.
9. In the regular VS open target SharePoint solution.

## To update package:
1. Modify assembly version in the project properties.
2. Edit Deploy\ReSharePoint.nuspec file. Change version in the <version> tag.
3. Build Release in VS. 

## Documentation
Please check out the full documentation [here](/Documentation/index.md).