# ============================================================
#  Cai dat EMS.EomsCategorySyncWorker thanh Windows Service
#  Chay tren SERVER, ngay trong thu muc chua file .exe
#  Cach chay: chuot phai -> Run with PowerShell (tu dong xin quyen Admin)
# ============================================================
param(
    [string]$ServiceName = "EMS.EomsCategorySyncWorker",
    [string]$DisplayName = "EMS EOMS Category Sync Worker",
    [string]$Description = "Dong bo danh muc Tinh/Xa/San pham EOMS vao Oracle (PKG_EOMS_CATEGORY) va ghi log MongoDB luc 00:00 hang ngay."
)

$ErrorActionPreference = "Stop"

# 1. Tu nang quyen Administrator neu chua co
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "Can quyen Administrator. Dang mo lai..." -ForegroundColor Yellow
    Start-Process powershell.exe -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""
    exit
}

try {
    $exePath = Join-Path $PSScriptRoot "EMS.EomsCategorySyncWorker.exe"
    if (-not (Test-Path $exePath)) {
        throw "Khong tim thay $exePath. Hay chay script nay trong thu muc da giai nen cung file .exe."
    }

    Write-Host "Thu muc cai dat : $PSScriptRoot"
    Write-Host "File thuc thi   : $exePath"

    # 2. Go service cu neu da ton tai
    $existing = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if ($existing) {
        Write-Host "Service da ton tai. Dang dung va go ban cu..." -ForegroundColor Yellow
        if ($existing.Status -ne "Stopped") {
            Stop-Service -Name $ServiceName -Force
            $existing.WaitForStatus("Stopped", [TimeSpan]::FromSeconds(30))
        }
        & sc.exe delete $ServiceName | Out-Null
        Start-Sleep -Seconds 3
    }

    # 3. Tao service (auto start)
    Write-Host "Dang tao service..." -ForegroundColor Cyan
    New-Service -Name $ServiceName `
        -BinaryPathName "`"$exePath`"" `
        -DisplayName $DisplayName `
        -Description $Description `
        -StartupType Automatic | Out-Null

    # 4. Tu restart khi crash: sau 60s, 3 lan; reset dem sau 1 ngay
    & sc.exe failure $ServiceName reset= 86400 actions= restart/60000/restart/60000/restart/60000 | Out-Null
    & sc.exe failureflag $ServiceName 1 | Out-Null

    # 5. Khoi dong
    Write-Host "Dang khoi dong service..." -ForegroundColor Cyan
    Start-Service -Name $ServiceName
    Start-Sleep -Seconds 2

    $svc = Get-Service -Name $ServiceName
    Write-Host ""
    Write-Host "===== HOAN TAT =====" -ForegroundColor Green
    Write-Host "Service : $($svc.Name)"
    Write-Host "Status  : $($svc.Status)"
    Write-Host "Xem log : Event Viewer > Windows Logs > Application, hoac Mongo emsoneloguat.EomsCategorySyncLog (Category = SUMMARY)"
}
catch {
    Write-Host ""
    Write-Host "[LOI] $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Read-Host "Nhan Enter de dong"
