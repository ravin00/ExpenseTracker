#!/bin/bash

# ExpenseTracker Management Script

case "$1" in
    "start")
        echo "Starting ExpenseTracker microservices..."
        docker-compose up --build -d
        echo " Services started!"
        echo " API Gateway: http://localhost:8080"
        echo " Auth Service: http://localhost:5001"
        echo " Expense Service: http://localhost:5002"
        ;;
    
    "stop")
        echo "Stopping ExpenseTracker microservices..."
        docker-compose down
        echo "Services stopped!"
        ;;
    
    "restart")
        echo "Restarting ExpenseTracker microservices..."
        docker-compose down
        docker-compose up --build -d
        echo " Services restarted!"
        ;;
    
    "logs")
        if [ -n "$2" ]; then
            echo " Showing logs for $2..."
            docker-compose logs -f "$2"
        else
            echo " Showing logs for all services..."
            docker-compose logs -f
        fi
        ;;
    
    "clean")
        echo "🧹 Cleaning up ExpenseTracker (including volumes)..."
        docker-compose down -v
        docker system prune -f
        echo " Cleanup complete!"
        ;;
    
    "status")
        echo "ExpenseTracker service status:"
        docker-compose ps
        ;;
    
    "test")
        echo "Testing services..."
        echo "Testing API Gateway..."
        curl -s http://localhost:8080/health || echo " Gateway not responding"
        echo -e "\nTesting Auth Service..."
        curl -s http://localhost:5001/swagger/index.html > /dev/null && echo "Auth Service OK" || echo "Auth Service not responding"
        echo "Testing Expense Service..."
        curl -s http://localhost:5002/swagger/index.html > /dev/null && echo "Expense Service OK" || echo " Expense Service not responding"
        ;;
    
    "db")
        echo "Connecting to SQL Server..."
        docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ExpenseTracker123!
        ;;
    
    *)
        echo "ExpenseTracker Management Script"
        echo ""
        echo "Usage: $0 {start|stop|restart|logs|clean|status|test|db}"
        echo ""
        echo "Commands:"
        echo "  start    - Start all services"
        echo "  stop     - Stop all services"
        echo "  restart  - Restart all services"
        echo "  logs     - Show logs (add service name for specific service)"
        echo "  clean    - Stop services and remove volumes"
        echo "  status   - Show service status"
        echo "  test     - Test service connectivity"
        echo "  db       - Connect to SQL Server"
        echo ""
        echo "Examples:"
        echo "  $0 start"
        echo "  $0 logs auth-service"
        echo "  $0 status"
        ;;
esac