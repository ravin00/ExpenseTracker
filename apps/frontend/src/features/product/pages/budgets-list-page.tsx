import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Skeleton } from '@/components/ui/skeleton'
import { DataTable, EmptyState, PageHeader } from '@/components/product'
import { useBudgetListQuery } from '@/features/product/data/api'
import { formatCurrency, formatDate } from '@/features/product/hooks/use-formatters'
import { cn } from '@/lib/utils'
import { useNavigate } from '@tanstack/react-router'
import { AlertTriangle, Filter, FolderSearch, Plus } from 'lucide-react'
import { useState } from 'react'

type BudgetStatusFilter = 'all' | 'on-track' | 'warning' | 'over'
type BudgetSort = 'name' | 'spent' | 'remaining' | 'updatedAt'

function statusClasses(status: 'on-track' | 'warning' | 'over') {
  if (status === 'on-track') return 'bg-emerald-500/10 text-emerald-700 dark:text-emerald-300'
  if (status === 'warning') return 'bg-amber-500/10 text-amber-700 dark:text-amber-300'
  return 'bg-rose-500/10 text-rose-700 dark:text-rose-300'
}

export function BudgetsListPage() {
  const navigate = useNavigate()
  const [search, setSearch] = useState('')
  const [statusFilter, setStatusFilter] = useState<BudgetStatusFilter>('all')
  const [sortBy, setSortBy] = useState<BudgetSort>('updatedAt')

  const { data, isLoading, isError } = useBudgetListQuery(search, statusFilter, sortBy)

  const columns = [
    {
      key: 'name',
      header: 'Budget',
      render: (item: NonNullable<typeof data>[number]) => (
        <div>
          <p className="font-medium">{item.name}</p>
          <p className="text-xs text-muted-foreground">{item.category}</p>
        </div>
      ),
    },
    {
      key: 'owner',
      header: 'Owner',
      render: (item: NonNullable<typeof data>[number]) => item.owner,
    },
    {
      key: 'spent',
      header: 'Spent',
      render: (item: NonNullable<typeof data>[number]) => formatCurrency(item.spent),
    },
    {
      key: 'remaining',
      header: 'Remaining',
      render: (item: NonNullable<typeof data>[number]) => formatCurrency(item.remaining),
    },
    {
      key: 'status',
      header: 'Status',
      render: (item: NonNullable<typeof data>[number]) => (
        <span className={cn('rounded-full px-2 py-1 text-xs font-semibold capitalize', statusClasses(item.status))}>
          {item.status.replace('-', ' ')}
        </span>
      ),
    },
    {
      key: 'updatedAt',
      header: 'Updated',
      render: (item: NonNullable<typeof data>[number]) => formatDate(item.updatedAt),
    },
  ]

  return (
    <div className="space-y-6">
      <PageHeader
        title="Budgets"
        description="Filter and manage all active allocations from one place."
        actions={
          <Button>
            <Plus className="h-4 w-4" />
            Add Budget
          </Button>
        }
      />

      <section className="surface rounded-2xl p-4 sm:p-5">
        <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-[1fr_180px_180px_auto]">
          <label className="space-y-2">
            <span className="text-xs font-medium text-muted-foreground">Search</span>
            <Input
              value={search}
              onChange={(event) => setSearch(event.target.value)}
              placeholder="Search name, category, owner"
            />
          </label>

          <label className="space-y-2">
            <span className="text-xs font-medium text-muted-foreground">Status</span>
            <select
              value={statusFilter}
              onChange={(event) => setStatusFilter(event.target.value as BudgetStatusFilter)}
              className="focus-ring h-9 w-full rounded-md border border-input bg-background px-3 text-sm"
              aria-label="Filter budgets by status"
            >
              <option value="all">All</option>
              <option value="on-track">On Track</option>
              <option value="warning">Warning</option>
              <option value="over">Over</option>
            </select>
          </label>

          <label className="space-y-2">
            <span className="text-xs font-medium text-muted-foreground">Sort by</span>
            <select
              value={sortBy}
              onChange={(event) => setSortBy(event.target.value as BudgetSort)}
              className="focus-ring h-9 w-full rounded-md border border-input bg-background px-3 text-sm"
              aria-label="Sort budgets"
            >
              <option value="updatedAt">Recently updated</option>
              <option value="name">Name</option>
              <option value="spent">Spent (high to low)</option>
              <option value="remaining">Remaining (high to low)</option>
            </select>
          </label>

          <Button variant="outline" className="self-end">
            <Filter className="h-4 w-4" />
            Save View
          </Button>
        </div>
      </section>

      {isLoading ? (
        <div className="space-y-3">
          <Skeleton className="h-14 w-full" />
          <Skeleton className="h-14 w-full" />
          <Skeleton className="h-14 w-full" />
        </div>
      ) : null}

      {isError ? (
        <EmptyState
          icon={AlertTriangle}
          title="Budgets unavailable"
          description="We could not load your budget list. Verify API connectivity and retry."
        />
      ) : null}

      {!isLoading && !isError && data && data.length === 0 ? (
        <EmptyState
          icon={FolderSearch}
          title="No budgets match this filter"
          description="Try removing filters or create a new budget to populate this view."
          actionLabel="Create Budget"
        />
      ) : null}

      {!isLoading && !isError && data && data.length > 0 ? (
        <DataTable
          data={data}
          columns={columns}
          getRowKey={(row) => row.id}
          ariaLabel="Budgets"
          onRowClick={(row) => navigate({ to: '/budgets/$budgetId', params: { budgetId: row.id } })}
        />
      ) : null}
    </div>
  )
}
