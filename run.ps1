# ICC-Re Build & Run Script
# 用法: 右键点击此文件 -> 使用 PowerShell 运行
# 或在终端中执行: .\run.ps1

param(
    [switch]$Release,
    [switch]$Clean,
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop"
$ProjectPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectName = "InkCanvasForClass-Remastered"
$ProjectFile = Join-Path $ProjectPath "$ProjectName\$ProjectName.csproj"

if (-not (Test-Path $ProjectFile)) {
    Write-Host "错误: 找不到项目文件 $ProjectFile" -ForegroundColor Red
    exit 1
}

Set-Location $ProjectPath

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ICC-Re 构建脚本" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($Clean) {
    Write-Host "[1/3] 清理项目..." -ForegroundColor Yellow
    dotnet clean -c Release --verbosity quiet 2>$null
    Remove-Item -Path "$ProjectName\bin" -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -Path "$ProjectName\obj" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "      清理完成" -ForegroundColor Green
}

if (-not $NoBuild) {
    Write-Host "[2/3] 还原依赖..." -ForegroundColor Yellow
    dotnet restore --verbosity quiet
    Write-Host "      还原完成" -ForegroundColor Green

    $Config = if ($Release) { "Release" } else { "Debug" }
    Write-Host "[3/3] 构建项目 ($Config)..." -ForegroundColor Yellow
    $buildOutput = dotnet build -c $Config --no-restore 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Host $buildOutput -ForegroundColor Red
        Write-Host ""
        Write-Host "构建失败!" -ForegroundColor Red
        exit 1
    }

    Write-Host "      构建成功" -ForegroundColor Green
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  启动 ICC-Re..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$ExePath = Join-Path $ProjectPath "$ProjectName\bin\$Config\net10.0-windows7.0\$ProjectName.exe"

if (-not (Test-Path $ExePath)) {
    $ExePath = Join-Path $ProjectPath "$ProjectName\bin\Debug\net10.0-windows7.0\$ProjectName.exe"
}

if (Test-Path $ExePath) {
    Write-Host "正在启动: $ExePath" -ForegroundColor Gray
    Start-Process $ExePath
    Write-Host ""
    Write-Host "ICC-Re 已启动!" -ForegroundColor Green
} else {
    Write-Host "错误: 找不到可执行文件" -ForegroundColor Red
    Write-Host "请先运行构建" -ForegroundColor Yellow
    exit 1
}
