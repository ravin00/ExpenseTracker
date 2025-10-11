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

### Test API Endpoints:
```bash
# Test auth service
curl -k -H "Host: api.expensetracker.yourdomain.com" https://localhost/auth/health

# Test all services
for service in auth expenses categories budgets savings analytics; do
  echo "Testing $service:"
  curl -k -H "Host: api.expensetracker.yourdomain.com" https://localhost/$service/health
  echo
done
```

### Test Monitoring:
```bash
# Test Grafana
curl -k -H "Host: monitoring.expensetracker.yourdomain.com" https://localhost/grafana/

# Test Prometheus  
curl -k -H "Host: monitoring.expensetracker.yourdomain.com" https://localhost/prometheus/
```

## 🌐 Browser Access

Add these entries to your `/etc/hosts` file for browser access:
```
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