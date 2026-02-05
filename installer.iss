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

; --- ADMIN RIGHTS ---
PrivilegesRequired=admin

; --- ICON FIX ---
; Point to the icon inside the subfolder
SetupIconFile=Optimizer\app_icon.ico 

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "publish\Optimizer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Guna.UI2.dll"; DestDir: "{app}"; Flags: ignoreversion

; --- ICON FIX ---
; Tell the installer to grab the icon from the subfolder
Source: "Optimizer\app_icon.ico"; DestDir: "{app}"; Flags: ignoreversion

Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Shortcuts point to the icon we just moved into the app folder
Name: "{group}\Optimizer"; Filename: "{app}\Optimizer.exe"; IconFilename: "{app}\app_icon.ico"
Name: "{autodesktop}\Optimizer"; Filename: "{app}\Optimizer.exe"; Tasks: desktopicon; IconFilename: "{app}\app_icon.ico"

[Registry]
Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers"; \
    ValueType: String; ValueName: "{app}\Optimizer.exe"; ValueData: "~ RUNASADMIN"; \
    Flags: uninsdeletevalue

[Run]
Filename: "{app}\Optimizer.exe"; Description: "{cm:LaunchProgram,Optimizer}"; Flags: nowait postinstall skipifsilent runascurrentuser
