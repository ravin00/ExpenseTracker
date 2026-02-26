import { LogoutConfirmDialog } from '@/components/LogoutConfirmDialog'
import { ModeToggle } from '@/components/mode-toggle'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { useAuthStore } from '@/stores/auth-store'
import { Link, Outlet, createRootRoute, useNavigate, useRouterState } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { BarChart3, LayoutDashboard, Target, Wallet } from 'lucide-react'
import { useEffect, useState } from 'react'
import { toast } from 'sonner'

export const Route = createRootRoute({
    component: RootLayout,
})

function RootLayout() {
    const { location } = useRouterState()
    const checkTokenValidity = useAuthStore((state) => state.checkTokenValidity)
    const isLandingPage = location.pathname === '/'

    // Check token validity on mount and route changes
    useEffect(() => {
        checkTokenValidity()
    }, [location.pathname, checkTokenValidity])

    // Set up periodic check (every 5 minutes)
    useEffect(() => {
        const interval = setInterval(() => {
            checkTokenValidity()
        }, 5 * 60 * 1000) // 5 minutes
        return () => clearInterval(interval)
    }, [checkTokenValidity])

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
                                    <Link
                                        to="/analytics"
                                        className="inline-flex items-center gap-2 px-3 py-2 text-sm font-medium rounded-md transition-colors"
                                        activeProps={{ className: 'bg-secondary text-secondary-foreground' }}
                                        inactiveProps={{ className: 'text-muted-foreground hover:bg-secondary/50' }}
                                    >
                                        <BarChart3 className="h-4 w-4" />
                                        Analytics
                                    </Link>
                                </div>
                            </div>
                            <div className="flex items-center space-x-4">
                                <ModeToggle />
                                <AuthButtons />
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

function AuthButtons() {
    const { isAuthenticated, user, logout } = useAuth()
    const navigate = useNavigate()
    const [showDropdown, setShowDropdown] = useState(false)
    const [showLogoutDialog, setShowLogoutDialog] = useState(false)

    const handleLogout = () => {
        logout()
        toast.success('Logged out successfully', {
            description: 'You have been signed out of your account',
        })
        navigate({ to: '/login' })
    }

    if (!isAuthenticated) {
        return (
            <div className="flex gap-2">
                <Link to="/login">
                    <button className="px-4 py-2 text-sm font-medium text-gray-700 hover:text-gray-900">
                        Sign in
                    </button>
                </Link>
            </div>
        )
    }

    return (
        <>
            <div className="relative">
                <button
                    onClick={() => setShowDropdown(!showDropdown)}
                    className="flex items-center gap-2 px-3 py-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                >
                    <div className="w-8 h-8 rounded-full bg-gradient-to-br from-blue-500 to-purple-600 flex items-center justify-center">
                        <span className="text-white text-sm font-semibold">
                            {user?.username?.[0]?.toUpperCase() || 'U'}
                        </span>
                    </div>
                    <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                        {user?.username || 'User'}
                    </span>
                </button>

                {showDropdown && (
                    <>
                        <div
                            className="fixed inset-0 z-10"
                            onClick={() => setShowDropdown(false)}
                        />
                        <div className="absolute right-0 mt-2 w-56 bg-white dark:bg-gray-800 rounded-lg shadow-xl border border-gray-200 dark:border-gray-700 py-2 z-20">
                            <div className="px-4 py-3 border-b border-gray-200 dark:border-gray-700">
                                <p className="text-sm font-semibold text-gray-900 dark:text-white">{user?.username}</p>
                                <p className="text-xs text-gray-500 dark:text-gray-400 truncate">{user?.email}</p>
                            </div>
                            <Link
                                to="/profile"
                                onClick={() => setShowDropdown(false)}
                                className="flex items-center gap-3 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                            >
                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                </svg>
                                View Profile
                            </Link>
                            <button
                                onClick={() => {
                                    setShowDropdown(false)
                                    setShowLogoutDialog(true)
                                }}
                                className="flex items-center gap-3 w-full px-4 py-2 text-sm text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                            >
                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                                </svg>
                                Log out
                            </button>
                        </div>
                    </>
                )}
            </div>

            <LogoutConfirmDialog
                open={showLogoutDialog}
                onOpenChange={setShowLogoutDialog}
                onConfirm={handleLogout}
            />
        </>
    )
}
