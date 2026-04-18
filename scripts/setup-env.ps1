# Setup environment: .NET restore

# Determine project root
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

Set-Location $ProjectRoot

Write-Host "Restoring .NET dependencies..."
dotnet restore

Write-Host "Building Playwright test project and installing browsers..."
dotnet build client/TailspinToys.E2E --verbosity quiet

$PlaywrightScript = Join-Path $ProjectRoot "client/TailspinToys.E2E/bin/Debug/net10.0/playwright.ps1"
if (Test-Path $PlaywrightScript) {
    & $PlaywrightScript install
}

Write-Host "Environment setup complete."
