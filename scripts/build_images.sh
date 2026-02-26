#!/bin/bash
set -e

# Get the directory where the script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
# Assuming the script is in scripts/, the project root is one level up
PROJECT_ROOT="$SCRIPT_DIR/.."

# Script to build and push SpendWise Docker images
REGISTRY="ravinbanda"
REPO="spendwise"
declare -a services=("AuthService" "ExpenseService" "CategoryService" "BudgetService" "SavingsGoalService" "AnalyticsService")
declare -a tags=("auth-service" "expense-service" "category-service" "budget-service" "savings-goal-service" "analytics-service")

echo "Building and Pushing Docker Images..."

for i in "${!services[@]}"; do
  SERVICE_DIR=${services[$i]}
  TAG=${tags[$i]}
  IMAGE_NAME="$REGISTRY/$REPO:$TAG-latest"
  
  echo "--------------------------------------------------"
  echo "Building $SERVICE_DIR -> $IMAGE_NAME"
  echo "--------------------------------------------------"
  
  # Navigate to backend directory or adjust path
  docker build -t $IMAGE_NAME -f "$PROJECT_ROOT/apps/backend/$SERVICE_DIR/Dockerfile" "$PROJECT_ROOT/apps/backend/$SERVICE_DIR"
  
  echo "Pushing $IMAGE_NAME"
  docker push $IMAGE_NAME
done

echo "All images built and pushed successfully!"
