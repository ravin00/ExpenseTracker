import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { analyticsApi } from '../services/analytics.api'

function formatDate(date: Date): string {
    return date.toISOString().split('T')[0]
}

function resolveDateRange(startDate?: string, endDate?: string): { startDate: string; endDate: string } {
    if (startDate && endDate) {
        return { startDate, endDate }
    }

    const currentDate = new Date()
    const start = new Date(currentDate)
    start.setDate(currentDate.getDate() - 30)

    return {
        startDate: startDate ?? formatDate(start),
        endDate: endDate ?? formatDate(currentDate),
    }
}

export function useAnalytics(startDate?: string, endDate?: string) {
    const { data: analytics = [], isLoading, error, refetch } = useQuery({
        queryKey: ['analytics', startDate, endDate],
        queryFn: () => startDate && endDate
            ? analyticsApi.getByDateRange(startDate, endDate)
            : analyticsApi.getAll(),
    })

    return { analytics, isLoading, error, refetch }
}

export function useExpensesByCategory(startDate?: string, endDate?: string) {
    const { data: categories = [], isLoading, error } = useQuery({
        queryKey: ['expenses-by-category', startDate, endDate],
        queryFn: () => {
            const range = resolveDateRange(startDate, endDate)
            return analyticsApi.getExpensesByCategory(range.startDate, range.endDate)
        },
    })

    return { categories, isLoading, error }
}

export function useFinancialSummary(startDate?: string, endDate?: string) {
    const { data: summary, isLoading, error } = useQuery({
        queryKey: ['financial-summary', startDate, endDate],
        queryFn: () => {
            const range = resolveDateRange(startDate, endDate)
            return analyticsApi.getFinancialSummary(range.startDate, range.endDate)
        },
    })

    return { summary, isLoading, error }
}

export function useGenerateAnalytics() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (period: string) => analyticsApi.generateAnalytics(period),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['financial-summary'] })
            queryClient.invalidateQueries({ queryKey: ['expenses-by-category'] })
            queryClient.invalidateQueries({ queryKey: ['spending-trends'] })
        },
    })
}
