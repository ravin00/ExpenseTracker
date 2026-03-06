import { cn } from '@/lib/utils'
import { ArrowDownRight, ArrowUpRight, Minus } from 'lucide-react'
import type { ReactNode } from 'react'

type StatsCardProps = {
  title: string
  value: string
  trendLabel: string
  trendDelta: number
  icon: ReactNode
}

export function StatsCard({ title, value, trendLabel, trendDelta, icon }: StatsCardProps) {
  const trendTone = trendDelta > 0 ? 'text-emerald-600 dark:text-emerald-400' : trendDelta < 0 ? 'text-amber-600 dark:text-amber-400' : 'text-muted-foreground'

  return (
    <article className="surface-elevated rounded-2xl p-5">
      <div className="flex items-center justify-between">
        <p className="text-sm font-medium text-muted-foreground">{title}</p>
        <span className="rounded-lg border border-border/80 bg-background/70 p-2 text-muted-foreground">{icon}</span>
      </div>
      <p className="mt-4 text-3xl font-semibold tracking-tight text-foreground">{value}</p>
      <div className={cn('mt-3 flex items-center gap-2 text-xs font-medium', trendTone)}>
        {trendDelta > 0 ? <ArrowUpRight className="h-3.5 w-3.5" /> : null}
        {trendDelta < 0 ? <ArrowDownRight className="h-3.5 w-3.5" /> : null}
        {trendDelta === 0 ? <Minus className="h-3.5 w-3.5" /> : null}
        <span>{Math.abs(trendDelta)}%</span>
        <span className="text-muted-foreground">{trendLabel}</span>
      </div>
    </article>
  )
}
