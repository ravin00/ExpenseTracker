import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Skeleton } from '@/components/ui/skeleton'
import { AppModal, DataTable, EmptyState, FormField, PageHeader, StatsCard } from '@/components/product'
import { useDashboardQuery } from '@/features/product/data/api'
import { formatCurrency, formatDate } from '@/features/product/hooks/use-formatters'
import { cn } from '@/lib/utils'
import { useNavigate } from '@tanstack/react-router'
import { Activity, AlertTriangle, CircleDollarSign, FolderKanban, Plus, Wallet } from 'lucide-react'

const statIcons = [CircleDollarSign, FolderKanban, AlertTriangle, Wallet]

function statusClasses(status: 'on-track' | 'warning' | 'over') {
  if (status === 'on-track') return 'bg-emerald-500/10 text-emerald-700 dark:text-emerald-300'
  if (status === 'warning') return 'bg-amber-500/10 text-amber-700 dark:text-amber-300'
  return 'bg-rose-500/10 text-rose-700 dark:text-rose-300'
}

export function DashboardPage() {
  const navigate = useNavigate()
  const { data, isLoading, isError } = useDashboardQuery()

  const columns = [
    {
      key: 'name',
      header: 'Budget',
      render: (item: NonNullable<typeof data>['recentBudgets'][number]) => (
        <div>
          <p className="font-medium">{item.name}</p>
          <p className="text-xs text-muted-foreground">{item.category}</p>
        </div>
      ),
    },
    {
      key: 'spent',
      header: 'Spent',
      render: (item: NonNullable<typeof data>['recentBudgets'][number]) => formatCurrency(item.spent),
    },
    {
      key: 'remaining',
      header: 'Remaining',
      render: (item: NonNullable<typeof data>['recentBudgets'][number]) => formatCurrency(item.remaining),
    },
    {
      key: 'status',
      header: 'Status',
      render: (item: NonNullable<typeof data>['recentBudgets'][number]) => (
        <span className={cn('rounded-full px-2 py-1 text-xs font-semibold capitalize', statusClasses(item.status))}>
          {item.status.replace('-', ' ')}
        </span>
      ),
    },
    {
      key: 'updatedAt',
      header: 'Updated',
      render: (item: NonNullable<typeof data>['recentBudgets'][number]) => formatDate(item.updatedAt),
    },
  ]

  if (isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-24 w-full" />
        <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
          {Array.from({ length: 4 }).map((_, idx) => (
            <Skeleton key={idx} className="h-36 w-full" />
          ))}
        </div>
        <Skeleton className="h-72 w-full" />
      </div>
    )
  }

  if (isError || !data) {
    return (
      <EmptyState
        icon={AlertTriangle}
        title="Dashboard is unavailable"
        description="We could not load dashboard data. Retry after checking the API connection."
      />
    )
  }

  return (
    <div className="space-y-8">
      <PageHeader
        title="Financial command center"
        description="Track budget health, detect risk early, and keep monthly spending predictable."
        actions={
          <>
            <Button variant="outline" onClick={() => navigate({ to: '/budgets' })}>
              View all budgets
            </Button>
            <AppModal
              title="Create budget"
              description="UI-only scaffold. Replace with your API mutation when backend wiring is ready."
              trigger={
                <Button>
                  <Plus className="h-4 w-4" />
                  New Budget
                </Button>
              }
            >
              <form className="space-y-4">
                <FormField id="budgetName" label="Budget name" hint="Example: Team Operations">
                  <Input id="budgetName" placeholder="Budget name" />
                </FormField>
                <FormField id="budgetLimit" label="Monthly limit">
                  <Input id="budgetLimit" type="number" placeholder="1200" />
                </FormField>
                <Button type="button" className="w-full">
                  Save draft
                </Button>
              </form>
            </AppModal>
          </>
        }
      />

      <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        {data.kpis.map((kpi, index) => {
          const Icon = statIcons[index] ?? Activity
          return (
            <StatsCard
              key={kpi.title}
              title={kpi.title}
              value={kpi.value}
              trendLabel={kpi.trendLabel}
              trendDelta={kpi.trendDelta}
              icon={<Icon className="h-4 w-4" />}
            />
          )
        })}
      </section>

      <section className="grid gap-6 xl:grid-cols-[2fr_1fr]">
        <div className="space-y-4">
          <h2 className="text-heading text-foreground">Recent budgets</h2>
          <DataTable
            data={data.recentBudgets}
            columns={columns}
            getRowKey={(row) => row.id}
            ariaLabel="Recent budgets"
            onRowClick={(row) => navigate({ to: '/budgets/$budgetId', params: { budgetId: row.id } })}
          />
        </div>

        <aside className="surface rounded-2xl p-5">
          <h2 className="text-heading text-foreground">Quick actions</h2>
          <ul className="mt-4 space-y-3">
            {data.upcomingActions.map((action) => (
              <li key={action.id} className="rounded-xl border border-border/80 bg-background/80 p-3">
                <p className="text-sm font-medium text-foreground">{action.label}</p>
                <p className="mt-1 text-xs text-muted-foreground">{action.description}</p>
              </li>
            ))}
          </ul>
        </aside>
      </section>
    </div>
  )
}
