@echo off
REM ============================================================
REM  Publish EMS.EomsCategorySyncWorker (Self-contained, win-x64)
REM  Ket qua: ..\deploy_package  + ..\EMS.EomsCategorySyncWorker.zip
REM  Server KHONG can cai .NET 9.
REM ============================================================
setlocal
cd /d "%~dp0"

set "OUT=%~dp0..\deploy_package"
set "ZIP=%~dp0..\EMS.EomsCategorySyncWorker.zip"
set "SVC=EMS.EomsCategorySyncWorker"

echo [0/3] Kiem tra va tam dung service %SVC% (neu dang chay)...
sc query "%SVC%" | find "RUNNING" >nul 2>&1
if not errorlevel 1 (
    echo Service dang chay. Dang tam dung de giai phong file .exe...
    net stop "%SVC%" >nul 2>&1
    timeout /t 2 /nobreak >nul
)

echo [1/3] Don dep thu muc cu...
if exist "%OUT%" rmdir /s /q "%OUT%"
if exist "%ZIP%" del /q "%ZIP%"

echo [2/3] dotnet publish (Release, win-x64, self-contained)...
dotnet publish "EMS.EomsCategorySyncWorker.csproj" -c Release -r win-x64 --self-contained true -o "%OUT%"
if errorlevel 1 (
    echo.
    echo [LOI] Publish that bai. Neu báo loi file locked, hay Stop service %SVC% bang quyen Administrator.
    pause
    exit /b 1
)

echo [3/3] Nen file zip...
powershell -NoProfile -Command "Compress-Archive -Path '%OUT%\*' -DestinationPath '%ZIP%' -Force"
if errorlevel 1 (
    echo [CANH BAO] Nen zip that bai, hay tu nen thu muc deploy_package.
)

echo.
echo ===== HOAN TAT =====
echo Thu muc : %OUT%
echo File zip: %ZIP%
echo Copy file zip len server, giai nen, sua appsettings.json, roi chay install-service.ps1 (Run as Administrator).
echo.
pause
endlocal
