import type { SpendingTrend } from '../types'

interface SpendingChartProps {
    trends: SpendingTrend[]
    isLoading: boolean
}

export function SpendingChart({ trends, isLoading }: SpendingChartProps) {
    if (isLoading) {
        return <div className="spending-chart loading">Loading chart...</div>
    }

    if (trends.length === 0) {
        return <div className="spending-chart empty">No trend data available</div>
    }

    const maxAmount = Math.max(...trends.map(t => t.amount))
    
    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount)

    const formatDate = (dateStr: string) => {
        const date = new Date(dateStr)
        return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })
    }

    return (
        <div className="spending-chart">
            <h2>Spending Trends</h2>
            
            <div className="chart-container">
                <div className="chart-bars">
                    {trends.map((trend, index) => (
                        <div key={index} className="bar-wrapper">
                            <div 
                                className="bar"
                                style={{ height: `${(trend.amount / maxAmount) * 100}%` }}
                                title={formatCurrency(trend.amount)}
                            >
                                <span className="bar-value">{formatCurrency(trend.amount)}</span>
                            </div>
                            <span className="bar-label">{formatDate(trend.date)}</span>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}