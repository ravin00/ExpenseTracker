import type { AnalyticsPeriod, SpendingTrend } from '../types'

interface TrendChartProps {
    trends: SpendingTrend[]
    period: AnalyticsPeriod
    isLoading: boolean
}

export function TrendChart({ trends, period, isLoading }: TrendChartProps) {
    if (isLoading) {
        return <div className="trend-chart loading">Loading trends...</div>
    }

    if (!trends || trends.length === 0) {
        return <div className="trend-chart empty">No trend data available for {period} view</div>
    }

    return (
        <div className="trend-chart">
            <h3>{period} Trend Analysis</h3>
            <div className="trend-list">
                {trends.map((trend, index) => (
                    <div key={index} className="trend-item">
                        <span className="trend-date">
                            {new Date(trend.date).toLocaleDateString()}
                        </span>
                        <span className="trend-amount">
                            ${trend.amount.toFixed(2)}
                        </span>
                    </div>
                ))}
            </div>
        </div>
    )
}
