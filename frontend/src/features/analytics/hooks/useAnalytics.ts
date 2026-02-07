import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { analyticsApi } from '../services/analytics.api'

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
        queryFn: () => analyticsApi.getExpensesByCategory(startDate, endDate),
    })

    return { categories, isLoading, error }
}

export function useFinancialSummary(startDate?: string, endDate?: string) {
    const { data: summary, isLoading, error } = useQuery({
        queryKey: ['financial-summary', startDate, endDate],
        queryFn: () => analyticsApi.getFinancialSummary(startDate, endDate),
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