import { createRootRoute, Link, Outlet, useRouterState } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { LayoutDashboard, Target, Wallet } from 'lucide-react'

export const Route = createRootRoute({
    component: RootLayout,
})

function RootLayout() {
    const { location } = useRouterState()
    const isLandingPage = location.pathname === '/'

    // Landing page manages its own layout (Navbar, Footer, etc.)
    if (isLandingPage) {
        return (
            <>
                <Outlet />
                <TanStackRouterDevtools />
            </>
        )
    }

    return (
        <>
            <div className="min-h-screen bg-background">
                <nav className="border-b">
                    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                        <div className="flex justify-between h-16">
                            <div className="flex items-center space-x-8">
                                <Link to="/" className="flex items-center space-x-2">
                                    <Wallet className="h-6 w-6 text-primary" />
                                    <span className="text-xl font-bold">Expense Tracker</span>
                                </Link>

                                <div className="flex space-x-4">
                                    <Link
                                        to="/"
                                        className="inline-flex items-center gap-2 px-3 py-2 text-sm font-medium rounded-md transition-colors"
                                        activeProps={{ className: 'bg-secondary text-secondary-foreground' }}
                                        inactiveProps={{ className: 'text-muted-foreground hover:bg-secondary/50' }}
                                    >
                                        <LayoutDashboard className="h-4 w-4" />
                                        Dashboard
                                    </Link>
                                    <Link
                                        to="/budgets"
                                        className="inline-flex items-center gap-2 px-3 py-2 text-sm font-medium rounded-md transition-colors"
                                        activeProps={{ className: 'bg-secondary text-secondary-foreground' }}
                                        inactiveProps={{ className: 'text-muted-foreground hover:bg-secondary/50' }}
                                    >
                                        <Target className="h-4 w-4" />
                                        Budgets
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </nav>

                <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
                    <Outlet />
                </main>
            </div>
            <TanStackRouterDevtools />
        </>
    )
}
