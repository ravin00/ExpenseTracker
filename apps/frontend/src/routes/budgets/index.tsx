import { AuthGuard } from '@/features/auth/components/AuthGuard'
import { BudgetsListPage } from '@/features/product/pages/budgets-list-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/budgets/')({
  component: BudgetsRoute,
})

function BudgetsRoute() {
  return (
    <AuthGuard>
      <BudgetsListPage />
    </AuthGuard>
  )
}
