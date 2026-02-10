// Analytics API Service
import { api } from '@/lib/api'
import type {
    Analytics,
    AnalyticsDto,
    DashboardData,
    ExpenseByCategory,
    FinancialSummaryData,
    SpendingTrend,
} from '../types'

export const analyticsApi = {
    getAll: async (): Promise<Analytics[]> => {
        return api.get<Analytics[]>('/analytics')
    },

    getById: async (id: number): Promise<Analytics> => {
        return api.get<Analytics>(`/analytics/${id}`)
    },

    getByDateRange: async (startDate: string, endDate: string): Promise<Analytics[]> => {
        return api.get<Analytics[]>(`/analytics/range?startDate=${startDate}&endDate=${endDate}`)
    },

    getDashboard: async (): Promise<DashboardData> => {
        return api.get<DashboardData>('/analytics/dashboard')
    },

    getFinancialSummary: async (startDate?: string, endDate?: string): Promise<FinancialSummaryData> => {
        const params = new URLSearchParams()
        if (startDate) params.append('startDate', startDate)
        if (endDate) params.append('endDate', endDate)
        const query = params.toString() ? `?${params.toString()}` : ''
        return api.get<FinancialSummaryData>(`/analytics/summary${query}`)
    },

    getExpensesByCategory: async (startDate?: string, endDate?: string): Promise<ExpenseByCategory[]> => {
        const params = new URLSearchParams()
        if (startDate) params.append('startDate', startDate)
        if (endDate) params.append('endDate', endDate)
        const query = params.toString() ? `?${params.toString()}` : ''
        return api.get<ExpenseByCategory[]>(`/analytics/categories${query}`)
    },

    getSpendingTrends: async (period?: string): Promise<SpendingTrend[]> => {
        const params = period ? `?period=${period}` : ''
        return api.get<SpendingTrend[]>(`/analytics/trends${params}`)
    },

    create: async (dto: AnalyticsDto): Promise<Analytics> => {
        return api.post<Analytics>('/analytics', dto)
    },

    generateAnalytics: async (period: string): Promise<Analytics> => {
        return api.post<Analytics>(`/analytics/generate`, { period })
    },

    health: async (): Promise<{ status: string }> => {
        return api.get('/analytics/health')
    },
}
