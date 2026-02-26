// Currency Display Component
interface CurrencyDisplayProps {
    amount: number
    currency?: string
    showSign?: boolean
    className?: string
}

export function CurrencyDisplay({
    amount,
    currency = 'USD',
    showSign = false,
    className = ''
}: CurrencyDisplayProps) {
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency,
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    })

    const isNegative = amount < 0
    const isPositive = amount > 0

    const colorClass = showSign
        ? isNegative
            ? 'text-red-600 dark:text-red-400'
            : isPositive
                ? 'text-emerald-600 dark:text-emerald-400'
                : ''
        : ''

    const prefix = showSign && isPositive ? '+' : ''

    return (
        <span className={`${colorClass} ${className}`}>
            {prefix}{formatter.format(amount)}
        </span>
    )
}
