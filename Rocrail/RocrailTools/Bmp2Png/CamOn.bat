@echo off
:start
cd C:\ProgramData\Rocrail\tools
if exist C:\ProgramData\Rocrail\tools\camoff goto stopp
if exist C:\ProgramData\Rocrail\tools\webcam1.png goto start
.\CommandCam.exe /devnum 2 /filename C:\ProgramData\Rocrail\tools\webcam1.bmp
.\Bmp2Png.exe webcam1.bmp
TIMEOUT 1
goto start
:stopp
del C:\ProgramData\Rocrail\tools\camoff
