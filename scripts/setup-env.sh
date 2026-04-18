#!/bin/bash

# Setup environment: .NET restore

# Determine project root
if [[ $(basename $(pwd)) == "scripts" ]]; then
    PROJECT_ROOT=$(pwd)/..
else
    PROJECT_ROOT=$(pwd)
fi

cd "$PROJECT_ROOT" || exit 1

echo "Restoring .NET dependencies..."
dotnet restore

echo "Building Playwright test project and installing browsers..."
dotnet build client/TailspinToys.E2E --verbosity quiet

PLAYWRIGHT_SCRIPT="client/TailspinToys.E2E/bin/Debug/net10.0/playwright.sh"
if [ -f "$PLAYWRIGHT_SCRIPT" ]; then
    bash "$PLAYWRIGHT_SCRIPT" install
fi

echo "Environment setup complete."
