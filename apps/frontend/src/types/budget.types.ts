export enum BudgetPeriod {
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Quarterly = 3,
    Yearly = 4,
    Custom = 5
}

export enum BudgetHealthStatus {
    Healthy = 0,
    Caution = 1,
    Warning = 2,
    OverBudget = 3
}

export type Budget = {
    id: number
    userId: number
    name: string
    description?: string | null
    amount: number
    categoryId?: number | null
    period: BudgetPeriod
    startDate: string
    endDate?: string | null
    spentAmount: number
    isActive: boolean
    createdAt: string
    updatedAt: string

    // Computed properties from C# model
    displayName: string
    progressPercentage: number
    remainingAmount: number
    isExceeded: boolean
    isCompleted: boolean
    exceededAmount: number
    daysRemaining: number
    daysOld: number
    isRecentlyCreated: boolean
    isRecentlyUpdated: boolean
    isNearDeadline: boolean
    statusText: string
    healthStatus: BudgetHealthStatus
}

export type BudgetDto = {
    name: string
    description?: string | null
    amount: number
    categoryId?: number | null
    period: BudgetPeriod
    startDate: string
    endDate?: string | null
    isActive?: boolean
}

export const BudgetPeriodLabels: Record<BudgetPeriod, string> = {
    [BudgetPeriod.Daily]: 'Daily',
    [BudgetPeriod.Weekly]: 'Weekly',
    [BudgetPeriod.Monthly]: 'Monthly',
    [BudgetPeriod.Quarterly]: 'Quarterly',
    [BudgetPeriod.Yearly]: 'Yearly',
    [BudgetPeriod.Custom]: 'Custom',
}

export const BudgetHealthStatusLabels: Record<BudgetHealthStatus, string> = {
    [BudgetHealthStatus.Healthy]: 'Healthy',
    [BudgetHealthStatus.Caution]: 'Caution',
    [BudgetHealthStatus.Warning]: 'Warning',
    [BudgetHealthStatus.OverBudget]: 'Over Budget',
}
