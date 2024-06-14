## To debug plugin 
1. Be sure you install ReSharper for experimental instance (custom hives)
To install to an experimental instance, run the ReSharper installer, select the Options button, and enter the name of the instance as reSP. 
2. Simply make sure it is selected as the start-up project
3. Check debug command line arguments: /ReSharper.Internal /rootsuffix reSP
4. Ctrl+F5 and simply attach a debugger to an instance of Visual Studio that will run the plugin 

## To link rule with help page 
1. You need to ensure that [CheckId - help page url] pair exists in the project resources.

## To update package:
1. Modify assembly version.
2. Edit Deploy\ReSharePoint.nuspec file. Change version in the <version> tag.
3. Edit Deploy\UpdatePackage.cmd. Set right package file name (ReSharePoint.1.0.0.1.nupkg).
4. Build Release in VS. 
5. Run UpdatePackage.cmd.
