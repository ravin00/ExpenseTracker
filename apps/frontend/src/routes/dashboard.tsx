import { AuthGuard } from '@/features/auth/components/AuthGuard'
import { DashboardPage } from '@/features/product/pages/dashboard-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/dashboard')({
  component: DashboardRoute,
})

function DashboardRoute() {
  return (
    <AuthGuard>
      <DashboardPage />
    </AuthGuard>
  )
}
