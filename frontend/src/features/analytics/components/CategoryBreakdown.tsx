import type { ExpenseByCategory } from '../types'

interface CategoryBreakdownProps {
    categories: ExpenseByCategory[]
    isLoading: boolean
}

export function CategoryBreakdown({ categories, isLoading }: CategoryBreakdownProps) {
    if (isLoading) {
        return <div className="category-breakdown loading">Loading categories...</div>
    }

    if (categories.length === 0) {
        return <div className="category-breakdown empty">No expense data available</div>
    }

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount)

    // Generate colors for categories
    const colors = [
        '#3b82f6', '#ef4444', '#22c55e', '#f59e0b',
        '#8b5cf6', '#ec4899', '#06b6d4', '#84cc16'
    ]

    return (
        <div className="category-breakdown">
            <h2>Expenses by Category</h2>

            <div className="category-list">
                {categories.map((category, index) => (
                    <div key={category.categoryName} className="category-item">
                        <div className="category-info">
                            <span
                                className="category-color"
                                style={{ backgroundColor: colors[index % colors.length] }}
                            />
                            <span className="category-name">{category.categoryName}</span>
                        </div>

                        <div className="category-stats">
                            <span className="amount">{formatCurrency(category.amount)}</span>
                            <span className="percentage">({category.percentage.toFixed(1)}%)</span>
                        </div>

                        <div className="progress-bar">
                            <div
                                className="progress-fill"
                                style={{
                                    width: `${category.percentage}%`,
                                    backgroundColor: colors[index % colors.length]
                                }}
                            />
                        </div>

                        <span className="transaction-count">
                            {category.transactionCount} transactions
                        </span>
                    </div>
                ))}
            </div>
        </div>
    )
}