import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/product/app-tabs'
import { ConfirmDialog, EmptyState, PageHeader } from '@/components/product'
import { useBudgetDetailQuery } from '@/features/product/data/api'
import { asPercent, formatCurrency, formatDate } from '@/features/product/hooks/use-formatters'
import { cn } from '@/lib/utils'
import { useNavigate } from '@tanstack/react-router'
import { AlertTriangle, ArrowLeft, Clock3, ShieldAlert } from 'lucide-react'
import { useMemo, useState } from 'react'

function statusClasses(status: 'on-track' | 'warning' | 'over') {
  if (status === 'on-track') return 'bg-emerald-500/10 text-emerald-700 dark:text-emerald-300'
  if (status === 'warning') return 'bg-amber-500/10 text-amber-700 dark:text-amber-300'
  return 'bg-rose-500/10 text-rose-700 dark:text-rose-300'
}

function progressPercent(spent: number, limit: number): number {
  if (limit <= 0) return 0
  return Math.min((spent / limit) * 100, 100)
}

type BudgetDetailsPageProps = {
  budgetId: string
}

export function BudgetDetailsPage({ budgetId }: BudgetDetailsPageProps) {
  const navigate = useNavigate()
  const { data, isLoading, isError } = useBudgetDetailQuery(budgetId)
  const [showArchiveDialog, setShowArchiveDialog] = useState(false)

  const usage = useMemo(() => {
    if (!data) return 0
    return progressPercent(data.spent, data.limit)
  }, [data])

  if (isLoading) {
    return (
      <div className="space-y-4">
        <Skeleton className="h-24 w-full" />
        <Skeleton className="h-52 w-full" />
        <Skeleton className="h-72 w-full" />
      </div>
    )
  }

  if (isError || !data) {
    return (
      <EmptyState
        icon={AlertTriangle}
        title="Budget not found"
        description="This budget could not be loaded. It may have been removed or renamed."
        actionLabel="Back to Budgets"
        onAction={() => navigate({ to: '/budgets' })}
      />
    )
  }

  return (
    <div className="space-y-6">
      <PageHeader
        title={data.name}
        description={`Managed by ${data.owner} in ${data.category}`}
        actions={
          <>
            <Button variant="outline" onClick={() => navigate({ to: '/budgets' })}>
              <ArrowLeft className="h-4 w-4" />
              Back
            </Button>
            <Button variant="destructive" onClick={() => setShowArchiveDialog(true)}>
              Archive
            </Button>
          </>
        }
      />

      <section className="grid gap-4 lg:grid-cols-3">
        <article className="surface rounded-2xl p-5 lg:col-span-2">
          <div className="flex items-center justify-between">
            <h2 className="text-heading text-foreground">Overview</h2>
            <span className={cn('rounded-full px-2 py-1 text-xs font-semibold capitalize', statusClasses(data.status))}>
              {data.status.replace('-', ' ')}
            </span>
          </div>

          <div className="mt-5 grid gap-4 sm:grid-cols-3">
            <div>
              <p className="text-xs text-muted-foreground">Spent</p>
              <p className="mt-1 text-xl font-semibold">{formatCurrency(data.spent)}</p>
            </div>
            <div>
              <p className="text-xs text-muted-foreground">Limit</p>
              <p className="mt-1 text-xl font-semibold">{formatCurrency(data.limit)}</p>
            </div>
            <div>
              <p className="text-xs text-muted-foreground">Remaining</p>
              <p className="mt-1 text-xl font-semibold">{formatCurrency(data.remaining)}</p>
            </div>
          </div>

          <div className="mt-5 space-y-2">
            <div className="flex items-center justify-between text-xs text-muted-foreground">
              <span>Usage</span>
              <span>{asPercent(usage)}</span>
            </div>
            <div className="h-2 rounded-full bg-muted">
              <div className="h-2 rounded-full bg-primary" style={{ width: `${usage}%` }} />
            </div>
          </div>
        </article>

        <aside className="surface rounded-2xl p-5">
          <h2 className="text-heading text-foreground">Policy</h2>
          <dl className="mt-4 space-y-3 text-sm">
            <div className="flex justify-between gap-4">
              <dt className="text-muted-foreground">Period</dt>
              <dd className="font-medium capitalize">{data.period}</dd>
            </div>
            <div className="flex justify-between gap-4">
              <dt className="text-muted-foreground">Last Updated</dt>
              <dd className="font-medium">{formatDate(data.updatedAt)}</dd>
            </div>
            <div className="rounded-xl border border-border/80 bg-background/80 p-3 text-xs text-muted-foreground">
              {data.notes}
            </div>
          </dl>
        </aside>
      </section>

      <section className="surface rounded-2xl p-5">
        <Tabs defaultValue="activity" className="space-y-4">
          <TabsList>
            <TabsTrigger value="activity">Activity Log</TabsTrigger>
            <TabsTrigger value="audit">Audit Notes</TabsTrigger>
          </TabsList>

          <TabsContent value="activity">
            <ul className="space-y-3">
              {data.activity.map((item) => (
                <li key={item.id} className="rounded-xl border border-border/80 bg-background/80 p-3">
                  <div className="flex items-start gap-2">
                    <Clock3 className="mt-0.5 h-4 w-4 text-muted-foreground" />
                    <div>
                      <p className="text-sm font-medium text-foreground">{item.message}</p>
                      <p className="text-xs text-muted-foreground">
                        {item.actor} • {formatDate(item.createdAt)}
                      </p>
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          </TabsContent>

          <TabsContent value="audit">
            <div className="rounded-xl border border-amber-500/30 bg-amber-500/10 p-4 text-sm text-amber-800 dark:text-amber-200">
              <div className="flex items-start gap-2">
                <ShieldAlert className="mt-0.5 h-4 w-4" />
                <p>
                  Keep immutable events in backend audit storage. This UI section is ready to attach to API logs when your endpoint is available.
                </p>
              </div>
            </div>
          </TabsContent>
        </Tabs>
      </section>

      <ConfirmDialog
        open={showArchiveDialog}
        onOpenChange={setShowArchiveDialog}
        title="Archive budget"
        description="Archived budgets are hidden from active workflows but kept for reporting."
        destructive
        confirmLabel="Archive"
        onConfirm={() => {
          navigate({ to: '/budgets' })
        }}
      />
    </div>
  )
}
