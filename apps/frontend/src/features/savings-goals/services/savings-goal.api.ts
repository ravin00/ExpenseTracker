// Savings Goal API Service
import { api } from '@/lib/api'
import type { SavingsGoal, SavingsGoalDto, SavingsGoalPriority, SavingsGoalStatistics } from '../types'

export const savingsGoalApi = {
    getAll: async (): Promise<SavingsGoal[]> => {
        return api.get<SavingsGoal[]>('/savingsgoal')
    },

    getById: async (id: number): Promise<SavingsGoal> => {
        return api.get<SavingsGoal>(`/savingsgoal/${id}`)
    },

    getByCategory: async (categoryId: number): Promise<SavingsGoal[]> => {
        return api.get<SavingsGoal[]>(`/savingsgoal/category/${categoryId}`)
    },

    getByPriority: async (priority: SavingsGoalPriority): Promise<SavingsGoal[]> => {
        return api.get<SavingsGoal[]>(`/savingsgoal/priority/${priority}`)
    },

    getDueSoon: async (days: number = 30): Promise<SavingsGoal[]> => {
        return api.get<SavingsGoal[]>(`/savingsgoal/due-soon?days=${days}`)
    },

    getStatistics: async (): Promise<SavingsGoalStatistics> => {
        return api.get<SavingsGoalStatistics>('/savingsgoal/statistics')
    },

    create: async (dto: SavingsGoalDto): Promise<SavingsGoal> => {
        return api.post<SavingsGoal>('/savingsgoal', dto)
    },

    update: async (id: number, dto: SavingsGoalDto): Promise<SavingsGoal> => {
        return api.put<SavingsGoal>(`/savingsgoal/${id}`, dto)
    },

    addContribution: async (id: number, amount: number): Promise<SavingsGoal> => {
        return api.post<SavingsGoal>(`/savingsgoal/${id}/contribute`, { amount })
    },

    withdraw: async (id: number, amount: number): Promise<SavingsGoal> => {
        return api.post<SavingsGoal>(`/savingsgoal/${id}/withdraw`, { amount })
    },

    delete: async (id: number): Promise<void> => {
        return api.delete(`/savingsgoal/${id}`)
    },
}
