// Budget Types

export type BudgetPeriod = 'Daily' | 'Weekly' | 'Monthly' | 'Quarterly' | 'Yearly' | 'Custom'
export type BudgetHealthStatus = 'Healthy' | 'Caution' | 'Warning' | 'OverBudget'

export interface Budget {
    id: number
    userId: number
    name: string
    description?: string
    amount: number
    categoryId?: number
    period: BudgetPeriod
    startDate: string
    endDate?: string
    spentAmount: number
    isActive: boolean
    createdAt: string
    updatedAt: string
    // Computed properties from backend
    progressPercentage: number
    remainingAmount: number
    isExceeded: boolean
    isCompleted: boolean
    daysRemaining: number
    healthStatus: BudgetHealthStatus
    statusText: string
}

export interface BudgetDto {
    name: string
    description?: string
    amount: number
    categoryId?: number
    period: BudgetPeriod
    startDate: string
    endDate?: string
    isActive?: boolean
}

export interface BudgetWithProgress {
    budget: Budget
    progress: number
    remaining: number
    isExceeded: boolean
}

export interface BudgetFilters {
    period?: BudgetPeriod
    categoryId?: number
    healthStatus?: BudgetHealthStatus
    activeOnly?: boolean
}
