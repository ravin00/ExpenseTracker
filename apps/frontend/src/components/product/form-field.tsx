import { Label } from '@/components/ui/label'
import { cn } from '@/lib/utils'
import type { ReactNode } from 'react'

type FieldProps = {
  id: string
  label: string
  hint?: string
  error?: string
  children: ReactNode
}

export function FormField({ id, label, hint, error, children }: FieldProps) {
  return (
    <div className="space-y-2">
      <Label htmlFor={id} className="text-sm font-medium text-foreground">
        {label}
      </Label>
      {children}
      {hint ? <p className="text-xs text-muted-foreground">{hint}</p> : null}
      {error ? (
        <p className={cn('text-xs font-medium text-destructive')} role="alert">
          {error}
        </p>
      ) : null}
    </div>
  )
}
