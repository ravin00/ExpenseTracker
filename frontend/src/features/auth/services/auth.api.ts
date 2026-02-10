// Auth API Service
// Handles all authentication-related API calls

import { api } from '@/lib/api'
import type { AuthResponse, LoginDto, RegisterDto, User } from '../types'

export const authApi = {
    login: async (dto: LoginDto): Promise<AuthResponse> => {
        return api.post<AuthResponse>('/auth/login', dto)
    },

    register: async (dto: RegisterDto): Promise<AuthResponse> => {
        return api.post<AuthResponse>('/auth/register', dto)
    },

    getProfile: async (userId: number): Promise<User> => {
        return api.get<User>(`/auth/profile/${userId}`)
    },

    updateProfile: async (data: { username?: string; email?: string }): Promise<User> => {
        return api.put<User>('/auth/profile', data)
    },

    health: async (): Promise<{ status: string }> => {
        return api.get('/auth/health')
    },
}
