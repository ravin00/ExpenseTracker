import { useQuery } from '@tanstack/react-query'
import { analyticsApi } from '../services/analytics.api'
import type { AnalyticsPeriod } from '../types'

export function useSpendingTrends(period: AnalyticsPeriod = 'Monthly') {
    const { data: trends = [], isLoading, error } = useQuery({
        queryKey: ['spending-trends', period],
        queryFn: () => analyticsApi.getSpendingTrends(period),
    })

    return { trends, isLoading, error }
}