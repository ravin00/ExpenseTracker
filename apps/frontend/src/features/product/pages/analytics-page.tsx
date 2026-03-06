import { Skeleton } from '@/components/ui/skeleton'
import { EmptyState, PageHeader } from '@/components/product'
import { useAnalyticsQuery } from '@/features/product/data/api'
import { formatCurrency } from '@/features/product/hooks/use-formatters'
import { AlertTriangle } from 'lucide-react'

export function AnalyticsPage() {
  const { data, isLoading, isError } = useAnalyticsQuery()

  if (isLoading) {
    return (
      <div className="space-y-4">
        <Skeleton className="h-20 w-full" />
        <Skeleton className="h-72 w-full" />
      </div>
    )
  }

  if (isError || !data) {
    return (
      <EmptyState
        icon={AlertTriangle}
        title="Analytics unavailable"
        description="Unable to retrieve analytics right now. Retry after service recovery."
      />
    )
  }

  const maxValue = Math.max(...data.map((point) => point.amount), 1)

  return (
    <div className="space-y-6">
      <PageHeader
        title="Analytics"
        description="Weekly spending trend overview with clean, low-noise visualization."
      />

      <section className="surface rounded-2xl p-5">
        <div className="grid gap-4">
          {data.map((point) => {
            const width = Math.round((point.amount / maxValue) * 100)

            return (
              <article key={point.label} className="space-y-2">
                <div className="flex items-center justify-between text-sm">
                  <span className="font-medium text-foreground">{point.label}</span>
                  <span className="text-muted-foreground">{formatCurrency(point.amount)}</span>
                </div>
                <div className="h-2 rounded-full bg-muted">
                  <div className="h-2 rounded-full bg-primary" style={{ width: `${width}%` }} />
                </div>
              </article>
            )
          })}
        </div>
      </section>
    </div>
  )
}
