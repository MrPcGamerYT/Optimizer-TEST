[Setup]
AppName=Optimizer
AppVersion=1.0.0
DefaultDirName={autopf}\Optimizer
DefaultGroupName=Optimizer
UninstallDisplayIcon={app}\Optimizer.exe
Compression=lzma
SolidCompression=yes
OutputDir=setup_output
OutputBaseFilename=Optimizer-v1-Setup
; SetupIconFile=app_icon.ico 

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Changed to Optimizer.exe to match your build output
Source: "publish\Optimizer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Guna.UI2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Optimizer"; Filename: "{app}\Optimizer.exe"
Name: "{autodesktop}\Optimizer"; Filename: "{app}\Optimizer.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Optimizer.exe"; Description: "{cm:LaunchProgram,Optimizer}"; Flags: nowait postinstall skipifsilent
