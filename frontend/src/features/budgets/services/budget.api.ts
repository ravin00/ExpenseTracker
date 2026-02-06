// Budget API Service
import { api } from '@/lib/api'
import type { Budget, BudgetDto, BudgetWithProgress } from '../types'

export const budgetApi = {
    getAll: async (activeOnly = false): Promise<Budget[]> => {
        const queryParam = activeOnly ? '?activeOnly=true' : ''
        return api.get<Budget[]>(`/budget${queryParam}`)
    },

    getById: async (id: number): Promise<BudgetWithProgress> => {
        return api.get<BudgetWithProgress>(`/budget/${id}`)
    },

    getByCategory: async (categoryId: number): Promise<Budget[]> => {
        return api.get<Budget[]>(`/budget/category/${categoryId}`)
    },

    create: async (dto: BudgetDto): Promise<Budget> => {
        return api.post<Budget>('/budget', dto)
    },

    update: async (id: number, dto: BudgetDto): Promise<Budget> => {
        return api.put<Budget>(`/budget/${id}`, dto)
    },

    updateSpentAmount: async (id: number, spentAmount: number): Promise<void> => {
        return api.put(`/budget/${id}/spent`, spentAmount)
    },

    delete: async (id: number): Promise<void> => {
        return api.delete(`/budget/${id}`)
    },
}
