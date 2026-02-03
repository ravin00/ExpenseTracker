# ExpenseTracker User Flows & API Mapping

## 🔄 **Core User Flows**

### **1. Authentication Flow**
```
Landing Page → Login/Register → Dashboard
     ↓
[If not logged in] → Auth Page
     ↓
Enter credentials → API: POST /api/Auth/login
     ↓
Receive JWT Token → Store in localStorage
     ↓
Redirect to Dashboard
```

### **2. Expense Management Flow**
```
Dashboard → View Expenses → Add/Edit/Delete
     ↓
API: GET /api/Expense (list all)
     ↓
Click "Add Expense" → Expense Form Modal
     ↓
Fill form → API: POST /api/Expense
     ↓
Success → Update list → Close modal
     ↓
Edit existing → API: PUT /api/Expense/{id}
     ↓
Delete → Confirm → API: DELETE /api/Expense/{id}
```

### **3. Category Management Flow**
```
Categories Page → View Categories Grid
     ↓
API: GET /api/Category (list all)
     ↓
Create Category → Category Form Modal
     ↓
API: POST /api/Category
     ↓
Edit Category → Update Form → API: PUT /api/Category/{id}
     ↓
Delete → Confirmation → API: DELETE /api/Category/{id}
```

### **4. Budget Flow**
```
Budgets Page → View Active Budgets
     ↓
API: GET /api/Budget
     ↓
Create Budget → Set amount, category, period
     ↓
API: POST /api/Budget
     ↓
Monitor spending → Real-time budget tracking
     ↓
API: GET /api/Budget/{id} (current status)
     ↓
Budget alerts → Notifications when near/over limit
```

### **5. Savings Goals Flow**
```
Goals Page → View Progress Cards
     ↓
API: GET /api/SavingsGoal
     ↓
Create Goal → Set target, deadline, category
     ↓
API: POST /api/SavingsGoal
     ↓
Add Contribution → API: POST /api/SavingsGoal/{id}/contribute
     ↓
Track Progress → Visual progress bars
     ↓
Goal Achievement → Celebration UI
```

### **6. Analytics Flow**
```
Analytics Page → Select Date Range
     ↓
API: GET /api/Analytics/range?startDate=&endDate=
     ↓
View Charts → Spending trends, category breakdown
     ↓
API: GET /api/Analytics/categories
API: GET /api/Analytics/trends
     ↓
Financial Health → API: GET /api/Analytics/health-status
     ↓
Generate Report → API: POST /api/Analytics/generate
```

## 📱 **Screen Flow Map**

### **Authentication Screens**
1. **Welcome/Landing** (Optional)
2. **Login Screen**
3. **Register Screen**
4. **Forgot Password** (Future)

### **Main Application Screens**
1. **Dashboard** (Home)
2. **Expenses List**
3. **Add/Edit Expense**
4. **Categories Grid**
5. **Add/Edit Category**
6. **Budgets List**
7. **Create/Edit Budget**
8. **Savings Goals**
9. **Add/Edit Goal**
10. **Analytics Dashboard**
11. **Profile/Settings**

### **Modal/Overlay Screens**
- Add Expense Form
- Edit Expense Form
- Add Category Form
- Create Budget Form
- Add Goal Form
- Contribute to Goal
- Delete Confirmations

## 🎯 **Key Features to Design**

### **Dashboard Highlights**
- Financial overview cards (Income, Expenses, Budget status, Savings)
- Quick action buttons
- Recent transactions list
- Spending trend chart
- Budget alerts/warnings

### **Expense Management**
- Filterable expense list
- Quick add expense (floating action button)
- Bulk operations
- Receipt attachments (future feature)
- Expense categories with icons

### **Budget Tracking**
- Visual progress bars
- Alert states (approaching limit, over budget)
- Monthly/weekly/custom periods
- Budget recommendations

### **Savings Goals**
- Progress visualization
- Target date tracking
- Contribution history
- Achievement celebrations

### **Analytics & Insights**
- Interactive charts
- Spending patterns
- Category comparisons
- Financial health score
- Export capabilities

## 🔄 **State Management Considerations**

### **Authentication State**
```typescript
interface AuthState {
  user: User | null;
  token: string | null;
  isLoading: boolean;
  isAuthenticated: boolean;
}
```

### **Expense State**
```typescript
interface ExpenseState {
  expenses: Expense[];
  categories: Category[];
  isLoading: boolean;
  filters: {
    dateRange: [Date, Date];
    category: string;
    amountRange: [number, number];
  };
}
```

### **Budget State**
```typescript
interface BudgetState {
  budgets: Budget[];
  currentMonth: BudgetSummary;
  alerts: BudgetAlert[];
  isLoading: boolean;
}
```

### **Goal State**
```typescript
interface GoalState {
  goals: SavingsGoal[];
  contributions: Contribution[];
  statistics: GoalStatistics;
  isLoading: boolean;
}
```

## 🎨 **Design Priorities**

### **1. Mobile-First Design**
- Touch-friendly buttons (minimum 44px)
- Swipe gestures for actions
- Responsive breakpoints
- Bottom navigation for mobile

### **2. Accessibility**
- WCAG 2.1 AA compliance
- Keyboard navigation
- Screen reader support
- High contrast mode
- Focus indicators

### **3. Performance**
- Lazy loading for charts
- Infinite scroll for long lists
- Optimistic UI updates
- Offline capability (future)

### **4. User Experience**
- Minimal form fields
- Smart defaults
- Auto-categorization
- Contextual help
- Progressive disclosure

## 📊 **Data Visualization Strategy**

### **Chart Types by Use Case**
- **Line Charts**: Spending trends over time
- **Pie/Donut Charts**: Expense category breakdown
- **Bar Charts**: Monthly comparisons
- **Progress Bars**: Budget and goal tracking
- **Area Charts**: Income vs expenses

### **Color Coding**
- Green: Income, positive trends, on-track budgets
- Red: Expenses, overspending, alerts
- Blue: Savings, goals, neutral actions
- Orange: Warnings, approaching limits
- Gray: Disabled, secondary information

## 🚀 **Implementation Phases**

### **Phase 1: Core Features**
1. Authentication (Login/Register)
2. Basic Dashboard
3. Expense CRUD operations
4. Category management

### **Phase 2: Advanced Features**
1. Budget creation and tracking
2. Savings goals
3. Basic analytics
4. Responsive design

### **Phase 3: Enhanced Experience**
1. Advanced analytics
2. Data export
3. Notifications
4. Settings/Profile

### **Phase 4: Future Enhancements**
1. Receipt scanning
2. Bank integration
3. Multi-currency support
4. Social features

This comprehensive flow map should guide your Figma design process and help you create a cohesive, user-friendly expense tracking application.