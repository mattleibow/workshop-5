#!/bin/bash

# Define color codes
GREEN='\033[0;32m'
NC='\033[0m' # No Color

# Store initial directory and script directory
INITIAL_DIR=$(pwd)
SCRIPT_DIR=$(dirname "$(realpath "$0")")
PROJECT_ROOT=$(dirname "$SCRIPT_DIR")

# Check if we're in scripts, client, or server directory and navigate up one level
current_directory=$(basename $(pwd))
if [[ "$current_directory" =~ ^(scripts|client|server)$ ]]; then
    cd ..
fi

echo "Restoring .NET dependencies..."
source "${SCRIPT_DIR}/setup-env.sh"

# Check if database needs seeding (empty or missing)
DB_PATH="${PROJECT_ROOT}/data/tailspin-toys.db"
if [[ ! -f "$DB_PATH" ]] || [[ ! -s "$DB_PATH" ]]; then
    echo "Database is empty or missing. It will be seeded on first API start."
fi

echo "Starting API (ASP.NET Core) server..."

# Start API server
cd "${PROJECT_ROOT}/server/TailspinToys.Api" || {
    echo "Error: server directory not found"
    cd "$INITIAL_DIR"
    exit 1
}
dotnet run --no-restore &

# Store the API server process ID
SERVER_PID=$!

echo "Starting client (Blazor)..."
cd "${PROJECT_ROOT}/client/TailspinToys.Web" || {
    echo "Error: client directory not found"
    cd "$INITIAL_DIR"
    exit 1
}
dotnet run --no-restore &

# Store the Blazor server process ID
CLIENT_PID=$!

# Sleep for 5 seconds
sleep 5

# Display the server URLs
echo -e "\n${GREEN}Server (ASP.NET Core API) running at: http://localhost:5100${NC}"
echo -e "${GREEN}Client (Blazor) server running at: http://localhost:4321${NC}\n"

echo "Ctl-C to stop the servers"

# Function to handle script termination
cleanup() {
    echo "Shutting down servers..."
    
    # Send SIGTERM first to allow graceful shutdown
    kill -TERM $SERVER_PID 2>/dev/null
    kill -TERM $CLIENT_PID 2>/dev/null
    
    # Wait briefly for graceful shutdown
    sleep 2
    
    # Then force kill if still running
    if ps -p $SERVER_PID > /dev/null 2>&1; then
        kill -9 $SERVER_PID 2>/dev/null
    fi
    
    if ps -p $CLIENT_PID > /dev/null 2>&1; then
        kill -9 $CLIENT_PID 2>/dev/null
    fi

    # Return to initial directory
    cd "$INITIAL_DIR"
    exit 0
}

# Trap multiple signals
trap cleanup SIGINT SIGTERM SIGQUIT EXIT

# Keep the script running
wait
