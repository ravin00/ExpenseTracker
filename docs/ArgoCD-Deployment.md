# ArgoCD Deployment Guide for ExpenseTracker

## Prerequisites

1. **Kubernetes cluster** with ArgoCD installed
2. **Container registry** (Docker Hub, GCR, ECR, etc.)
3. **Git repository** (your current repo: https://github.com/ravin00/ExpenseTracker)

## Installation Steps

### Step 1: Install ArgoCD (if not already installed)

```bash
# Create ArgoCD namespace
kubectl create namespace argocd

# Install ArgoCD
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

# Wait for ArgoCD to be ready
kubectl wait --for=condition=available --timeout=300s deployment/argocd-server -n argocd
```

### Step 2: Access ArgoCD UI

```bash
# Port forward to ArgoCD server
kubectl port-forward svc/argocd-server -n argocd 8080:443

# Get initial admin password
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
```

Access ArgoCD at: https://localhost:8080
- Username: `admin`
- Password: (from the command above)

### Step 3: Build and Push Container Images

```bash
# Option 1: Using the provided script
./scripts/build-and-push.sh your-registry.com/expensetracker latest

# Option 2: Using Skaffold with ArgoCD profile
export DEFAULT_REPO=your-registry.com/expensetracker
skaffold build --profile=argocd

# Option 3: Manual build (example for one service)
docker build -t your-registry.com/expensetracker/auth-service:latest Backend/AuthService
docker push your-registry.com/expensetracker/auth-service:latest
```

### Step 4: Update Kubernetes Manifests

Update your service manifests to use the pushed images:

```yaml
# In Infrastructure/Kubernetes/services.yaml
containers:
- name: auth-service
  image: your-registry.com/expensetracker/auth-service:latest
```

### Step 5: Deploy Application to ArgoCD

#### Option A: Deploy Single Application

```bash
# Apply the ArgoCD Application
kubectl apply -f Infrastructure/Kubernetes/argoCD.yaml
```

#### Option B: Deploy ApplicationSet (Multiple Environments)

```bash
# Apply the ArgoCD ApplicationSet
kubectl apply -f Infrastructure/Kubernetes/applicationSet.yaml
```

### Step 6: Monitor Deployment

1. **In ArgoCD UI:**
   - Go to Applications
   - Click on `expensetracker` (or environment-specific apps)
   - Monitor sync status and health

2. **Via kubectl:**
   ```bash
   # Check application status
   kubectl get applications -n argocd
   
   # Check pods in target namespace
   kubectl get pods -n expensetracker
   
   # Check services
   kubectl get svc -n expensetracker
   ```

## Configuration Details

### Environment Configuration

The ApplicationSet creates three environments:

1. **Development** (`expensetracker-dev`):
   - 1 replica per service
   - Lower resource limits
   - Suitable for testing

2. **Staging** (`expensetracker-staging`):
   - 2 replicas per service
   - Medium resource limits
   - Pre-production testing

3. **Production** (`expensetracker-prod`):
   - 3 replicas per service
   - Higher resource limits
   - Production workloads

### Database Configuration

- **PostgreSQL 16 Alpine** with persistent storage
- **5Gi storage** by default
- **Connection secrets** managed via Kubernetes secrets
- **Health checks** configured for reliability

### Monitoring

- **Prometheus** for metrics collection
- **Grafana** for visualization
- **Port forwarding** configured in Skaffold

## Accessing Services

After deployment, access services via port-forwarding:

```bash
# Access Grafana
kubectl port-forward svc/grafana -n expensetracker 3000:3000

# Access Prometheus
kubectl port-forward svc/prometheus -n expensetracker 9090:9090

# Access API Gateway (if configured)
kubectl port-forward svc/gateway -n expensetracker 8080:80
```

## Troubleshooting

### Common Issues

1. **Image Pull Errors:**
   ```bash
   # Check if images exist in registry
   docker pull your-registry.com/expensetracker/auth-service:latest
   
   # Check image pull secrets
   kubectl get secrets -n expensetracker
   ```

2. **Database Connection Issues:**
   ```bash
   # Check PostgreSQL pod
   kubectl logs -f deployment/postgres -n expensetracker
   
   # Check database secrets
   kubectl get secret app-secrets -n expensetracker -o yaml
   ```

3. **ArgoCD Sync Issues:**
   ```bash
   # Check application events
   kubectl describe application expensetracker -n argocd
   
   # Force refresh
   argocd app sync expensetracker --hard-refresh
   ```

### Useful Commands

```bash
# Get all resources in namespace
kubectl get all -n expensetracker

# Check application logs
kubectl logs -f deployment/auth-service -n expensetracker

# Scale deployment
kubectl scale deployment auth-service --replicas=2 -n expensetracker

# Delete and recreate application
kubectl delete -f Infrastructure/Kubernetes/argoCD.yaml
kubectl apply -f Infrastructure/Kubernetes/argoCD.yaml
```

## Security Notes

1. **Change default passwords** in production
2. **Use proper RBAC** for ArgoCD
3. **Secure container registry** access
4. **Use TLS** for all communications
5. **Rotate secrets** regularly

## Next Steps

1. Set up **ingress controllers** for external access
2. Configure **SSL/TLS certificates**
3. Implement **backup strategies** for PostgreSQL
4. Set up **alerting** and monitoring
5. Configure **horizontal pod autoscaling**