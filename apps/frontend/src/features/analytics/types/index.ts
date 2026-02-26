// Analytics Types

export type AnalyticsPeriod = 'Daily' | 'Weekly' | 'Monthly' | 'Quarterly' | 'Yearly'

export interface Analytics {
    id: number
    userId: number
    date: string
    totalExpenses: number
    totalIncome: number
    totalSavings: number
    budgetUtilization: number
    categoryId: number
    categoryName?: string
    categoryAmount: number
    period: AnalyticsPeriod
    createdAt: string
    updatedAt: string
    // Computed
    netIncome: number
    savingsRate: number
    expenseRatio: number
    isPositiveCashFlow: boolean
    financialHealthStatus: string
}

export interface AnalyticsDto {
    date: string
    totalExpenses: number
    totalIncome: number
    totalSavings: number
    budgetUtilization?: number
    categoryId?: number
    categoryName?: string
    categoryAmount?: number
    period: AnalyticsPeriod
}

export interface ExpenseByCategory {
    categoryId: number
    categoryName: string
    amount: number
    percentage: number
    transactionCount: number
}

export interface SpendingTrend {
    date: string
    amount: number
    period: AnalyticsPeriod
}

export interface BudgetAnalysisData {
    budgetId: number
    budgetName: string
    budgetAmount: number
    spentAmount: number
    remainingAmount: number
    utilizationPercentage: number
    isOverBudget: boolean
}

export interface FinancialSummaryData {
    totalIncome: number
    totalExpenses: number
    totalSavings: number
    netWorth: number
    savingsRate: number
    financialHealth: string
    topCategories: ExpenseByCategory[]
    spendingTrends: SpendingTrend[]
    budgetAnalyses: BudgetAnalysisData[]
}

export interface DashboardData {
    summary: FinancialSummaryData
    recentExpenses: { id: number; description: string; amount: number; date: string; category?: string }[]
    activeBudgets: { id: number; name: string; amount: number; spentAmount: number; progressPercentage: number }[]
    savingsGoals: { id: number; name: string; targetAmount: number; currentAmount: number; progressPercentage: number }[]
    alerts: { type: string; message: string; severity: 'info' | 'warning' | 'danger' }[]
}
