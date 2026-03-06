import { AuthGuard } from '@/features/auth/components/AuthGuard'
import { BudgetDetailsPage } from '@/features/product/pages/budget-details-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/budgets/$budgetId')({
  component: BudgetDetailsRoute,
})

function BudgetDetailsRoute() {
  const { budgetId } = Route.useParams()

  return (
    <AuthGuard>
      <BudgetDetailsPage budgetId={budgetId} />
    </AuthGuard>
  )
}
