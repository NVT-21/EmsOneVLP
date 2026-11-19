# ============================================================
#  Go bo Windows Service EMS.EomsCategorySyncWorker
#  Cach chay: chuot phai -> Run with PowerShell (tu dong xin quyen Admin)
# ============================================================
param(
    [string]$ServiceName = "EMS.EomsCategorySyncWorker"
)

$ErrorActionPreference = "Stop"

$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "Can quyen Administrator. Dang mo lai..." -ForegroundColor Yellow
    Start-Process powershell.exe -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""
    exit
}

try {
    $svc = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if (-not $svc) {
        Write-Host "Service '$ServiceName' khong ton tai. Khong can go." -ForegroundColor Yellow
    }
    else {
        if ($svc.Status -ne "Stopped") {
            Write-Host "Dang dung service..." -ForegroundColor Cyan
            Stop-Service -Name $ServiceName -Force
            $svc.WaitForStatus("Stopped", [TimeSpan]::FromSeconds(30))
        }
        Write-Host "Dang go service..." -ForegroundColor Cyan
        & sc.exe delete $ServiceName | Out-Null
        Write-Host ""
        Write-Host "===== DA GO SERVICE '$ServiceName' =====" -ForegroundColor Green
        Write-Host "File trong thu muc cai dat khong bi xoa, ban co the tu xoa neu khong dung nua."
    }
}
catch {
    Write-Host ""
    Write-Host "[LOI] $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Read-Host "Nhan Enter de dong"
