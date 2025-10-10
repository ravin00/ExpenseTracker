# Infrastructure: Docker, Kubernetes, Prometheus, Grafana

This folder contains everything to run the backend locally with Docker Compose or on Kubernetes with Skaffold/Tilt. It also includes Prometheus + Grafana for metrics.

## What’s included

- Docker Compose stack with SQL Server, all microservices, Prometheus, Grafana
- Kubernetes manifests (namespace, configs/secrets, SQL Server, services, Prometheus, Grafana)
- Skaffold and Tilt for fast local dev on Kubernetes
- Probes, resource requests/limits, and Prometheus scraping annotations

## Prereqs

- macOS with Docker Desktop (or Colima) and kubectl
- A local Kubernetes cluster (Docker Desktop, kind, or minikube)
- Skaffold (optional) and Tilt (optional)

## Option A: Run with Docker Compose

1) Copy env template if present and set secrets
   - If there's an `.env.template`, copy to `.env` and fill values (DB passwords, JWT secret).
2) Start the stack
   - From repo root: use the provided `manage.sh` if available, otherwise run Docker Compose.
3) Access
   - Services: `http://localhost:<mapped-port>` (see docker-compose for ports)
   - Prometheus: `http://localhost:9090`
   - Grafana: `http://localhost:3000` (default admin/admin)

Metrics endpoint for each service: `/metrics`. Health endpoint: `/health`.

## Option B: Run on Kubernetes with Skaffold

1) Ensure your kube context points to a local cluster
2) Apply base infra (namespace/configs/sqlserver/monitoring/services) or let Skaffold apply all manifests
3) Run:
   - `skaffold dev` for iterative dev (rebuilds on changes, port-forwards Prometheus and Grafana)
   - `skaffold run` for a one-off deploy
4) Access
   - Prometheus: `http://localhost:9090`
   - Grafana: `http://localhost:3000`

## Option C: Run on Kubernetes with Tilt

1) Ensure your kube context points to a local cluster
2) From repo root, run `tilt up`
3) Use the Tilt UI to see logs and port-forwards

## Notes and conventions

- All services listen on port 80 in containers (`ASPNETCORE_URLS=http://+:80`); Services map `targetPort: 80`.
- Prometheus scrapes via pod annotations at `/metrics`.
- Liveness/Readiness probes hit `/health` to keep only healthy pods in service.

## Troubleshooting

- Database init
  - If SQL Server isn’t ready, services may restart until the DB is reachable. Give it ~20–40 seconds.
- Pods not ready
  - Check `kubectl get pods -n expensetracker` and `kubectl describe pod/<pod> -n expensetracker`.
- No metrics in Grafana
  - Verify Prometheus targets: Prometheus UI → Status → Targets. Ensure pods are discovered and scraping `/metrics`.
  - Confirm each app logs include Prometheus middleware and `/metrics` mapping.
- Port conflicts on macOS
  - Ensure 3000 (Grafana) and 9090 (Prometheus) are free. Stop other processes or change forwards in `skaffold.yaml`/`Tiltfile`.

## Next steps

- Add Ingress/Gateway API for a single entry point and TLS
- Add a CI workflow to build/push images and deploy to your cluster
- Expand Grafana dashboards (latency, error rates, DB metrics)
