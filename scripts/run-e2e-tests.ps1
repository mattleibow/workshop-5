# Run end-to-end tests: starts servers, runs tests, then stops servers

# Determine project root
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

Set-Location $ProjectRoot

Write-Host "Starting Tailspin Toys E2E Tests" -ForegroundColor Blue

# Check if servers are already running
$ApiRunning = $false
$WebRunning = $false
try { $null = Invoke-WebRequest -Uri "http://localhost:5100/api/games" -UseBasicParsing -TimeoutSec 2; $ApiRunning = $true } catch {}
try { $null = Invoke-WebRequest -Uri "http://localhost:4321" -UseBasicParsing -TimeoutSec 2; $WebRunning = $true } catch {}

# Start servers if not already running
$ApiJob = $null
$WebJob = $null

if (-not $ApiRunning) {
    Write-Host "Starting API server..." -ForegroundColor Green
    $ApiJob = Start-Job -ScriptBlock {
        param($Root)
        Set-Location "$Root/server/TailspinToys.Api"
        dotnet run
    } -ArgumentList $ProjectRoot
}

if (-not $WebRunning) {
    Write-Host "Starting Blazor server..." -ForegroundColor Green
    $WebJob = Start-Job -ScriptBlock {
        param($Root)
        Set-Location "$Root/client/TailspinToys.Web"
        dotnet run
    } -ArgumentList $ProjectRoot
}

# Wait for servers to be ready
Write-Host "Waiting for servers to be ready..." -ForegroundColor Green
$Timeout = 120
$Elapsed = 0
while ($Elapsed -lt $Timeout) {
    $ApiOk = $false
    $WebOk = $false
    try { $null = Invoke-WebRequest -Uri "http://localhost:5100/api/games" -UseBasicParsing -TimeoutSec 2; $ApiOk = $true } catch {}
    try { $null = Invoke-WebRequest -Uri "http://localhost:4321" -UseBasicParsing -TimeoutSec 2; $WebOk = $true } catch {}

    if ($ApiOk -and $WebOk) { break }
    Start-Sleep -Seconds 2
    $Elapsed += 2
}

if ($Elapsed -ge $Timeout) {
    Write-Host "Timeout waiting for servers to start" -ForegroundColor Red
    if ($ApiJob) { Stop-Job $ApiJob; Remove-Job $ApiJob }
    if ($WebJob) { Stop-Job $WebJob; Remove-Job $WebJob }
    exit 1
}

Write-Host "  * ASP.NET Core API server: http://localhost:5100"
Write-Host "  * Blazor client server: http://localhost:4321"
Write-Host ""
Write-Host "Running tests:" -ForegroundColor Blue

# Run Playwright E2E tests
dotnet test client\TailspinToys.E2E\ --verbosity minimal

# Store and return the exit code
$TestExitCode = $LASTEXITCODE

# Stop servers if we started them
if ($ApiJob) { Stop-Job $ApiJob -ErrorAction SilentlyContinue; Remove-Job $ApiJob -ErrorAction SilentlyContinue }
if ($WebJob) { Stop-Job $WebJob -ErrorAction SilentlyContinue; Remove-Job $WebJob -ErrorAction SilentlyContinue }

Write-Host ""
Write-Host "E2E tests completed with exit code: $TestExitCode" -ForegroundColor Blue
exit $TestExitCode
