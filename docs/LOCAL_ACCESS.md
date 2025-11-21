# ExpenseTracker - Local Access Guide

## 🎉 Ingress Controller Successfully Installed!

Both ingress resources are now working properly with the NGINX Ingress Controller.

## 📋 Service Access URLs

### API Services (via Ingress)
- **Base URL**: `https://localhost` (with Host header)
- **Auth Service**: `https://localhost/auth/health`
- **Expense Service**: `https://localhost/expenses/health`  
- **Category Service**: `https://localhost/categories/health`
- **Budget Service**: `https://localhost/budgets/health`
- **Savings Service**: `https://localhost/savings/health`
- **Analytics Service**: `https://localhost/analytics/health`

### Monitoring Services
- **Grafana**: `https://localhost/grafana/` (with Host: monitoring.expensetracker.yourdomain.com)
- **Prometheus**: `https://localhost/prometheus/` (with Host: monitoring.expensetracker.yourdomain.com)

## 🔧 Testing Commands

### Test API Endpoints

```bash
# Test health endpoints
for service in auth expenses categories budgets savings analytics; do
  echo "Testing $service:"
  curl -k -H "Host: api.expensetracker.yourdomain.com" https://localhost/$service/health
  echo
done

# Test Swagger API endpoints
for service in auth expenses categories budgets savings analytics; do
  echo "Swagger for $service: https://localhost/$service/swagger/index.html"
done

# Test user registration (example)
curl -k -H "Host: api.expensetracker.yourdomain.com" \
     -H "Content-Type: application/json" \
     -X POST https://localhost/auth/api/Auth/register \
     -d '{"username":"testuser","email":"test@example.com","password":"Test123!"}'

# Test user login (example)  
curl -k -H "Host: api.expensetracker.yourdomain.com" \
     -H "Content-Type: application/json" \
     -X POST https://localhost/auth/api/Auth/login \
     -d '{"username":"testuser","email":"test@example.com","password":"Test123!"}'
```

### Test Monitoring

```bash
# Test Grafana
curl -k -H "Host: monitoring.expensetracker.yourdomain.com" https://localhost/grafana/

# Test Prometheus  
curl -k -H "Host: monitoring.expensetracker.yourdomain.com" https://localhost/prometheus/
```

## 🚀 Easy Access via Port Forwarding (Recommended)

```bash
# Start port forwards (run in background)
# Note: Using port 3001 for Grafana to avoid conflicts with Docker Desktop
kubectl port-forward svc/grafana 3001:3000 -n expensetracker &
kubectl port-forward svc/prometheus 9090:9090 -n expensetracker &

# Access directly without host headers
echo "📊 Grafana: http://localhost:3001/grafana/"
echo "🔍 Prometheus: http://localhost:9090/"
```

### 🔐 Login Credentials

**Grafana:**
- URL: `http://localhost:3001/grafana/`
- Username: `admin`  
- Password: `SuperSecureGrafanaPassword123!`

**Prometheus:**
- URL: `http://localhost:9090/`
- No authentication required

**Note**: Grafana runs on port 3001 locally to avoid conflicts with Docker Desktop's use of port 3000.

## 📊 **ExpenseTracker Dashboards Created**

Your Grafana now includes these pre-configured dashboards:

### **1. ExpenseTracker - Business Dashboard**
- **URL**: http://localhost:3001/grafana/d/0b6b1a64-2362-4354-b5d1-24ca1c1a0221/expensetracker-business-dashboard
- **Focus**: Overall application performance and business metrics
- **Panels**: API request rates, response times, database connections, service health status

### **2. ExpenseTracker - System Health**  
- **URL**: http://localhost:3001/grafana/d/b6da9c8d-160c-4af4-afdc-cf798f35454b/expensetracker-system-health
- **Focus**: System performance and .NET runtime metrics
- **Panels**: Service uptime, garbage collection, thread pool usage, database performance

### **3. ExpenseTracker - API Performance**
- **URL**: Available in dashboard list after creation
- **Focus**: Individual microservice performance
- **Panels**: Request rates per service (Auth, Expense, Category, Budget, Savings, Analytics)

## 🎯 **Key Metrics Available**

**Application Metrics:**
- HTTP request rates by service
- API response times  
- Failed request counts
- Service health status (UP/DOWN)

**Database Metrics:**
- Active PostgreSQL connections
- Entity Framework query performance
- Database command duration
- Connection pool utilization

**.NET Performance:**
- Memory usage per service
- Garbage collection rates (Gen 0, 1, 2)
- Thread pool metrics
- JIT compilation statistics

## 🚀 **Quick Start Guide**

1. **Access Grafana**: http://localhost:3001/grafana/
2. **Login**: admin / SuperSecureGrafanaPassword123!
3. **View Dashboards**: Click "Dashboards" → "Browse" 
4. **Import Custom**: Use `/Users/ravinbandara/Desktop/ExpenseTracker/grafana-dashboard-expensetracker.json`

## 🌐 Browser Access via Ingress

Add these entries to your `/etc/hosts` file for browser access:

```hostfile
127.0.0.1 api.expensetracker.yourdomain.com
127.0.0.1 monitoring.expensetracker.yourdomain.com
```

Then access:
- **API**: https://api.expensetracker.yourdomain.com/auth/health
- **Grafana**: https://monitoring.expensetracker.yourdomain.com/grafana/
- **Prometheus**: https://monitoring.expensetracker.yourdomain.com/prometheus/

## 🔒 Security Notes

- All traffic is redirected to HTTPS (SSL redirect enabled)
- Self-signed certificates are used (browser will show security warning)
- Rate limiting: 100 requests per minute per IP
- CORS enabled for the configured domain

## 🏥 Health Check

All services should return "Healthy" status when accessed via their `/health` endpoints.

## 📊 ArgoCD Status

- **Sync Status**: Synced ✅
- **Health Status**: Healthy ✅
- **Ingress Controller**: NGINX ✅