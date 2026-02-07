import type { FinancialSummaryData } from '../types'

interface FinancialSummaryProps {
    data: FinancialSummaryData | undefined
    isLoading: boolean
}

export function FinancialSummary({ data, isLoading }: FinancialSummaryProps) {
    if (isLoading) {
        return <div className="financial-summary loading">Loading summary...</div>
    }

    if (!data) {
        return <div className="financial-summary empty">No data available</div>
    }

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount)

    const getHealthColor = (health: string) => {
        switch (health.toLowerCase()) {
            case 'excellent': return 'text-green-600'
            case 'good': return 'text-blue-600'
            case 'fair': return 'text-yellow-600'
            default: return 'text-red-600'
        }
    }

    return (
        <div className="financial-summary">
            <h2>Financial Summary</h2>

            <div className="summary-grid">
                <div className="summary-card income">
                    <span className="label">Total Income</span>
                    <span className="value">{formatCurrency(data.totalIncome)}</span>
                </div>

                <div className="summary-card expenses">
                    <span className="label">Total Expenses</span>
                    <span className="value">{formatCurrency(data.totalExpenses)}</span>
                </div>

                <div className="summary-card savings">
                    <span className="label">Total Savings</span>
                    <span className="value">{formatCurrency(data.totalSavings)}</span>
                </div>

                <div className="summary-card net-worth">
                    <span className="label">Net Worth</span>
                    <span className="value">{formatCurrency(data.netWorth)}</span>
                </div>
            </div>

            <div className="health-indicator">
                <span>Financial Health: </span>
                <span className={getHealthColor(data.financialHealth)}>
                    {data.financialHealth}
                </span>
                <span className="savings-rate">
                    (Savings Rate: {data.savingsRate.toFixed(1)}%)
                </span>
            </div>
        </div>
    )
}