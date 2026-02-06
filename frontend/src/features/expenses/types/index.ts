// Expense Types

export interface Expense {
    id: number
    userId: number
    description: string
    amount: number
    date: string
    category?: string
    notes?: string
    createdAt: string
    updatedAt: string
    isActive: boolean
}

export interface ExpenseCreateDto {
    description: string
    amount: number
    date: string
    category?: string
    notes?: string
}

export interface ExpenseUpdateDto {
    description: string
    amount: number
    date: string
    category?: string
    notes?: string
}

export interface ExpenseFilters {
    startDate?: string
    endDate?: string
    category?: string
    minAmount?: number
    maxAmount?: number
    search?: string
}

export interface ExpenseStats {
    totalExpenses: number
    averageExpense: number
    expenseCount: number
    topCategory: string
    monthlyChange: number
}
