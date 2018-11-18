@echo off
cd C:\ProgramData\Rocrail\tools
.\CommandCam.exe /quiet /devnum 2 /filename C:\ProgramData\Rocrail\tools\webcam1.bmp /delay 10
.\Bmp2Png.exe webcam1.bmp
