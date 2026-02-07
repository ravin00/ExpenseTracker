import { useState } from 'react'
import { toast } from 'sonner'
import '../analytics.css';
import { useExpensesByCategory, useFinancialSummary, useGenerateAnalytics } from '../hooks/useAnalytics'
import { useSpendingTrends } from '../hooks/useSpendingTrends'
import type { AnalyticsPeriod } from '../types'
import { BudgetAnalysis } from './BudgetAnalysis'
import { CategoryBreakdown } from './CategoryBreakdown'
import { DateRangePicker } from './DateRangePicker'
import { FinancialSummary } from './FinancialSummary'
import { SpendingChart } from './SpendingChart'
import { TrendChart } from './TrendChart'

export function AnalyticsDashboard() {
    const [dateRange, setDateRange] = useState<{ start?: string; end?: string }>({})
    const [trendPeriod, setTrendPeriod] = useState<AnalyticsPeriod>('Monthly')

    const { mutate: generateAnalytics, isPending: isGenerating } = useGenerateAnalytics()

    const handleGenerateData = () => {
        generateAnalytics('Monthly', {
            onSuccess: () => toast.success('Analytics data generated successfully'),
            onError: () => toast.error('Failed to generate analytics data')
        })
    }

    const { summary, isLoading: summaryLoading } = useFinancialSummary(
        dateRange.start,
        dateRange.end
    )
    const { categories, isLoading: categoriesLoading } = useExpensesByCategory(
        dateRange.start,
        dateRange.end
    )
    const { trends, isLoading: trendsLoading } = useSpendingTrends(trendPeriod)

    console.log('Dashboard Data:', {
        summary, summaryLoading,
        categories, categoriesLoading,
        trends, trendsLoading,
        dateRange
    })

    const handleDateRangeChange = (startDate: string, endDate: string) => {
        setDateRange({ start: startDate, end: endDate })
    }

    const periods: AnalyticsPeriod[] = ['Daily', 'Weekly', 'Monthly', 'Quarterly', 'Yearly']

    return (
        <div className="analytics-dashboard">
            <header className="dashboard-header">
                <h1>Analytics Dashboard</h1>
                <div className="header-actions">
                    <DateRangePicker onRangeChange={handleDateRangeChange} />
                    <button
                        className="btn-primary"
                        onClick={handleGenerateData}
                        disabled={isGenerating}
                        style={{ marginLeft: '1rem', padding: '0.5rem 1rem', borderRadius: '0.375rem', backgroundColor: '#2563eb', color: 'white', border: 'none', cursor: isGenerating ? 'not-allowed' : 'pointer' }}
                    >
                        {isGenerating ? 'Generating...' : 'Generate Data'}
                    </button>
                </div>
            </header>

            <section className="summary-section">
                <FinancialSummary data={summary} isLoading={summaryLoading} />
            </section>

            <section className="charts-section">
                <div className="chart-controls">
                    <label>Trend Period:</label>
                    <select
                        value={trendPeriod}
                        onChange={(e) => setTrendPeriod(e.target.value as AnalyticsPeriod)}
                    >
                        {periods.map(p => (
                            <option key={p} value={p}>{p}</option>
                        ))}
                    </select>
                </div>

                <div className="charts-grid">
                    <SpendingChart trends={trends} isLoading={trendsLoading} />
                    <TrendChart trends={trends} period={trendPeriod} isLoading={trendsLoading} />
                </div>
            </section>

            <section className="breakdown-section">
                <div className="breakdown-grid">
                    <CategoryBreakdown categories={categories} isLoading={categoriesLoading} />
                    <BudgetAnalysis
                        budgets={summary?.budgetAnalyses || []}
                        isLoading={summaryLoading}
                    />
                </div>
            </section>
        </div>
    )
}