// Savings Goal Types

export type SavingsGoalStatus = 'Active' | 'Paused' | 'Completed' | 'Cancelled'
export type SavingsGoalPriority = 'Low' | 'Medium' | 'High' | 'Critical'

export interface SavingsGoal {
    id: number
    userId: number
    name: string
    description?: string
    targetAmount: number
    currentAmount: number
    startDate: string
    targetDate?: string
    status: SavingsGoalStatus
    priority: SavingsGoalPriority
    color?: string
    categoryId?: number
    monthlyContributionTarget: number
    isAutoContribution: boolean
    createdAt: string
    updatedAt: string
    completedAt?: string
    // Computed properties
    progressPercentage: number
    remainingAmount: number
    isCompleted: boolean
}

export interface SavingsGoalDto {
    name: string
    description?: string
    targetAmount: number
    currentAmount?: number
    startDate?: string
    targetDate?: string
    status?: SavingsGoalStatus
    priority?: SavingsGoalPriority
    color?: string
    categoryId?: number
    monthlyContributionTarget?: number
    isAutoContribution?: boolean
}

export interface SavingsGoalStatistics {
    totalGoals: number
    completedGoals: number
    activeGoals: number
    totalTargetAmount: number
    totalCurrentAmount: number
    overallProgress: number
}

export interface ContributionDto {
    amount: number
}

export interface WithdrawDto {
    amount: number
}
