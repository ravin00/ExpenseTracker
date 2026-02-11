# SpendWise Kubernetes Deployment

## Architecture

                    ┌─────────────────────────────────────┐
                    │         Nginx Ingress Controller      │
                    │         api.spendwise.com             │
                    └────┬──────┬──────┬──────┬──────┬─────┘
                         │      │      │      │      │
              ┌──────────┘  ┌───┘  ┌───┘  ┌───┘  ┌───┘
              ▼             ▼      ▼      ▼      ▼
         ┌────────┐  ┌────────┐ ┌──────┐ ┌──────┐ ┌───────────┐
         │  Auth  │  │Expense │ │Budget│ │ Cat. │ │ Analytics │
         │Service │  │Service │ │Svc   │ │ Svc  │ │  Service  │
         │  x2-5  │  │  x2-5  │ │ x2-4 │ │ x2-4 │ │   x2-5   │
         └───┬────┘  └───┬────┘ └──┬───┘ └──┬───┘ └─────┬─────┘
             │            │        │        │            │
             └────────────┴────────┴────────┴────────────┘
                                   │
                         ┌─────────▼─────────┐
                         │   PostgreSQL DB    │
                         │  (Managed / K8s)   │
                         └───────────────────┘

## Quick Start

### Local Development (Minikube)

    minikube start
    helm install spendwise ./k8s/charts/spendwise -f k8s/charts/spendwise/values-dev.yaml

### Staging

    helm install spendwise ./k8s/charts/spendwise -f k8s/charts/spendwise/values-staging.yaml

### Production (with ArgoCD)

    kubectl apply -f k8s/argocd/application.yaml

## Commands

| Action    | Command                                                   |
| --------- | --------------------------------------------------------- |
| Install   | `helm install spendwise ./k8s/charts/spendwise`           |
| Upgrade   | `helm upgrade spendwise ./k8s/charts/spendwise`           |
| Rollback  | `helm rollback spendwise 1`                               |
| Uninstall | `helm uninstall spendwise`                                |
| Dry run   | `helm install spendwise ./k8s/charts/spendwise --dry-run` |
| Template  | `helm template spendwise ./k8s/charts/spendwise`          |

🚀 Deployment Commands

```bash
# Dev (Minikube) - Deploys to 'spendwise-dev' namespace
helm install spendwise ./k8s/charts/spendwise \
  -n spendwise-dev --create-namespace \
  -f ./k8s/charts/spendwise/values-dev.yaml

# Staging - Deploys to 'spendwise-staging' namespace
helm upgrade --install spendwise ./k8s/charts/spendwise \
  -n spendwise-staging --create-namespace \
  -f ./k8s/charts/spendwise/values-staging.yaml

# Production (via ArgoCD — auto-syncs from Git)
kubectl apply -f k8s/argocd/application.yaml
```

## 🕵️‍♂️ Verifying with ArgoCD

1. **Access the UI**

   ```bash
   # Port-forward the ArgoCD server to localhost:8080
   kubectl port-forward svc/argocd-server -n argocd 8080:443
   ```

   Open [https://localhost:8080](https://localhost:8080) in your browser.

2. **Login**
   - **Username**: `admin`
   - **Password**: Run this command to get it:
     ```bash
     kubectl get secret argocd-initial-admin-secret -n argocd -o jsonpath="{.data.password}" | base64 -d; echo
     ```

3. **Check Application**
   - You should see the `spendwise` application tile.
   - Click it to see the visual tree of all your microservices (Auth, Expense, Budget, etc.).
   - Status should be `Healthy` and `Synced`.
