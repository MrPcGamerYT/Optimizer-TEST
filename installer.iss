[Setup]
AppName=Optimizer-Test
AppVersion=1.0.0
DefaultDirName={autopf}\Optimizer-Test
DefaultGroupName=Optimizer-Test
OutputDir=./setup_output
OutputBaseFilename=Optimizer-Installer
Compression=lzma
SolidCompression=yes

[Files]
; This includes your main EXE and the Guna DLL
Source: "publish\Optimizer-Test.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Guna.UI2.dll"; DestDir: "{app}"; Flags: ignoreversion
; Add any other DLLs here if needed

[Icons]
Name: "{group}\Optimizer-Test"; Filename: "{app}\Optimizer-Test.exe"
Name: "{commondesktop}\Optimizer-Test"; Filename: "{app}\Optimizer-Test.exe"
