import { AuthGuard } from '@/features/auth/components/AuthGuard'
import { AnalyticsPage } from '@/features/product/pages/analytics-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/analytics')({
  component: AnalyticsRoute,
})

function AnalyticsRoute() {
  return (
    <AuthGuard>
      <AnalyticsPage />
    </AuthGuard>
  )
}
