// Progress Bar Component
interface ProgressBarProps {
    value: number
    max?: number
    showLabel?: boolean
    size?: 'sm' | 'md' | 'lg'
    variant?: 'default' | 'success' | 'warning' | 'danger'
    className?: string
}

const sizeClasses = {
    sm: 'h-1.5',
    md: 'h-2.5',
    lg: 'h-4',
}

const variantClasses = {
    default: 'bg-blue-600',
    success: 'bg-emerald-500',
    warning: 'bg-amber-500',
    danger: 'bg-red-500',
}

export function ProgressBar({
    value,
    max = 100,
    showLabel = false,
    size = 'md',
    variant = 'default',
    className = '',
}: ProgressBarProps) {
    const percentage = Math.min(100, Math.max(0, (value / max) * 100))

    // Auto-determine variant based on percentage
    const autoVariant = percentage >= 100
        ? 'danger'
        : percentage >= 80
            ? 'warning'
            : variant

    return (
        <div className={`w-full ${className}`}>
            <div className={`w-full bg-gray-200 dark:bg-gray-700 rounded-full overflow-hidden ${sizeClasses[size]}`}>
                <div
                    className={`${sizeClasses[size]} rounded-full transition-all duration-500 ease-out ${variantClasses[autoVariant]}`}
                    style={{ width: `${percentage}%` }}
                />
            </div>
            {showLabel && (
                <div className="flex justify-between mt-1 text-xs text-gray-500 dark:text-gray-400">
                    <span>{Math.round(percentage)}%</span>
                    <span>{value} / {max}</span>
                </div>
            )}
        </div>
    )
}
