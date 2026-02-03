#!/bin/bash

# Build and Push Script for ExpenseTracker
# Usage: ./scripts/build-and-push.sh [REGISTRY] [TAG]

set -e

# Default values
REGISTRY=${1:-"docker.io/your-username"}
TAG=${2:-"latest"}
GIT_COMMIT=$(git rev-parse --short HEAD)

echo "Building and pushing ExpenseTracker images..."
echo "Registry: $REGISTRY"
echo "Tag: $TAG"
echo "Git Commit: $GIT_COMMIT"

# Services to build
SERVICES=("auth-service" "expense-service" "category-service" "budget-service" "savings-goal-service" "analytics-service")

# Build and push each service
for service in "${SERVICES[@]}"; do
    echo "Building $service..."
    
    # Convert service name to directory name
    case $service in
        "auth-service") dir="AuthService" ;;
        "expense-service") dir="ExpenseService" ;;
        "category-service") dir="CategoryService" ;;
        "budget-service") dir="BudgetService" ;;
        "savings-goal-service") dir="SavingsGoalService" ;;
        "analytics-service") dir="AnalyticsService" ;;
    esac
    
    # Build the image
    docker build -t "$REGISTRY/$service:$TAG" -t "$REGISTRY/$service:$GIT_COMMIT" "Backend/$dir"
    
    echo "Pushing $service..."
    docker push "$REGISTRY/$service:$TAG"
    docker push "$REGISTRY/$service:$GIT_COMMIT"
    
    echo "$service pushed successfully!"
done

echo "All images built and pushed successfully!"
echo ""
echo "To deploy with ArgoCD, update your Kubernetes manifests to use:"
echo "   Image: $REGISTRY/[service-name]:$TAG"
echo ""
echo "ArgoCD will automatically sync from your Git repository."