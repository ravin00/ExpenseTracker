import type { BudgetAnalysisData } from '../types'

interface BudgetAnalysisProps {
    budgets: BudgetAnalysisData[]
    isLoading: boolean
}

export function BudgetAnalysis({ budgets, isLoading }: BudgetAnalysisProps) {
    if (isLoading) {
        return <div className="budget-analysis loading">Loading budget analysis...</div>
    }

    if (budgets.length === 0) {
        return <div className="budget-analysis empty">No budget data available</div>
    }

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount)

    const getStatusColor = (utilization: number, isOverBudget: boolean) => {
        if (isOverBudget) return 'danger'
        if (utilization >= 90) return 'warning'
        if (utilization >= 70) return 'caution'
        return 'safe'
    }

    return (
        <div className="budget-analysis">
            <h2>Budget Analysis</h2>
            
            <div className="budget-list">
                {budgets.map((budget) => {
                    const status = getStatusColor(budget.utilizationPercentage, budget.isOverBudget)
                    
                    return (
                        <div key={budget.budgetId} className={`budget-item ${status}`}>
                            <div className="budget-header">
                                <h3>{budget.budgetName}</h3>
                                {budget.isOverBudget && (
                                    <span className="over-budget-badge">Over Budget!</span>
                                )}
                            </div>
                            
                            <div className="budget-amounts">
                                <div className="amount-item">
                                    <span className="label">Budget</span>
                                    <span className="value">{formatCurrency(budget.budgetAmount)}</span>
                                </div>
                                <div className="amount-item">
                                    <span className="label">Spent</span>
                                    <span className="value">{formatCurrency(budget.spentAmount)}</span>
                                </div>
                                <div className="amount-item">
                                    <span className="label">Remaining</span>
                                    <span className={`value ${budget.remainingAmount < 0 ? 'negative' : ''}`}>
                                        {formatCurrency(budget.remainingAmount)}
                                    </span>
                                </div>
                            </div>
                            
                            <div className="budget-progress">
                                <div className="progress-bar">
                                    <div 
                                        className={`progress-fill ${status}`}
                                        style={{ width: `${Math.min(budget.utilizationPercentage, 100)}%` }}
                                    />
                                </div>
                                <span className="percentage">{budget.utilizationPercentage.toFixed(1)}%</span>
                            </div>
                        </div>
                    )
                })}
            </div>
        </div>
    )
}