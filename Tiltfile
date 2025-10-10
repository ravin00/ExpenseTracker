docker_build('auth-service', 'Backend/AuthService', dockerfile='Dockerfile')
docker_build('expense-service', 'Backend/ExpenseService', dockerfile='Dockerfile')
docker_build('category-service', 'Backend/CategoryService', dockerfile='Dockerfile')
docker_build('budget-service', 'Backend/BudgetService', dockerfile='Dockerfile')
docker_build('savings-goal-service', 'Backend/SavingsGoalService', dockerfile='Dockerfile')
docker_build('analytics-service', 'Backend/AnalyticsService', dockerfile='Dockerfile')

k8s_yaml('Infrastructure/Kubernetes/namespace.yaml')
k8s_yaml('Infrastructure/Kubernetes/config-secrets.yaml')
k8s_yaml('Infrastructure/Kubernetes/sqlserver.yaml')
k8s_yaml('Infrastructure/Kubernetes/services.yaml')
k8s_yaml('Infrastructure/Kubernetes/monitoring.yaml')

allow_k8s_contexts('kind-*', 'minikube', 'docker-desktop', 'rancher-desktop')

set_team('ExpenseTracker')

kubectl_resource('grafana', port_forwards=[port_forward(3000, 3000)])
kubectl_resource('prometheus', port_forwards=[port_forward(9090, 9090)])