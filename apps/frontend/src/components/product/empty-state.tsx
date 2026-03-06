import { Button } from '@/components/ui/button'
import type { LucideIcon } from 'lucide-react'

type EmptyStateProps = {
  title: string
  description: string
  icon: LucideIcon
  actionLabel?: string
  onAction?: () => void
}

export function EmptyState({ title, description, icon: Icon, actionLabel, onAction }: EmptyStateProps) {
  return (
    <div className="surface flex flex-col items-center justify-center rounded-2xl p-8 text-center">
      <span className="mb-4 rounded-full border border-border bg-background p-3 text-muted-foreground">
        <Icon className="h-5 w-5" />
      </span>
      <h3 className="text-lg font-semibold text-foreground">{title}</h3>
      <p className="mt-2 max-w-md text-sm text-muted-foreground">{description}</p>
      {actionLabel && onAction ? (
        <Button onClick={onAction} className="mt-5">
          {actionLabel}
        </Button>
      ) : null}
    </div>
  )
}
