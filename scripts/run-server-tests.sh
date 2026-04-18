#!/bin/bash

# Determine project root
if [[ $(basename $(pwd)) == "scripts" || $(basename $(pwd)) == "server" ]]; then
    PROJECT_ROOT=$(pwd)/..
else
    PROJECT_ROOT=$(pwd)
fi

# Run server tests
cd "$PROJECT_ROOT" || exit 1
echo "Running server tests..."

dotnet test server/TailspinToys.Api.Tests/TailspinToys.Api.Tests.csproj --verbosity minimal
