// Category API Service
import { api } from '@/lib/api'
import type { Category, CategoryDto } from '../types'

export const categoryApi = {
    getAll: async (): Promise<Category[]> => {
        return api.get<Category[]>('/category')
    },

    getById: async (id: number): Promise<Category> => {
        return api.get<Category>(`/category/${id}`)
    },

    create: async (dto: CategoryDto): Promise<Category> => {
        return api.post<Category>('/category', dto)
    },

    update: async (id: number, dto: CategoryDto): Promise<Category> => {
        return api.put<Category>(`/category/${id}`, dto)
    },

    delete: async (id: number): Promise<void> => {
        return api.delete(`/category/${id}`)
    },
}
