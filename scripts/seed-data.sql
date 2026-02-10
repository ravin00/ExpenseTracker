-- Sample Data for ExpenseTracker Database
-- This script populates all databases with realistic test data

-- =====================================================
-- 1. AUTH SERVICE - Create Test User
-- =====================================================
\c authservice

-- Insert test user (password: Test123!)
-- Password hash is for "Test123!" using BCrypt
INSERT INTO "Users" ("Username", "Email", "PasswordHash", "CreatedAt", "UpdatedAt", "IsActive")
VALUES 
    ('johndoe', 'john@example.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYILSBL8EBK', NOW() - INTERVAL '30 days', NOW(), true),
    ('janedoe', 'jane@example.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYILSBL8EBK', NOW() - INTERVAL '60 days', NOW(), true)
ON CONFLICT ("Email") DO NOTHING;

-- =====================================================
-- 2. CATEGORY SERVICE - Create Categories
-- =====================================================
\c categoryservice

INSERT INTO "Categories" ("UserId", "Name", "Description", "Icon", "Color", "CreatedAt", "UpdatedAt", "IsActive")
VALUES 
    (1, 'Food & Dining', 'Restaurants, groceries, food delivery', '🍔', '#FF6B6B', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Transportation', 'Gas, public transit, ride sharing', '🚗', '#4ECDC4', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Shopping', 'Clothing, electronics, household items', '🛍️', '#95E1D3', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Entertainment', 'Movies, games, subscriptions', '🎮', '#F38181', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Healthcare', 'Medical expenses, pharmacy, fitness', '🏥', '#AA96DA', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Utilities', 'Electricity, water, internet, phone', '💡', '#FCBAD3', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Housing', 'Rent, mortgage, maintenance', '🏠', '#A8D8EA', NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Education', 'Courses, books, learning materials', '📚', '#FFD93D', NOW() - INTERVAL '30 days', NOW(), true)
ON CONFLICT DO NOTHING;

-- =====================================================
-- 3. EXPENSE SERVICE - Create Sample Expenses
-- =====================================================
\c expenseservice

INSERT INTO "Expenses" ("UserId", "Description", "Amount", "Date", "Category", "Notes", "CreatedAt", "UpdatedAt", "IsActive")
VALUES 
    -- This month's expenses
    (1, 'Grocery shopping at Whole Foods', 156.78, NOW() - INTERVAL '2 days', 'Food & Dining', 'Weekly groceries', NOW() - INTERVAL '2 days', NOW(), true),
    (1, 'Netflix subscription', 15.99, NOW() - INTERVAL '3 days', 'Entertainment', 'Monthly subscription', NOW() - INTERVAL '3 days', NOW(), true),
    (1, 'Gas station fill-up', 45.50, NOW() - INTERVAL '5 days', 'Transportation', NULL, NOW() - INTERVAL '5 days', NOW(), true),
    (1, 'Dinner at Italian restaurant', 87.65, NOW() - INTERVAL '6 days', 'Food & Dining', 'Date night', NOW() - INTERVAL '6 days', NOW(), true),
    (1, 'Gym membership', 49.99, NOW() - INTERVAL '7 days', 'Healthcare', 'Monthly fee', NOW() - INTERVAL '7 days', NOW(), true),
    (1, 'Electric bill', 125.00, NOW() - INTERVAL '8 days', 'Utilities', 'Monthly utility', NOW() - INTERVAL '8 days', NOW(), true),
    (1, 'New running shoes', 89.99, NOW() - INTERVAL '10 days', 'Shopping', 'Nike Air', NOW() - INTERVAL '10 days', NOW(), true),
    (1, 'Uber ride to airport', 35.00, NOW() - INTERVAL '12 days', 'Transportation', NULL, NOW() - INTERVAL '12 days', NOW(), true),
    (1, 'Coffee shop', 12.50, NOW() - INTERVAL '14 days', 'Food & Dining', 'Morning coffee', NOW() - INTERVAL '14 days', NOW(), true),
    (1, 'Movie tickets', 28.00, NOW() - INTERVAL '15 days', 'Entertainment', 'Avatar 3', NOW() - INTERVAL '15 days', NOW(), true),
    
    -- Last month's expenses
    (1, 'Rent payment', 1500.00, NOW() - INTERVAL '35 days', 'Housing', 'Monthly rent', NOW() - INTERVAL '35 days', NOW(), true),
    (1, 'Grocery shopping', 142.30, NOW() - INTERVAL '40 days', 'Food & Dining', NULL, NOW() - INTERVAL '40 days', NOW(), true),
    (1, 'Internet bill', 79.99, NOW() - INTERVAL '42 days', 'Utilities', 'Monthly internet', NOW() - INTERVAL '42 days', NOW(), true),
    (1, 'Gas', 52.00, NOW() - INTERVAL '43 days', 'Transportation', NULL, NOW() - INTERVAL '43 days', NOW(), true),
    (1, 'Restaurant dinner', 65.00, NOW() - INTERVAL '45 days', 'Food & Dining', NULL, NOW() - INTERVAL '45 days', NOW(), true),
    (1, 'Online course', 199.00, NOW() - INTERVAL '47 days', 'Education', 'Web Development course', NOW() - INTERVAL '47 days', NOW(), true),
    (1, 'New headphones', 149.99, NOW() - INTERVAL '50 days', 'Shopping', 'Sony WH-1000XM5', NOW() - INTERVAL '50 days', NOW(), true),
    (1, 'Doctor visit', 75.00, NOW() - INTERVAL '52 days', 'Healthcare', 'Annual checkup', NOW() - INTERVAL '52 days', NOW(), true),
    (1, 'Spotify premium', 9.99, NOW() - INTERVAL '55 days', 'Entertainment', 'Monthly subscription', NOW() - INTERVAL '55 days', NOW(), true),
    (1, 'Lunch with colleagues', 42.50, NOW() - INTERVAL '58 days', 'Food & Dining', NULL, NOW() - INTERVAL '58 days', NOW(), true)
ON CONFLICT DO NOTHING;

-- =====================================================
-- 4. BUDGET SERVICE - Create Monthly Budgets
-- =====================================================
\c budgetservice

INSERT INTO "Budgets" ("UserId", "CategoryId", "Amount", "SpentAmount", "Period", "StartDate", "EndDate", "CreatedAt", "UpdatedAt", "IsActive")
VALUES 
    -- Current month budgets
    (1, 1, 600.00, 399.23, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 2, 200.00, 80.50, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 3, 300.00, 89.99, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 4, 150.00, 43.99, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 5, 200.00, 49.99, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 6, 300.00, 125.00, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 7, 1500.00, 0.00, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true),
    (1, 8, 250.00, 0.00, 'Monthly', DATE_TRUNC('month', NOW()), DATE_TRUNC('month', NOW()) + INTERVAL '1 month', NOW() - INTERVAL '20 days', NOW(), true)
ON CONFLICT DO NOTHING;

-- =====================================================
-- 5. SAVINGS GOAL SERVICE - Create Savings Goals
-- =====================================================
\c savingsgoalservice

INSERT INTO "SavingsGoals" ("UserId", "Name", "Description", "TargetAmount", "CurrentAmount", "Deadline", "Priority", "CategoryId", "CreatedAt", "UpdatedAt", "IsActive")
VALUES 
    (1, 'Emergency Fund', 'Build 6-month emergency fund', 10000.00, 3500.00, NOW() + INTERVAL '180 days', 'High', NULL, NOW() - INTERVAL '60 days', NOW(), true),
    (1, 'Vacation to Japan', 'Summer trip to Tokyo and Kyoto', 5000.00, 1200.00, NOW() + INTERVAL '120 days', 'Medium', NULL, NOW() - INTERVAL '45 days', NOW(), true),
    (1, 'New Laptop', 'MacBook Pro for development', 2500.00, 800.00, NOW() + INTERVAL '90 days', 'High', NULL, NOW() - INTERVAL '30 days', NOW(), true),
    (1, 'Car Down Payment', 'Save for new car purchase', 8000.00, 2100.00, NOW() + INTERVAL '240 days', 'Medium', NULL, NOW() - INTERVAL '90 days', NOW(), true),
    (1, 'Home Renovation', 'Kitchen remodel project', 15000.00, 4500.00, NOW() + INTERVAL '365 days', 'Low', NULL, NOW() - INTERVAL '120 days', NOW(), true)
ON CONFLICT DO NOTHING;

-- Display summary
\c authservice
SELECT 'Users created:' as info, COUNT(*) as count FROM "Users";

\c categoryservice
SELECT 'Categories created:' as info, COUNT(*) as count FROM "Categories";

\c expenseservice
SELECT 'Expenses created:' as info, COUNT(*) as count FROM "Expenses";
SELECT 'Total expense amount:' as info, ROUND(SUM("Amount")::numeric, 2) as total FROM "Expenses";

\c budgetservice
SELECT 'Budgets created:' as info, COUNT(*) as count FROM "Budgets";

\c savingsgoalservice
SELECT 'Savings goals created:' as info, COUNT(*) as count FROM "SavingsGoals";
