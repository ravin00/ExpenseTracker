import { Button } from '@/components/ui/button';
import { ArrowRight, Play, Shield, Sparkles, TrendingUp, Zap } from 'lucide-react';

export function Hero() {
    return (
        <section className="relative min-h-screen flex items-center justify-center overflow-hidden pt-20">
            {/* Animated Background */}
            <div className="absolute inset-0 -z-10">
                {/* Mesh Gradient Background */}
                <div className="absolute inset-0 bg-[radial-gradient(ellipse_80%_80%_at_50%_-20%,rgba(120,119,198,0.3),rgba(255,255,255,0))]" />

                {/* Floating Gradient Orbs with Animation */}
                <div className="absolute top-1/4 -left-20 w-[500px] h-[500px] bg-gradient-to-r from-blue-500/30 to-violet-500/30 rounded-full blur-3xl animate-blob" />
                <div className="absolute top-1/3 -right-20 w-[400px] h-[400px] bg-gradient-to-r from-violet-500/25 to-fuchsia-500/25 rounded-full blur-3xl animate-blob delay-700" />
                <div className="absolute bottom-1/4 left-1/3 w-[600px] h-[600px] bg-gradient-to-r from-cyan-500/20 to-blue-500/20 rounded-full blur-3xl animate-blob delay-300" />

                {/* Animated Grid Pattern */}
                <div
                    className="absolute inset-0 opacity-[0.02] dark:opacity-[0.04]"
                    style={{
                        backgroundImage: `linear-gradient(rgba(0,0,0,0.1) 1.5px, transparent 1.5px), linear-gradient(90deg, rgba(0,0,0,0.1) 1.5px, transparent 1.5px)`,
                        backgroundSize: '80px 80px',
                    }}
                />

                {/* Floating Particles */}
                <div className="absolute inset-0 overflow-hidden">
                    {[...Array(20)].map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-1 h-1 bg-blue-500/40 rounded-full animate-float"
                            style={{
                                left: `${Math.random() * 100}%`,
                                top: `${Math.random() * 100}%`,
                                animationDelay: `${i * 0.3}s`,
                                animationDuration: `${4 + Math.random() * 4}s`,
                            }}
                        />
                    ))}
                </div>

                {/* Radial Fade */}
                <div className="absolute inset-0 bg-gradient-to-b from-transparent via-transparent to-white/90 dark:to-gray-950/90" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
                {/* Badge with Glow */}
                <div className="animate-fade-in-down inline-flex items-center gap-2 px-5 py-2 rounded-full bg-gradient-to-r from-blue-50 to-violet-50 dark:from-blue-950/60 dark:to-violet-950/60 border border-blue-200/60 dark:border-blue-700/50 mb-8 shadow-lg shadow-blue-500/10">
                    <div className="relative">
                        <Sparkles className="h-4 w-4 text-blue-600 dark:text-blue-400 animate-pulse" />
                        <div className="absolute inset-0 bg-blue-400 blur-md opacity-40" />
                    </div>
                    <span className="text-xs font-bold text-blue-700 dark:text-blue-300 tracking-wider uppercase">
                        AI-Powered Expense Tracking
                    </span>
                    <span className="px-2 py-0.5 bg-blue-600/10 dark:bg-blue-400/10 rounded-full text-[10px] font-semibold text-blue-600 dark:text-blue-400">NEW</span>
                </div>

                {/* Headline with Enhanced Typography */}
                <h1 className="animate-fade-in-up text-5xl sm:text-6xl md:text-7xl lg:text-8xl font-black tracking-tighter leading-[0.95] mb-8">
                    <span className="block text-gray-900 dark:text-white drop-shadow-sm">Take Control of</span>
                    <span className="block mt-3 bg-gradient-to-r from-blue-600 via-violet-600 to-fuchsia-600 bg-clip-text text-transparent animate-gradient-x" style={{ backgroundSize: '200% auto' }}>
                        Your Finances
                    </span>
                </h1>

                {/* Subheadline with Better Contrast */}
                <p className="animate-fade-in-up delay-200 max-w-2xl mx-auto text-lg sm:text-xl text-gray-600 dark:text-gray-300 leading-relaxed mb-12 font-medium">
                    Track expenses, set intelligent budgets, and gain{' '}
                    <span className="text-blue-600 dark:text-blue-400 font-semibold">AI-powered insights</span>{' '}
                    into your spending habits. The smart way to manage your money.
                </p>

                {/* CTA Buttons with Enhanced Styling */}
                <div className="animate-fade-in-up delay-400 flex flex-col sm:flex-row items-center justify-center gap-4 mb-20">
                    <Button
                        size="lg"
                        className="group relative bg-gradient-to-r from-blue-600 via-violet-600 to-blue-600 bg-[length:200%_auto] hover:bg-right text-white shadow-2xl shadow-blue-500/30 hover:shadow-blue-500/50 px-10 py-7 text-base font-semibold rounded-2xl transition-all duration-500 hover:scale-105 border border-white/20"
                    >
                        <span className="relative z-10 flex items-center">
                            Start Tracking Free
                            <ArrowRight className="ml-2 h-5 w-5 group-hover:translate-x-1.5 transition-transform duration-300" />
                        </span>
                        <div className="absolute inset-0 rounded-2xl bg-gradient-to-r from-white/0 via-white/25 to-white/0 opacity-0 group-hover:opacity-100 transition-opacity duration-500 animate-shimmer" />
                    </Button>
                    <Button
                        variant="outline"
                        size="lg"
                        className="group px-8 py-7 text-base font-semibold rounded-2xl border-2 border-gray-200 dark:border-gray-700 bg-white/80 dark:bg-gray-900/80 backdrop-blur-sm hover:bg-gray-50 dark:hover:bg-gray-800 transition-all duration-300 hover:border-gray-300 dark:hover:border-gray-600"
                    >
                        <div className="relative mr-2">
                            <Play className="h-5 w-5 text-blue-600 group-hover:text-violet-600 transition-colors" />
                        </div>
                        Watch Demo
                    </Button>
                </div>

                {/* Hero Visual – Premium Dashboard Preview */}
                <div className="animate-fade-in-up delay-600 relative max-w-5xl mx-auto">
                    {/* Multi-layer Glow Effect */}
                    <div className="absolute -inset-6 bg-gradient-to-r from-blue-600/30 via-violet-600/30 to-fuchsia-600/30 rounded-3xl blur-3xl animate-pulse-glow" />
                    <div className="absolute -inset-3 bg-gradient-to-r from-blue-500/20 to-violet-500/20 rounded-3xl blur-xl" />

                    <div className="relative rounded-3xl border border-gray-200/60 dark:border-gray-700/60 bg-white/90 dark:bg-gray-900/90 backdrop-blur-2xl shadow-2xl overflow-hidden">
                        {/* Premium Browser Chrome */}
                        <div className="flex items-center gap-3 px-5 py-4 bg-gradient-to-r from-gray-50 via-gray-100/80 to-gray-50 dark:from-gray-800 dark:via-gray-850 dark:to-gray-800 border-b border-gray-200/80 dark:border-gray-700/80">
                            <div className="flex gap-2">
                                <div className="w-3.5 h-3.5 rounded-full bg-gradient-to-br from-red-400 to-red-500 shadow-sm" />
                                <div className="w-3.5 h-3.5 rounded-full bg-gradient-to-br from-yellow-400 to-amber-500 shadow-sm" />
                                <div className="w-3.5 h-3.5 rounded-full bg-gradient-to-br from-green-400 to-emerald-500 shadow-sm" />
                            </div>
                            <div className="flex-1 flex justify-center">
                                <div className="px-6 py-1.5 bg-white dark:bg-gray-700/80 rounded-lg text-xs text-gray-500 dark:text-gray-400 flex items-center gap-2 shadow-inner border border-gray-200/50 dark:border-gray-600/50">
                                    <Shield className="h-3 w-3 text-green-500" />
                                    <span>app.spendwise.io/dashboard</span>
                                </div>
                            </div>
                        </div>

                        {/* Dashboard Preview Content */}
                        <div className="p-6 sm:p-8 bg-gradient-to-br from-gray-50/50 to-white dark:from-gray-900 dark:to-gray-900/80">
                            {/* Top Stats Row */}
                            <div className="grid grid-cols-1 sm:grid-cols-3 gap-5 mb-6">
                                <DashboardCard
                                    label="Total Balance"
                                    value="$12,480"
                                    change="+8.2%"
                                    positive
                                    icon={<TrendingUp className="h-4 w-4" />}
                                    gradient="from-blue-500 to-cyan-500"
                                />
                                <DashboardCard
                                    label="Monthly Spent"
                                    value="$3,240"
                                    change="-12.5%"
                                    positive
                                    icon={<Shield className="h-4 w-4" />}
                                    gradient="from-emerald-500 to-teal-500"
                                />
                                <DashboardCard
                                    label="Savings Goal"
                                    value="68%"
                                    change="+5.3%"
                                    positive
                                    icon={<Zap className="h-4 w-4" />}
                                    gradient="from-violet-500 to-purple-500"
                                />
                            </div>

                            {/* Premium Chart Area */}
                            <div className="rounded-2xl bg-gradient-to-br from-gray-50 to-gray-100/80 dark:from-gray-800/80 dark:to-gray-800/40 p-6 h-52 sm:h-64 flex items-end gap-1.5 border border-gray-200/50 dark:border-gray-700/50 relative overflow-hidden">
                                {/* Chart Grid Lines */}
                                <div className="absolute inset-6 flex flex-col justify-between">
                                    {[...Array(4)].map((_, i) => (
                                        <div key={i} className="border-b border-dashed border-gray-200 dark:border-gray-700/50" />
                                    ))}
                                </div>

                                {[40, 65, 45, 80, 55, 90, 70, 85, 60, 95, 75, 88].map((h, i) => (
                                    <div key={i} className="flex-1 flex flex-col justify-end relative z-10">
                                        <div
                                            className="rounded-t-lg bg-gradient-to-t from-blue-600 via-violet-500 to-fuchsia-500 opacity-90 hover:opacity-100 transition-all duration-300 hover:scale-y-110 origin-bottom cursor-pointer shadow-lg shadow-blue-500/20 hover:shadow-blue-500/40"
                                            style={{
                                                height: `${h}%`,
                                                animationDelay: `${i * 100}ms`,
                                            }}
                                        />
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>

                {/* Trust Badges with Enhanced Design */}
                <div className="animate-fade-in-up delay-800 mt-20 flex flex-col items-center gap-6">
                    <div className="flex items-center gap-8 flex-wrap justify-center">
                        <div className="flex items-center gap-2 text-gray-500 dark:text-gray-400">
                            <Shield className="h-5 w-5 text-green-500" />
                            <span className="text-sm font-medium">Bank-Level Security</span>
                        </div>
                        <div className="hidden sm:block w-px h-4 bg-gray-300 dark:bg-gray-700" />
                        <div className="flex items-center gap-2 text-gray-500 dark:text-gray-400">
                            <Zap className="h-5 w-5 text-amber-500" />
                            <span className="text-sm font-medium">Real-time Sync</span>
                        </div>
                        <div className="hidden sm:block w-px h-4 bg-gray-300 dark:bg-gray-700" />
                        <div className="flex items-center gap-1.5">
                            {[...Array(5)].map((_, i) => (
                                <svg key={i} className="w-5 h-5 text-amber-400 fill-current drop-shadow-sm" viewBox="0 0 20 20">
                                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                                </svg>
                            ))}
                            <span className="ml-2 text-sm font-semibold text-gray-700 dark:text-gray-200">4.9/5</span>
                            <span className="text-sm text-gray-500 dark:text-gray-400">• 50K+ users</span>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

function DashboardCard({ label, value, change, positive, icon, gradient }: {
    label: string;
    value: string;
    change: string;
    positive: boolean;
    icon: React.ReactNode;
    gradient: string;
}) {
    return (
        <div className="group relative rounded-2xl bg-white dark:bg-gray-800/80 border border-gray-200/80 dark:border-gray-700/80 p-5 text-left hover:shadow-xl hover:-translate-y-1 transition-all duration-300 overflow-hidden">
            {/* Hover Gradient Overlay */}
            <div className={`absolute inset-0 bg-gradient-to-br ${gradient} opacity-0 group-hover:opacity-5 transition-opacity duration-300`} />

            <div className="flex items-start justify-between relative z-10">
                <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400 font-medium">{label}</p>
                    <p className="text-2xl sm:text-3xl font-bold text-gray-900 dark:text-white mt-1">{value}</p>
                </div>
                <div className={`p-2.5 rounded-xl bg-gradient-to-br ${gradient} shadow-lg`}>
                    <div className="text-white">{icon}</div>
                </div>
            </div>
            <div className="flex items-center gap-1.5 mt-3 relative z-10">
                <span className={`inline-flex items-center gap-1 text-xs font-semibold px-2 py-1 rounded-full ${positive ? 'text-emerald-700 bg-emerald-100 dark:text-emerald-400 dark:bg-emerald-950/50' : 'text-red-700 bg-red-100 dark:text-red-400 dark:bg-red-950/50'}`}>
                    {change}
                </span>
                <span className="text-xs text-gray-500 dark:text-gray-500">from last month</span>
            </div>
        </div>
    )
}
