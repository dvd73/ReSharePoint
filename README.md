## Description
reSP is a free plugin for [JetBrain ReSharper](https://plugins.jetbrains.com/plugin/11684-resp) tool to assist SharePoint developer.

Using ReSharper you have an online static validation of code, markup, XML and Javascript. Once installed the reSP plugin pay your attention to the problems in SharePoint API (SSOM, JSOM) usage, potential performance issues, invalid ASP and XML and provide productivity tools like IntelliSense and live templates. 

Code inspection is performed automatically in design time for all opened files. Also, it can be applied to a whole solution or project. 

![inspect image](/assets/inspect.png 'you can inspect both the current file and the entire project or solution')

Depends on issue severity the problem text is highlighted in different ways and corresponded color marks are added to the marker bar. Mostly each of highlightings has a quick fix action (look like a bulb). And you can apply this fix on the current file, current folder or project or whole solution.

![severity image](/assets/severity.png 'you can change the severity as well')

reSP plugin comes as a single bundle with two different editions:

* Basic - includes validation rules.
* Professional - includes productivity features like code completion, live templates etc.

You can enable/disable reSP features in the ReSharper Product & Features dialog.

![options image](/assets/options.png 'you can specify file for ignoring')

## Supported platforms
* JetBrains ReSharper 9.0 or high 
* Visual Studio 2010/2012/2013/2015/2017/2019/2022
* SharePoint project types:
   * Farm solution
   * Sandboxed
   * Console application

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

## To link rule with help page 
1. You need to ensure that [CheckId - help page url] pair exists in the project resources.

![help image](/assets/get_help.png 'how to get help for incident')

## To update package:
1. Modify assembly version in the project properties.
2. Edit Deploy\ReSharePoint.nuspec file. Change version in the <version> tag.
3. Build Release in VS. 
