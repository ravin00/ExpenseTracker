# Frontend Architecture

This document describes the frontend folder structure following industry best practices.

## 📁 Project Structure

```
src/
├── components/           # Shared UI components
│   ├── common/          # Reusable generic components
│   │   ├── Avatar.tsx
│   │   ├── Badge.tsx
│   │   ├── CurrencyDisplay.tsx
│   │   ├── EmptyState.tsx
│   │   ├── ErrorMessage.tsx
│   │   ├── LoadingSpinner.tsx
│   │   ├── ProgressBar.tsx
│   │   └── index.ts
│   ├── landing/         # Landing page components
│   ├── layout/          # App layout components (Sidebar, Header, etc.)
│   └── ui/              # shadcn/ui primitives
│
├── features/            # Feature-based modules (domain-driven)
│   ├── auth/            # Authentication feature
│   │   ├── components/  # Feature-specific components
│   │   ├── hooks/       # Feature-specific hooks
│   │   ├── services/    # API services
│   │   ├── types/       # TypeScript types
│   │   └── index.ts     # Barrel export
│   ├── expenses/        # Expense tracking feature
│   ├── budgets/         # Budget management feature
│   ├── categories/      # Category management feature
│   ├── savings-goals/   # Savings goals feature
│   ├── analytics/       # Analytics & reporting feature
│   ├── dashboard/       # Dashboard feature
│   └── index.ts
│
├── hooks/               # Global shared hooks
├── lib/                 # Utility libraries (api, utils)
├── routes/              # TanStack Router route definitions
├── services/            # Legacy/global services (migrating to features/)
├── stores/              # Global state (Zustand stores)
├── types/               # Global TypeScript types
└── main.tsx             # App entry point
```

## 🏗️ Architecture Principles

### 1. Feature-Based Architecture

Each feature is a self-contained module with:

- **components/** - React components specific to this feature
- **hooks/** - Custom hooks for data fetching and business logic
- **services/** - API service functions
- **types/** - TypeScript interfaces and types
- **index.ts** - Barrel export for clean imports

### 2. Separation of Concerns

- **Components** handle UI rendering
- **Hooks** manage data fetching and state
- **Services** handle API communication
- **Types** define data structures

### 3. Import Patterns

```typescript
// Import from feature modules
import { LoginForm, useAuth } from "@/features/auth";
import { ExpenseList, useExpenses } from "@/features/expenses";

// Import shared components
import { LoadingSpinner, Avatar } from "@/components/common";
import { AppLayout } from "@/components/layout";
```

### 4. Naming Conventions

- **Components**: PascalCase (e.g., `ExpenseCard.tsx`)
- **Hooks**: camelCase with `use` prefix (e.g., `useExpenses.ts`)
- **Services**: camelCase with `.api.ts` suffix (e.g., `expense.api.ts`)
- **Types**: PascalCase for interfaces (e.g., `Expense`, `ExpenseDto`)

## 🔧 Feature Modules

| Feature         | Description                    | Backend API        |
| --------------- | ------------------------------ | ------------------ |
| `auth`          | User authentication & profiles | `/api/auth`        |
| `expenses`      | Expense CRUD operations        | `/api/expense`     |
| `budgets`       | Budget management              | `/api/budget`      |
| `categories`    | Category management            | `/api/category`    |
| `savings-goals` | Savings goal tracking          | `/api/savingsgoal` |
| `analytics`     | Financial reports & insights   | `/api/analytics`   |
| `dashboard`     | Main dashboard widgets         | Multiple APIs      |

## 📦 Shared Components

### Common Components

- `LoadingSpinner` - Loading indicator
- `ErrorMessage` - Error display with retry
- `EmptyState` - Empty list placeholder
- `CurrencyDisplay` - Formatted currency
- `ProgressBar` - Progress indicator
- `Badge` - Status badges
- `Avatar` - User avatar with initials fallback

### Layout Components

- `AppLayout` - Main app wrapper
- `Sidebar` - Navigation sidebar
- `Header` - Top header bar
- `PageHeader` - Page title and actions
- `MobileNav` - Mobile navigation
- `UserMenu` - User dropdown menu
- `ThemeToggle` - Dark/light mode toggle
