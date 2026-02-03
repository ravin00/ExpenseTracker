import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
    component: DashboardPage,
})

function DashboardPage() {
    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
                <p className="text-muted-foreground mt-1">
                    Welcome back! Here's an overview of your expenses.
                </p>
            </div>

            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                <div className="rounded-lg border bg-card p-6">
                    <h3 className="text-sm font-medium text-muted-foreground">Total Budget</h3>
                    <p className="text-2xl font-bold mt-2">$0.00</p>
                </div>
                <div className="rounded-lg border bg-card p-6">
                    <h3 className="text-sm font-medium text-muted-foreground">Total Spent</h3>
                    <p className="text-2xl font-bold mt-2">$0.00</p>
                </div>
                <div className="rounded-lg border bg-card p-6">
                    <h3 className="text-sm font-medium text-muted-foreground">Remaining</h3>
                    <p className="text-2xl font-bold mt-2">$0.00</p>
                </div>
                <div className="rounded-lg border bg-card p-6">
                    <h3 className="text-sm font-medium text-muted-foreground">Active Budgets</h3>
                    <p className="text-2xl font-bold mt-2">0</p>
                </div>
            </div>
        </div>
    )
}
