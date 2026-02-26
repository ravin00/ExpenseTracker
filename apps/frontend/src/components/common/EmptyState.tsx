// Empty State Component
import type { ReactNode } from 'react'

interface EmptyStateProps {
    icon?: ReactNode
    title: string
    description?: string
    action?: ReactNode
}

export function EmptyState({ icon, title, description, action }: EmptyStateProps) {
    return (
        <div className="flex flex-col items-center justify-center p-12 text-center">
            {icon && <div className="text-gray-400 mb-4">{icon}</div>}
            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">{title}</h3>
            {description && (
                <p className="text-gray-500 dark:text-gray-400 mb-6 max-w-sm">{description}</p>
            )}
            {action}
        </div>
    )
}
