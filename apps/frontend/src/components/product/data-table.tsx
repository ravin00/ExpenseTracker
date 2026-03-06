import { cn } from '@/lib/utils'
import type { ReactNode } from 'react'

type Column<T> = {
  key: string
  header: string
  className?: string
  mobileLabel?: string
  render: (row: T) => ReactNode
}

type DataTableProps<T> = {
  data: T[]
  columns: Array<Column<T>>
  getRowKey: (row: T) => string
  ariaLabel: string
  onRowClick?: (row: T) => void
}

export function DataTable<T>({ data, columns, getRowKey, ariaLabel, onRowClick }: DataTableProps<T>) {
  return (
    <div className="surface rounded-2xl">
      <div className="hidden overflow-x-auto md:block">
        <table className="w-full min-w-[640px] text-left" aria-label={ariaLabel}>
          <thead className="border-b border-border/80 text-xs uppercase tracking-wide text-muted-foreground">
            <tr>
              {columns.map((column) => (
                <th key={column.key} className={cn('px-4 py-3 font-medium', column.className)}>
                  {column.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data.map((row) => (
              <tr
                key={getRowKey(row)}
                className={cn(
                  'border-b border-border/60 text-sm text-foreground transition-colors last:border-b-0',
                  onRowClick ? 'cursor-pointer hover:bg-accent/30 focus-within:bg-accent/20' : undefined,
                )}
                onClick={onRowClick ? () => onRowClick(row) : undefined}
              >
                {columns.map((column) => (
                  <td key={column.key} className={cn('px-4 py-4 align-middle', column.className)}>
                    {column.render(row)}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <ul className="grid gap-3 p-3 md:hidden" aria-label={ariaLabel}>
        {data.map((row) => (
          <li
            key={getRowKey(row)}
            className={cn(
              'rounded-xl border border-border/70 bg-background/80 p-4',
              onRowClick ? 'cursor-pointer hover:border-primary/40' : undefined,
            )}
            onClick={onRowClick ? () => onRowClick(row) : undefined}
          >
            <dl className="space-y-2">
              {columns.map((column) => (
                <div key={column.key} className="grid grid-cols-[110px_1fr] gap-3 text-sm">
                  <dt className="text-muted-foreground">{column.mobileLabel ?? column.header}</dt>
                  <dd className="font-medium text-foreground">{column.render(row)}</dd>
                </div>
              ))}
            </dl>
          </li>
        ))}
      </ul>
    </div>
  )
}
