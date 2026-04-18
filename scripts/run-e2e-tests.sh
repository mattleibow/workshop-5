#!/bin/bash

# Run end-to-end tests: starts servers, runs tests, then stops servers

# Define color codes
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Determine project root
if [[ $(basename $(pwd)) == "scripts" ]]; then
    PROJECT_ROOT=$(pwd)/..
else
    PROJECT_ROOT=$(pwd)
fi

cd "$PROJECT_ROOT" || exit 1

echo -e "${BLUE}Starting Tailspin Toys E2E Tests${NC}"

# Build all projects first
echo -e "${GREEN}Building projects...${NC}"
dotnet build --verbosity quiet || exit 1

# Check if servers are already running
API_RUNNING=false
WEB_RUNNING=false
curl -s -o /dev/null -w "" http://localhost:5100/api/games 2>/dev/null && API_RUNNING=true
curl -s -o /dev/null -w "" http://localhost:4321 2>/dev/null && WEB_RUNNING=true

# Start servers if not already running
if [ "$API_RUNNING" = false ]; then
    echo -e "${GREEN}Starting API server...${NC}"
    dotnet run --project server/TailspinToys.Api/ --no-build > /dev/null 2>&1 &
    API_PID=$!
fi

if [ "$WEB_RUNNING" = false ]; then
    echo -e "${GREEN}Starting Blazor server...${NC}"
    dotnet run --project client/TailspinToys.Web/ --no-build > /dev/null 2>&1 &
    WEB_PID=$!
fi

# Wait for servers to be ready
echo -e "${GREEN}Waiting for servers to be ready...${NC}"
TIMEOUT=120
ELAPSED=0
while [ $ELAPSED -lt $TIMEOUT ]; do
    API_OK=false
    WEB_OK=false
    curl -s -o /dev/null http://localhost:5100/api/games 2>/dev/null && API_OK=true
    curl -s -o /dev/null http://localhost:4321 2>/dev/null && WEB_OK=true

    if [ "$API_OK" = true ] && [ "$WEB_OK" = true ]; then
        break
    fi

    sleep 2
    ELAPSED=$((ELAPSED + 2))
done

if [ $ELAPSED -ge $TIMEOUT ]; then
    echo -e "${BLUE}Timeout waiting for servers to start${NC}"
    [ -n "$API_PID" ] && kill $API_PID 2>/dev/null
    [ -n "$WEB_PID" ] && kill $WEB_PID 2>/dev/null
    exit 1
fi

echo -e "  • ASP.NET Core API server: http://localhost:5100"
echo -e "  • Blazor client server: http://localhost:4321"
echo ""
echo -e "${BLUE}Running tests:${NC}"

# Run Playwright E2E tests
dotnet test client/TailspinToys.E2E/ --verbosity minimal

# Store the exit code
TEST_EXIT_CODE=$?

# Stop servers if we started them
[ -n "$API_PID" ] && kill $API_PID 2>/dev/null
[ -n "$WEB_PID" ] && kill $WEB_PID 2>/dev/null

echo ""
echo -e "${BLUE}E2E tests completed with exit code: $TEST_EXIT_CODE${NC}"
exit $TEST_EXIT_CODE
