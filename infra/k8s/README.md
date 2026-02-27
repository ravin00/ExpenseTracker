# SpendWise Kubernetes Deployment

This folder contains Helm and ArgoCD configuration for `dev`, `staging`, and `prod` environments.

## Local Development

For local iteration, prefer Docker Compose in the repo root.
Use Kubernetes locally only when validating ingress, probes, or ArgoCD behavior.

## Helm Deploy Commands

```bash
# Dev
helm upgrade --install spendwise ./infra/k8s/charts/spendwise \
  -n spendwise-dev --create-namespace \
  -f ./infra/k8s/charts/spendwise/values.yaml \
  -f ./infra/k8s/charts/spendwise/values-dev.yaml

# Staging
helm upgrade --install spendwise ./infra/k8s/charts/spendwise \
  -n spendwise-staging --create-namespace \
  -f ./infra/k8s/charts/spendwise/values.yaml \
  -f ./infra/k8s/charts/spendwise/values-staging.yaml

# Prod
helm upgrade --install spendwise ./infra/k8s/charts/spendwise \
  -n spendwise-prod --create-namespace \
  -f ./infra/k8s/charts/spendwise/values.yaml \
  -f ./infra/k8s/charts/spendwise/values-prod.yaml
```

## ArgoCD Application Manifests

```bash
# Dev
kubectl apply -f ./infra/k8s/argocd/application-dev.yaml

# Staging
kubectl apply -f ./infra/k8s/argocd/application-staging.yaml

# Prod
kubectl apply -f ./infra/k8s/argocd/application.yaml
```

## Validation

```bash
# Lint chart
helm lint ./infra/k8s/charts/spendwise -f ./infra/k8s/charts/spendwise/values.yaml

# Render manifests
helm template spendwise ./infra/k8s/charts/spendwise \
  -f ./infra/k8s/charts/spendwise/values.yaml \
  -f ./infra/k8s/charts/spendwise/values-dev.yaml
```

## ArgoCD Access

```bash
kubectl port-forward svc/argocd-server -n argocd 8080:80
```

Open [http://localhost:8080](http://localhost:8080).

Get admin password:

```bash
kubectl get secret argocd-initial-admin-secret -n argocd \
  -o jsonpath="{.data.password}" | base64 -d; echo
```
