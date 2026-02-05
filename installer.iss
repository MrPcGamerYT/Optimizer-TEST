[Setup]
AppName=Optimizer-Test
AppVersion=1.0.0
DefaultDirName={autopf}\Optimizer-Test
DefaultGroupName=Optimizer-Test
UninstallDisplayIcon={app}\Optimizer-Test.exe
Compression=lzma
SolidCompression=yes
OutputDir=setup_output
OutputBaseFilename=Optimizer-Setup
; --- ICON SETTINGS ---
; If you have an icon file, uncomment the line below and point to it
; SetupIconFile=icon.ico 

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; The * captures the EXE, the Guna DLL, and everything else in the publish folder
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Start Menu Shortcut
Name: "{group}\Optimizer-Test"; Filename: "{app}\Optimizer-Test.exe"
; Desktop Shortcut
Name: "{autodesktop}\Optimizer-Test"; Filename: "{app}\Optimizer-Test.exe"; Tasks: desktopicon

[Run]
; Option to launch the app after installation
Filename: "{app}\Optimizer-Test.exe"; Description: "{cm:LaunchProgram,Optimizer-Test}"; Flags: nowait postinstall skipifsilent
