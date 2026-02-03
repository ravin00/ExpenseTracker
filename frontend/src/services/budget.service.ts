import { api } from '@/lib/api'
import { Budget, BudgetDto } from '@/types/budget.types'

export const budgetApi = {
    getAll: async (activeOnly = false): Promise<Budget[]> => {
        const queryParam = activeOnly ? '?activeOnly=true' : ''
        return api.get<Budget[]>(`/budget${queryParam}`)
    },

    getById: async (id: number): Promise<{
        budget: Budget
        progress: number
        remaining: number
        isExceeded: boolean
    }> => {
        return api.get(`/budget/${id}`)
    },

    getByCategory: async (categoryId: number): Promise<Budget[]> => {
        return api.get<Budget[]>(`/budget/category/${categoryId}`)
    },

    create: async (budget: BudgetDto): Promise<Budget> => {
        return api.post<Budget>('/budget', budget)
    },

    update: async (id: number, budget: BudgetDto): Promise<Budget> => {
        return api.put<Budget>(`/budget/${id}`, budget)
    },

    delete: async (id: number): Promise<void> => {
        return api.delete(`/budget/${id}`)
    },
}
