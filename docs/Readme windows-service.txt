
Var kompilerad exe-fil finns
cd \dev\plandokument\windows-service\src\obj\release


Installation via 
 * Developer Command Prompt eller
 * C:\Windows\Microsoft.NET\Framework64\v4.0.30319

Installera
 ==> installutil.exe Plan.WindowsService.exe

Avinstallera
 ==> installutil.exe /u Plan.WindowsService.exe




Installation via
 * Microsoft Visual Studio Installer Projects

VSIX
 ==> https://marketplace.visualstudio.com/items?itemName=visualstudioclient.MicrosoftVisualStudio2017InstallerProjects#overview

Dokumentation
 ==> https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/2kt85ked(v%3dvs.100)




FileWatcher
 ==> https://www.codeproject.com/Articles/18521/How-to-implement-a-simple-filewatcher-Windows-serv




Installera
 ==> https://stackoverflow.com/questions/9021075/how-to-create-an-installer-for-a-net-windows-service-using-visual-studio/9021107#9021107


Radera event-logg manuellt genom PowerShell
 ==> Remove-EventLog -LogName "ThumnailsLog"

