# Start the Tailspin Toys application (ASP.NET Core API + Blazor client)

# Store initial directory and script directory
$InitialDir = Get-Location
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

# Navigate to project root
Set-Location $ProjectRoot

Write-Host "Restoring .NET dependencies..."
& "$ScriptDir\setup-env.ps1"

# Check if database needs seeding (empty or missing)
$DbPath = "$ProjectRoot\data\tailspin-toys.db"
if (-not (Test-Path $DbPath)) {
    Write-Host "Database is missing. It will be seeded on first API start."
}

Write-Host "Starting API (ASP.NET Core) server..."

# Start API server
$ServerJob = Start-Job -ScriptBlock {
    param($ProjectRoot)
    Set-Location "$ProjectRoot\server\TailspinToys.Api"
    dotnet run --no-restore
} -ArgumentList $ProjectRoot

Write-Host "Starting client (Blazor)..."
$ClientJob = Start-Job -ScriptBlock {
    param($ProjectRoot)
    Set-Location "$ProjectRoot\client\TailspinToys.Web"
    dotnet run --no-restore
} -ArgumentList $ProjectRoot

# Sleep for 5 seconds
Start-Sleep -Seconds 5

# Display the server URLs
Write-Host ""
Write-Host "Server (ASP.NET Core API) running at: http://localhost:5100" -ForegroundColor Green
Write-Host "Client (Blazor) server running at: http://localhost:4321" -ForegroundColor Green
Write-Host ""
Write-Host "Press Ctrl+C to stop the servers"

# Function to handle cleanup
function Stop-Servers {
    Write-Host "Shutting down servers..."
    Stop-Job $ServerJob -ErrorAction SilentlyContinue
    Stop-Job $ClientJob -ErrorAction SilentlyContinue
    Remove-Job $ServerJob -ErrorAction SilentlyContinue
    Remove-Job $ClientJob -ErrorAction SilentlyContinue
    Set-Location $InitialDir
}

# Register cleanup on script termination
try {
    while ($true) {
        Receive-Job $ServerJob -ErrorAction SilentlyContinue
        Receive-Job $ClientJob -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 1
    }
} finally {
    Stop-Servers
}
