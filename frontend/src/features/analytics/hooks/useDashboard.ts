import { useQuery } from '@tanstack/react-query'
import { analyticsApi } from '../services/analytics.api'

export function useDashboard() {
    const { data: dashboard, isLoading, error, refetch } = useQuery({
        queryKey: ['dashboard'],
        queryFn: analyticsApi.getDashboard,
        staleTime: 1000 * 60 * 5, // Cache for 5 minutes
    })

    return { dashboard, isLoading, error, refetch }
}