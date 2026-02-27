# SpendWise Kubernetes Deployment

This folder contains Helm and Argo CD GitOps configuration for `dev`, `staging`, and `prod` environments.

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

## Argo CD (Project + ApplicationSets)

```bash
# Create SpendWise Argo project boundaries (once per Argo instance)
kubectl apply -f ./infra/k8s/argocd/project-spendwise.yaml

# Local or shared dev cluster: apply DEV only
kubectl apply -f ./infra/k8s/argocd/applicationset-spendwise.yaml

# Staging Argo instance/cluster: apply STAGING
kubectl apply -f ./infra/k8s/argocd/applicationset-spendwise-staging.yaml

# Production Argo instance/cluster: apply PROD (manual sync by default)
kubectl apply -f ./infra/k8s/argocd/applicationset-spendwise-prod.yaml
```

Notes:
- Do not apply `staging` and `prod` ApplicationSets on single-node local clusters.
- `dev` and `staging` are automated sync.
- `prod` is manual sync by design.
- `spendwise-secrets` is expected to be externally managed and is ignored in orphan warnings.
- `values-dev.yaml` intentionally overrides security context for local runtime compatibility.

## Validation

```bash
# Lint chart
helm lint ./infra/k8s/charts/spendwise -f ./infra/k8s/charts/spendwise/values.yaml

# Render manifests
helm template spendwise ./infra/k8s/charts/spendwise \
  -f ./infra/k8s/charts/spendwise/values.yaml \
  -f ./infra/k8s/charts/spendwise/values-dev.yaml
```

## Argo CD Access

```bash
kubectl port-forward svc/argocd-server -n argocd 8080:80
```

Open [http://localhost:8080](http://localhost:8080).

If you run local clusters without an ingress controller/load balancer, keep `values-dev.yaml` with:

```yaml
ingress:
  enabled: false
```

Get admin password:

```bash
kubectl get secret argocd-initial-admin-secret -n argocd \
  -o jsonpath="{.data.password}" | base64 -d; echo
```
