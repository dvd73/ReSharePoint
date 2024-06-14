# The script copies the project DLLs to the reSP Installation folder, if any

# There are a few return codes:
# 42 - can't copy one of the *.dll to the target folder

# More details might be found here:
# - PowerShell Script in PostBuild" 
#	http://stackoverflow.com/questions/813003/powershell-script-in-postbuild

param($SolutionDir, $ProjectDir, $ConfigurationName, $TargetDir, $TargePath)

#Write-Output "SolutionDir:[$SolutionDir]"
#Write-Output "ProjectDir:[$ProjectDir]"

$targetPackages = @()
$targetPackages += $TargePath
$targetPackages += [System.IO.Path]::Combine($TargetDir, "ReSharePoint.pdb")
$targetPackages += [System.IO.Path]::Combine($TargetDir, "ReSharePoint.Entities.dll")
$targetPackages += [System.IO.Path]::Combine($TargetDir, "ReSharePoint.Entities.pdb")

function DeployPackagesToFolder($targetFolder, $packages) {

	Write-Output "Perform module copy..."
	Write-Output "	Target folder:[$targetFolder]"

	if([System.IO.Directory]::Exists($targetFolder) -eq $true)
	{
		foreach($package in $packages) {
			if ($package)
			{
				Write-Output "		Source package:[$package]"
				try 
				{
					Copy-Item -path $package -dest $targetFolder –errorvariable $errors
				}
				catch 
				{
					$ErrorMessage = $_.Exception.Message				
					Write-Output " Error Message: [$ErrorMessage]"				
					[Environment]::Exit(-42)
				}
			}
		}
	} 
}

#DeployPackagesToFolder ([System.IO.Path]::Combine($Env:LOCALAPPDATA, "JetBrains\Installations\ReSharperPlatformVs15_edc2360c")) $targetPackages
DeployPackagesToFolder ([System.IO.Path]::Combine($Env:LOCALAPPDATA, "JetBrains\Installations\ReSharperPlatformVs15_edc2360creSP")) $targetPackages
DeployPackagesToFolder ([System.IO.Path]::Combine($Env:LOCALAPPDATA, "JetBrains\Installations\ReSharperPlatformVs15_edc2360creSP_001")) $targetPackages

[Environment]::Exit(0)
