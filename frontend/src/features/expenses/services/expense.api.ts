// Expense API Service
import { api } from '@/lib/api'
import type { Expense, ExpenseCreateDto, ExpenseUpdateDto } from '../types'

export const expenseApi = {
    getAll: async (): Promise<Expense[]> => {
        return api.get<Expense[]>('/expense')
    },

    getById: async (id: number): Promise<Expense> => {
        return api.get<Expense>(`/expense/${id}`)
    },

    create: async (dto: ExpenseCreateDto): Promise<Expense> => {
        return api.post<Expense>('/expense', dto)
    },

    update: async (id: number, dto: ExpenseUpdateDto): Promise<void> => {
        return api.put(`/expense/${id}`, dto)
    },

    delete: async (id: number): Promise<void> => {
        return api.delete(`/expense/${id}`)
    },
}
