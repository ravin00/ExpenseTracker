import { createFileRoute } from '@tanstack/react-router'

import { AnalyticsDashboard } from '../features/analytics/components/AnalyticsDashboard'
import { AuthGuard } from '../features/auth/components/AuthGuard'

export const Route = createFileRoute('/analytics')({
    component: () => (
        <AuthGuard>
            <AnalyticsDashboard />
        </AuthGuard>
    ),
})
