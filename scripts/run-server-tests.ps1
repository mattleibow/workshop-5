# Run server tests

# Determine project root
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

# Run server tests
Set-Location $ProjectRoot
Write-Host "Running server tests..."

dotnet test server\TailspinToys.Api.Tests\TailspinToys.Api.Tests.csproj --verbosity minimal
