import { useBudgets } from '@/hooks/use-budgets'
import { useBudgetStore } from '@/stores/budget-store'
import { createFileRoute } from '@tanstack/react-router'
import { Plus, Search } from 'lucide-react'

export const Route = createFileRoute('/budgets/')({
    component: BudgetsPage,
})

function BudgetsPage() {
    const { budgets, isLoading, error } = useBudgets()
    const { filters, setFilters } = useBudgetStore()

    if (isLoading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
            </div>
        )
    }

    if (error) {
        return (
            <div className="text-center py-12">
                <p className="text-destructive">Error loading budgets. Please try again.</p>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Budgets</h1>
                    <p className="text-muted-foreground mt-1">
                        Manage your spending limits and track progress
                    </p>
                </div>
                <button className="inline-flex items-center gap-2 px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90">
                    <Plus className="h-4 w-4" />
                    Create Budget
                </button>
            </div>

            {/* Search */}
            <div className="flex gap-4">
                <div className="relative flex-1">
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                    <input
                        type="text"
                        placeholder="Search budgets..."
                        value={filters.searchQuery}
                        onChange={(e) => setFilters({ searchQuery: e.target.value })}
                        className="w-full pl-9 pr-4 py-2 border rounded-md bg-background"
                    />
                </div>
            </div>

            {budgets.length === 0 ? (
                <div className="text-center py-12 border rounded-lg bg-card">
                    <p className="text-muted-foreground mb-4">
                        No budgets found. Create your first budget to get started.
                    </p>
                    <button className="inline-flex items-center gap-2 px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90">
                        <Plus className="h-4 w-4" />
                        Create Budget
                    </button>
                </div>
            ) : (
                <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                    {budgets.map((budget) => (
                        <div key={budget.id} className="border rounded-lg bg-card p-6 hover:shadow-lg transition-shadow">
                            <h3 className="text-lg font-semibold">{budget.displayName}</h3>
                            {budget.description && (
                                <p className="text-sm text-muted-foreground mt-1">{budget.description}</p>
                            )}
                            <div className="mt-4 space-y-2">
                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">Progress</span>
                                    <span className="font-medium">{budget.progressPercentage.toFixed(1)}%</span>
                                </div>
                                <div className="h-2 bg-secondary rounded-full overflow-hidden">
                                    <div
                                        className="h-full bg-primary transition-all"
                                        style={{ width: `${Math.min(budget.progressPercentage, 100)}%` }}
                                    />
                                </div>
                                <div className="flex justify-between pt-2">
                                    <div>
                                        <p className="text-sm text-muted-foreground">Spent</p>
                                        <p className="font-bold">${budget.spentAmount.toFixed(2)}</p>
                                    </div>
                                    <div className="text-right">
                                        <p className="text-sm text-muted-foreground">Budget</p>
                                        <p className="font-bold">${budget.amount.toFixed(2)}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    )
}
