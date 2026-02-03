import { Budget, BudgetPeriod } from '@/types/budget.types'
import { create } from 'zustand'

interface BudgetFilters {
    activeOnly: boolean
    period?: BudgetPeriod
    searchQuery: string
    categoryId?: number
}

interface BudgetState {
    selectedBudget: Budget | null
    filters: BudgetFilters
    setSelectedBudget: (budget: Budget | null) => void
    setFilters: (filters: Partial<BudgetFilters>) => void
    resetFilters: () => void
}

const defaultFilters: BudgetFilters = {
    activeOnly: true,
    searchQuery: '',
}

export const useBudgetStore = create<BudgetState>((set) => ({
    selectedBudget: null,
    filters: defaultFilters,

    setSelectedBudget: (budget) => set({ selectedBudget: budget }),

    setFilters: (newFilters) =>
        set((state) => ({
            filters: { ...state.filters, ...newFilters },
        })),

    resetFilters: () => set({ filters: defaultFilters }),
}))
