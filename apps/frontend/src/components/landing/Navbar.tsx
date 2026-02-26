import { LogoutConfirmDialog } from '@/components/LogoutConfirmDialog'
import { ModeToggle } from '@/components/mode-toggle'
import { Button } from '@/components/ui/button'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { Link, useNavigate } from '@tanstack/react-router'
import { ChevronRight, Menu, Wallet, X } from 'lucide-react'
import { useEffect, useState } from 'react'
import { toast } from 'sonner'

export function Navbar() {
    const [scrolled, setScrolled] = useState(false)
    const [mobileOpen, setMobileOpen] = useState(false)
    const [showDropdown, setShowDropdown] = useState(false)
    const [showLogoutDialog, setShowLogoutDialog] = useState(false)
    const { isAuthenticated, user, logout } = useAuth()
    const navigate = useNavigate()

    useEffect(() => {
        const onScroll = () => setScrolled(window.scrollY > 20)
        window.addEventListener('scroll', onScroll)
        return () => window.removeEventListener('scroll', onScroll)
    }, [])

    const handleLogout = () => {
        logout()
        toast.success('Logged out successfully', {
            description: 'You have been signed out of your account',
        })
        navigate({ to: '/login' })
    }

    const navLinks = [
        { label: 'Features', href: '#features' },
        { label: 'How It Works', href: '#how-it-works' },
        { label: 'Testimonials', href: '#testimonials' },
        { label: 'Pricing', href: '#pricing' },
    ]

    return (
        <header
            className={`fixed top-0 left-0 right-0 z-50 transition-all duration-500 ${scrolled
                ? 'bg-white/70 dark:bg-gray-950/70 backdrop-blur-2xl shadow-xl shadow-black/5 border-b border-gray-200/50 dark:border-gray-800/50 py-2'
                : 'bg-transparent py-4'
                }`}
        >
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex items-center justify-between h-14">
                    {/* Logo with Premium Glow */}
                    <Link to="/" className="flex items-center gap-3 group">
                        <div className="relative">
                            <div className="absolute inset-0 bg-gradient-to-r from-blue-500 to-violet-500 rounded-xl blur-lg opacity-40 group-hover:opacity-70 transition-all duration-300" />
                            <div className="relative bg-gradient-to-br from-blue-600 via-violet-600 to-fuchsia-600 p-2.5 rounded-xl shadow-lg">
                                <Wallet className="h-5 w-5 text-white" />
                            </div>
                        </div>
                        <div className="flex flex-col">
                            <span className="text-xl font-black bg-gradient-to-r from-gray-900 via-gray-800 to-gray-600 dark:from-white dark:via-gray-200 dark:to-gray-400 bg-clip-text text-transparent">
                                SpendWise
                            </span>
                        </div>
                    </Link>

                    {/* Desktop Nav with Pill Style */}
                    <nav className="hidden md:flex items-center gap-1 bg-gray-100/80 dark:bg-gray-800/80 backdrop-blur-sm rounded-full px-2 py-1.5 border border-gray-200/50 dark:border-gray-700/50">
                        {navLinks.map((link) => (
                            <a
                                key={link.href}
                                href={link.href}
                                className="px-4 py-2 text-sm font-medium text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white rounded-full hover:bg-white dark:hover:bg-gray-700 transition-all duration-200 shadow-none hover:shadow-sm"
                            >
                                {link.label}
                            </a>
                        ))}
                    </nav>

                    {/* Desktop CTA with Premium Buttons */}
                    <div className="hidden md:flex items-center gap-3">
                        <ModeToggle />
                        {isAuthenticated ? (
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
                                                to="/analytics"
                                                onClick={() => setShowDropdown(false)}
                                                className="flex items-center gap-3 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                                            >
                                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
                                                </svg>
                                                Dashboard
                                            </Link>
                                            <Link
                                                to="/profile"
                                                onClick={() => setShowDropdown(false)}
                                                className="flex items-center gap-3 px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                                            >
                                                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                                </svg>
                                                Profile
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
                        ) : (
                            <>
                                <Link to="/login">
                                    <Button
                                        variant="ghost"
                                        className="text-sm font-semibold hover:bg-gray-100 dark:hover:bg-gray-800 rounded-xl px-5"
                                    >
                                        Sign In
                                    </Button>
                                </Link>
                                <Link to="/register">
                                    <Button className="group relative bg-gradient-to-r from-blue-600 via-violet-600 to-blue-600 bg-[length:200%_auto] hover:bg-right text-white shadow-lg shadow-blue-500/25 hover:shadow-blue-500/40 transition-all duration-500 text-sm font-semibold px-6 py-2.5 rounded-xl border border-white/20 overflow-hidden">
                                        <span className="relative z-10 flex items-center gap-1">
                                            Get Started Free
                                            <ChevronRight className="h-4 w-4 group-hover:translate-x-0.5 transition-transform" />
                                        </span>
                                    </Button>
                                </Link>
                            </>
                        )}
                    </div>

                    {/* Mobile Toggle with Animation */}
                    <button
                        onClick={() => setMobileOpen(!mobileOpen)}
                        className="md:hidden relative p-2.5 rounded-xl bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 transition-all duration-200"
                    >
                        <div className="relative w-5 h-5">
                            <Menu className={`absolute inset-0 h-5 w-5 transition-all duration-300 ${mobileOpen ? 'opacity-0 rotate-90' : 'opacity-100 rotate-0'}`} />
                            <X className={`absolute inset-0 h-5 w-5 transition-all duration-300 ${mobileOpen ? 'opacity-100 rotate-0' : 'opacity-0 -rotate-90'}`} />
                        </div>
                    </button>
                </div>
            </div>

            {/* Mobile Menu with Slide Animation */}
            <div
                className={`md:hidden overflow-hidden transition-all duration-400 ease-out ${mobileOpen ? 'max-h-[400px] opacity-100' : 'max-h-0 opacity-0'
                    }`}
            >
                <div className="px-4 pb-6 pt-4 bg-white/95 dark:bg-gray-950/95 backdrop-blur-2xl border-t border-gray-200/50 dark:border-gray-800/50 mx-4 mt-2 rounded-2xl shadow-xl">
                    <nav className="flex flex-col gap-1">
                        {navLinks.map((link, index) => (
                            <a
                                key={link.href}
                                href={link.href}
                                onClick={() => setMobileOpen(false)}
                                className="px-4 py-3.5 text-sm font-semibold text-gray-700 dark:text-gray-200 hover:text-blue-600 dark:hover:text-blue-400 rounded-xl hover:bg-gray-50 dark:hover:bg-gray-800/80 transition-all duration-200 flex items-center justify-between group"
                                style={{ animationDelay: `${index * 50}ms` }}
                            >
                                {link.label}
                                <ChevronRight className="h-4 w-4 text-gray-400 group-hover:text-blue-500 group-hover:translate-x-0.5 transition-all" />
                            </a>
                        ))}
                    </nav>
                    <div className="flex flex-col gap-3 mt-5 pt-5 border-t border-gray-200 dark:border-gray-800">
                        {isAuthenticated ? (
                            <>
                                <div className="px-4 py-3 bg-gray-50 dark:bg-gray-800/50 rounded-xl">
                                    <p className="text-sm font-semibold text-gray-900 dark:text-white">{user?.username}</p>
                                    <p className="text-xs text-gray-500 dark:text-gray-400 truncate">{user?.email}</p>
                                </div>
                                <Link to="/analytics" onClick={() => setMobileOpen(false)}>
                                    <Button
                                        variant="outline"
                                        className="w-full justify-center rounded-xl py-3 font-semibold border-2"
                                    >
                                        Dashboard
                                    </Button>
                                </Link>
                                <Link to="/profile" onClick={() => setMobileOpen(false)}>
                                    <Button
                                        variant="outline"
                                        className="w-full justify-center rounded-xl py-3 font-semibold border-2"
                                    >
                                        Profile
                                    </Button>
                                </Link>
                                <button
                                    onClick={() => {
                                        setMobileOpen(false)
                                        setShowLogoutDialog(true)
                                    }}
                                    className="w-full px-4 py-3 text-sm font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl hover:bg-red-100 dark:hover:bg-red-900/30 transition-colors"
                                >
                                    Log out
                                </button>
                            </>
                        ) : (
                            <>
                                <Link to="/login" onClick={() => setMobileOpen(false)}>
                                    <Button
                                        variant="outline"
                                        className="w-full justify-center rounded-xl py-3 font-semibold border-2"
                                    >
                                        Sign In
                                    </Button>
                                </Link>
                                <Link to="/register" onClick={() => setMobileOpen(false)}>
                                    <Button className="w-full justify-center bg-gradient-to-r from-blue-600 via-violet-600 to-blue-600 bg-[length:200%_auto] text-white rounded-xl py-3 font-semibold shadow-lg shadow-blue-500/25">
                                        Get Started Free
                                    </Button>
                                </Link>
                            </>
                        )}
                    </div>
                </div>
            </div>

            {/* Logout Confirmation Dialog */}
            <LogoutConfirmDialog
                open={showLogoutDialog}
                onOpenChange={setShowLogoutDialog}
                onConfirm={handleLogout}
            />
        </header>
    )
}
