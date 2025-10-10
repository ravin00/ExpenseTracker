# ExpenseTracker Documentation

Welcome to the ExpenseTracker documentation. This folder contains comprehensive documentation covering all aspects of the application, from development to deployment and operations.

## 📋 Documentation Index

### 🏗️ Infrastructure & Deployment
- **[Infrastructure Improvements](INFRASTRUCTURE_IMPROVEMENTS.md)** - Comprehensive guide to the infrastructure transformation and best practices implementation
- **[Infrastructure README](Infrastructure-README.md)** - Basic infrastructure setup and configuration guide
- **[ArgoCD Deployment](ArgoCD-Deployment.md)** - ArgoCD setup and GitOps deployment guide
- **[ArgoCD Deployment Success](ArgoCD-Deployment-Success.md)** - ArgoCD deployment success documentation

### 🔒 Security
- **[Security Guide](SECURITY.md)** - Security practices, vulnerability reporting, and compliance guidelines

### 🛠️ Development & Quality
- **[General Improvements](IMPROVEMENTS.md)** - Overall project improvements and enhancements
- **[Quality Improvements Status](QUALITY_IMPROVEMENTS_STATUS.md)** - Backend code quality improvements tracking

## 🚀 Quick Start Guides

### For Developers
1. Start with [IMPROVEMENTS.md](IMPROVEMENTS.md) for an overview of project enhancements
2. Review [QUALITY_IMPROVEMENTS_STATUS.md](QUALITY_IMPROVEMENTS_STATUS.md) for backend development standards
3. Check [SECURITY.md](SECURITY.md) for security guidelines

### For DevOps/Infrastructure
1. Begin with [INFRASTRUCTURE_IMPROVEMENTS.md](INFRASTRUCTURE_IMPROVEMENTS.md) for the complete infrastructure overview
2. Follow [ArgoCD-Deployment.md](ArgoCD-Deployment.md) for GitOps setup
3. Reference [Infrastructure-README.md](Infrastructure-README.md) for basic setup

### For Security Teams
1. Review [SECURITY.md](SECURITY.md) for vulnerability reporting and security practices
2. Check [INFRASTRUCTURE_IMPROVEMENTS.md](INFRASTRUCTURE_IMPROVEMENTS.md) for security implementations

## 📊 Project Architecture

```
ExpenseTracker/
├── Backend/                    # .NET microservices
│   ├── AuthService/           # Authentication & authorization
│   ├── ExpenseService/        # Expense management
│   ├── CategoryService/       # Category management
│   ├── BudgetService/         # Budget tracking
│   ├── SavingsGoalService/    # Savings goals
│   └── AnalyticsService/      # Analytics & reporting
├── Infrastructure/            # Deployment configurations
│   ├── Docker/               # Docker Compose setups
│   ├── Kubernetes/           # Kubernetes manifests
│   └── Monitoring/           # Prometheus & Grafana configs
├── docs/                     # Documentation (this folder)
└── scripts/                  # Automation scripts
```

## 🔄 Documentation Updates

This documentation is actively maintained and updated with each major release. For the most current information:

- Check the git commit history for recent changes
- Verify dates in individual documents
- Cross-reference with the main README.md for version alignment

## 📞 Getting Help

1. **Development Issues**: Check [QUALITY_IMPROVEMENTS_STATUS.md](QUALITY_IMPROVEMENTS_STATUS.md)
2. **Infrastructure Issues**: Review [INFRASTRUCTURE_IMPROVEMENTS.md](INFRASTRUCTURE_IMPROVEMENTS.md)
3. **Security Concerns**: Follow procedures in [SECURITY.md](SECURITY.md)
4. **Deployment Issues**: Consult [ArgoCD-Deployment.md](ArgoCD-Deployment.md)

## 📈 Documentation Metrics

- **Total Documents**: 7
- **Last Updated**: October 11, 2025
- **Coverage Areas**: Infrastructure, Security, Development, Deployment
- **Maintenance Status**: ✅ Active

---

*This index is automatically updated with each documentation change. For specific questions not covered in these documents, please create an issue in the project repository.*