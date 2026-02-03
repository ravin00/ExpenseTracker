import { budgetApi } from '@/services/budget.service'
import { useBudgetStore } from '@/stores/budget-store'
import { useQuery } from '@tanstack/react-query'
import { useMemo } from 'react'

export function useBudgets() {
    const { filters } = useBudgetStore()

    const { data: budgets, isLoading, error } = useQuery({
        queryKey: ['budgets', filters.activeOnly],
        queryFn: () => budgetApi.getAll(filters.activeOnly),
    })

    // Apply client-side filters using Zustand state
    const filteredBudgets = useMemo(() => {
        if (!budgets) return []

        let filtered = [...budgets]

        // Filter by search query
        if (filters.searchQuery) {
            const query = filters.searchQuery.toLowerCase()
            filtered = filtered.filter(
                (budget) =>
                    budget.name.toLowerCase().includes(query) ||
                    budget.description?.toLowerCase().includes(query)
            )
        }

        // Filter by period
        if (filters.period !== undefined) {
            filtered = filtered.filter((budget) => budget.period === filters.period)
        }

        // Filter by category
        if (filters.categoryId !== undefined) {
            filtered = filtered.filter((budget) => budget.categoryId === filters.categoryId)
        }

        return filtered
    }, [budgets, filters])

    return {
        budgets: filteredBudgets,
        allBudgets: budgets,
        isLoading,
        error,
    }
}
