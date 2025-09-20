# ExpenseTracker Microservices

A microservices-based expense tracking application built with .NET 9.0, featuring authentication and expense management services.

## Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────────┐
│   API Gateway   │    │   Auth Service   │    │  Expense Service    │
│   (Nginx)       │────│   (Port 5001)    │────│   (Port 5002)       │
│   Port 8080     │    │                  │    │                     │
└─────────────────┘    └──────────────────┘    └─────────────────────┘
                                │                          │
                                └──────────┬───────────────┘
                                           │
                                  ┌────────────────┐
                                  │  SQL Server    │
                                  │  (Port 1433)   │
                                  └────────────────┘
```

## Services

### 1. **Auth Service** (Port 5001)
- User registration and authentication
- JWT token generation
- User management
- Database: `ExpenseTrackerAuth`

### 2. **Expense Service** (Port 5002)
- Expense CRUD operations
- JWT token validation
- User-specific expense filtering
- Database: `ExpenseTrackerExpenses`

### 3. **API Gateway** (Port 8080)
- Nginx reverse proxy
- Route management
- Load balancing
- Centralized entry point

### 4. **SQL Server** (Port 1433)
- Shared database server
- Separate databases per service
- Health checks enabled

## Quick Start

### Prerequisites
- Docker and Docker Compose
- Git

### 1. Clone and Navigate
```bash
git clone <your-repo-url>
cd ExpenseTracker
```

### 2. Build and Run
```bash
# Build and start all services
docker-compose up --build

# Or run in background
docker-compose up --build -d
```

### 3. Verify Services
- **API Gateway**: http://localhost:8080
- **Auth Service**: http://localhost:5001
- **Expense Service**: http://localhost:5002
- **Auth Swagger**: http://localhost:8080/auth/swagger
- **Expense Swagger**: http://localhost:8080/expense/swagger

## API Endpoints

### Authentication Service
```bash
# Register a new user
POST http://localhost:8080/api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "SecurePassword123"
}

# Login
POST http://localhost:8080/api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "SecurePassword123"
}
```

### Expense Service (Requires JWT Token)
```bash
# Get all expenses
GET http://localhost:8080/api/expense
Authorization: Bearer <jwt-token>

# Create expense
POST http://localhost:8080/api/expense
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "description": "Coffee",
  "amount": 4.50,
  "date": "2025-09-20T10:30:00Z"
}

# Get specific expense
GET http://localhost:8080/api/expense/{id}
Authorization: Bearer <jwt-token>

# Update expense
PUT http://localhost:8080/api/expense/{id}
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "description": "Updated Coffee",
  "amount": 5.00,
  "date": "2025-09-20T10:30:00Z"
}

# Delete expense
DELETE http://localhost:8080/api/expense/{id}
Authorization: Bearer <jwt-token>
```

## Development

### Individual Service Development
```bash
# Run Auth Service only
docker-compose up sqlserver auth-service

# Run Expense Service only
docker-compose up sqlserver expense-service

# Run without gateway
docker-compose up sqlserver auth-service expense-service
```

### Local .NET Development
```bash
# Auth Service
cd Backend/AuthService
dotnet run

# Expense Service
cd Backend/ExpenseService
dotnet run
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f auth-service
docker-compose logs -f expense-service
docker-compose logs -f sqlserver
```

## Configuration

### Environment Variables (.env file)
- `DB_PASSWORD`: SQL Server SA password
- `JWT_SECRET`: JWT signing secret key
- `AUTH_SERVICE_PORT`: Auth service port (default: 5001)
- `EXPENSE_SERVICE_PORT`: Expense service port (default: 5002)
- `GATEWAY_PORT`: API Gateway port (default: 8080)

### Database Connection Strings
- **Auth DB**: `Server=sqlserver,1433;Database=ExpenseTrackerAuth;User Id=sa;Password=<password>;TrustServerCertificate=true;`
- **Expense DB**: `Server=sqlserver,1433;Database=ExpenseTrackerExpenses;User Id=sa;Password=<password>;TrustServerCertificate=true;`

## Troubleshooting

### Common Issues

1. **Port conflicts**
   ```bash
   # Check what's using ports
   lsof -i :8080
   lsof -i :5001
   lsof -i :5002
   lsof -i :1433
   ```

2. **Database connection issues**
   ```bash
   # Check SQL Server health
   docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ExpenseTracker123! -Q "SELECT 1"
   ```

3. **Service not starting**
   ```bash
   # Rebuild specific service
   docker-compose build auth-service
   docker-compose up auth-service
   ```

4. **Clear everything and restart**
   ```bash
   docker-compose down -v
   docker-compose up --build
   ```

### Useful Commands
```bash
# Stop all services
docker-compose down

# Stop and remove volumes (clears database)
docker-compose down -v

# View running containers
docker-compose ps

# Execute commands in containers
docker-compose exec auth-service bash
docker-compose exec sqlserver bash

# Scale services (if needed)
docker-compose up --scale expense-service=2
```

## Testing the Complete Flow

1. **Start all services**
   ```bash
   docker-compose up --build
   ```

2. **Register a user**
   ```bash
   curl -X POST http://localhost:8080/api/auth/register \
     -H "Content-Type: application/json" \
     -d '{"username":"testuser","email":"test@example.com","password":"SecurePassword123"}'
   ```

3. **Login and get JWT token**
   ```bash
   curl -X POST http://localhost:8080/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"email":"test@example.com","password":"SecurePassword123"}'
   ```

4. **Use the token to create an expense**
   ```bash
   curl -X POST http://localhost:8080/api/expense \
     -H "Content-Type: application/json" \
     -H "Authorization: Bearer <your-jwt-token>" \
     -d '{"description":"Coffee","amount":4.50}'
   ```

5. **Get all expenses**
   ```bash
   curl -X GET http://localhost:8080/api/expense \
     -H "Authorization: Bearer <your-jwt-token>"
   ```

## Production Considerations

- Use proper secrets management (Azure Key Vault, AWS Secrets Manager)
- Implement health checks for all services
- Add monitoring and logging (ELK stack, Application Insights)
- Use production-grade databases
- Implement API rate limiting
- Add SSL/TLS certificates
- Consider container orchestration (Kubernetes)
- Implement proper backup strategies