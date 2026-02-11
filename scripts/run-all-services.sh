#!/bin/bash

# Run all ExpenseTracker services concurrently

echo "Cleaning up existing processes..."

# Kill any existing processes on the ports
lsof -ti:5048 | xargs kill -9 2>/dev/null || true
lsof -ti:5003 | xargs kill -9 2>/dev/null || true
lsof -ti:5004 | xargs kill -9 2>/dev/null || true
lsof -ti:5005 | xargs kill -9 2>/dev/null || true
lsof -ti:5006 | xargs kill -9 2>/dev/null || true
lsof -ti:5090 | xargs kill -9 2>/dev/null || true

sleep 2

echo "Starting all ExpenseTracker services..."

# Get the base directory
BASE_DIR="$(cd "$(dirname "$0")" && pwd)"

# Start each service in background with proper directory context
(cd "$BASE_DIR/Backend/AuthService" && dotnet run) &
AUTH_PID=$!

(cd "$BASE_DIR/Backend/ExpenseService" && dotnet run) &
EXPENSE_PID=$!

(cd "$BASE_DIR/Backend/CategoryService" && dotnet run) &
CATEGORY_PID=$!

(cd "$BASE_DIR/Backend/BudgetService" && dotnet run) &
BUDGET_PID=$!

(cd "$BASE_DIR/Backend/SavingsGoalService" && dotnet run) &
SAVINGS_PID=$!

(cd "$BASE_DIR/Backend/AnalyticsService" && dotnet run) &
ANALYTICS_PID=$!

echo "All services started!"
echo ""
echo "Swagger URLs:"
echo "   Auth Service:         http://localhost:5048/swagger"
echo "   Category Service:     http://localhost:5003/swagger"
echo "   Budget Service:       http://localhost:5004/swagger"
echo "   Savings Goal Service: http://localhost:5005/swagger"
echo "   Analytics Service:    http://localhost:5006/swagger"
echo "   Expense Service:      http://localhost:5090/swagger"
echo ""
echo "Press Ctrl+C to stop all services..."

# Wait for all background processes
wait $AUTH_PID $EXPENSE_PID $CATEGORY_PID $BUDGET_PID $SAVINGS_PID $ANALYTICS_PID
