import { Button } from '@/components/ui/button'
import { Link } from '@tanstack/react-router'
import { ChevronRight, Menu, Wallet, X } from 'lucide-react'
import { useEffect, useState } from 'react'

export function Navbar() {
    const [scrolled, setScrolled] = useState(false)
    const [mobileOpen, setMobileOpen] = useState(false)

    useEffect(() => {
        const onScroll = () => setScrolled(window.scrollY > 20)
        window.addEventListener('scroll', onScroll)
        return () => window.removeEventListener('scroll', onScroll)
    }, [])

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
                    </div>
                </div>
            </div>
        </header>
    )
}
