import {
    ArrowUpRight,
    BarChart3,
    Bell,
    Lock,
    PieChart,
    Shield,
    Smartphone,
    TrendingUp,
    Zap,
} from 'lucide-react'
import { useScrollReveal } from '../../hooks/useScrollReveal'

const features = [
    {
        icon: BarChart3,
        title: 'Smart Analytics',
        description: 'AI-powered insights into your spending patterns with visual charts and trend analysis.',
        color: 'blue',
        gradient: 'from-blue-500 to-cyan-500',
        shadowColor: 'shadow-blue-500/20',
    },
    {
        icon: Shield,
        title: 'Budget Guardrails',
        description: 'Set spending limits by category and get alerts before you overspend.',
        color: 'emerald',
        gradient: 'from-emerald-500 to-teal-500',
        shadowColor: 'shadow-emerald-500/20',
    },
    {
        icon: Zap,
        title: 'Instant Tracking',
        description: 'Add expenses in seconds with quick-entry and smart categorization.',
        color: 'amber',
        gradient: 'from-amber-500 to-orange-500',
        shadowColor: 'shadow-amber-500/20',
    },
    {
        icon: PieChart,
        title: 'Category Breakdown',
        description: 'See exactly where your money goes with detailed category reports.',
        color: 'violet',
        gradient: 'from-violet-500 to-purple-500',
        shadowColor: 'shadow-violet-500/20',
    },
    {
        icon: Bell,
        title: 'Smart Alerts',
        description: 'Receive notifications for unusual spending, bill reminders, and budget limits.',
        color: 'rose',
        gradient: 'from-rose-500 to-pink-500',
        shadowColor: 'shadow-rose-500/20',
    },
    {
        icon: TrendingUp,
        title: 'Savings Goals',
        description: 'Set savings targets and track your progress with motivational milestones.',
        color: 'cyan',
        gradient: 'from-cyan-500 to-blue-500',
        shadowColor: 'shadow-cyan-500/20',
    },
    {
        icon: Smartphone,
        title: 'Works Everywhere',
        description: 'Responsive design that works beautifully on desktop, tablet, and mobile.',
        color: 'indigo',
        gradient: 'from-indigo-500 to-violet-500',
        shadowColor: 'shadow-indigo-500/20',
    },
    {
        icon: Lock,
        title: 'Bank-Level Security',
        description: 'Your data is encrypted and protected with enterprise-grade security.',
        color: 'slate',
        gradient: 'from-slate-600 to-slate-800',
        shadowColor: 'shadow-slate-500/20',
    },
]

export function Features() {
    const { ref, isVisible } = useScrollReveal()

    return (
        <section id="features" className="py-28 sm:py-36 relative overflow-hidden">
            {/* Background Elements */}
            <div className="absolute inset-0 -z-10">
                <div className="absolute inset-0 bg-gradient-to-b from-white via-gray-50/80 to-white dark:from-gray-950 dark:via-gray-900/80 dark:to-gray-950" />
                <div className="absolute top-0 left-1/4 w-96 h-96 bg-blue-500/5 rounded-full blur-3xl" />
                <div className="absolute bottom-0 right-1/4 w-96 h-96 bg-violet-500/5 rounded-full blur-3xl" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {/* Section Header */}
                <div ref={ref} className={`text-center max-w-3xl mx-auto mb-20 transition-all duration-700 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'}`}>
                    <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-blue-50 to-violet-50 dark:from-blue-950/60 dark:to-violet-950/60 border border-blue-200/50 dark:border-blue-800/50 mb-8 shadow-sm">
                        <Zap className="h-4 w-4 text-blue-600 dark:text-blue-400" />
                        <span className="text-xs font-bold text-blue-700 dark:text-blue-300 tracking-wider uppercase">Powerful Features</span>
                    </div>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-black tracking-tight text-gray-900 dark:text-white mb-6 leading-[1.1]">
                        Everything you need to{' '}
                        <span className="bg-gradient-to-r from-blue-600 via-violet-600 to-fuchsia-600 bg-clip-text text-transparent">
                            master your money
                        </span>
                    </h2>
                    <p className="text-lg sm:text-xl text-gray-600 dark:text-gray-400 leading-relaxed max-w-2xl mx-auto">
                        Powerful features designed to simplify your financial life and help you make smarter spending decisions.
                    </p>
                </div>

                {/* Bento Grid Layout */}
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
                    {features.map((feature, index) => (
                        <FeatureCard key={feature.title} feature={feature} index={index} />
                    ))}
                </div>
            </div>
        </section>
    )
}

function FeatureCard({ feature, index }: { feature: typeof features[number]; index: number }) {
    const { ref, isVisible } = useScrollReveal(0.1)
    const Icon = feature.icon

    return (
        <div
            ref={ref}
            className={`group relative rounded-3xl border border-gray-200/80 dark:border-gray-800/80 bg-white dark:bg-gray-900/80 p-7 transition-all duration-500 hover:shadow-2xl ${feature.shadowColor} hover:-translate-y-2 cursor-pointer overflow-hidden ${isVisible ? 'animate-fade-in-up' : 'opacity-0'
                }`}
            style={{ animationDelay: `${index * 80}ms` }}
        >
            {/* Hover Gradient Background */}
            <div className={`absolute inset-0 bg-gradient-to-br ${feature.gradient} opacity-0 group-hover:opacity-[0.03] transition-opacity duration-500`} />

            {/* Top Glow on Hover */}
            <div className={`absolute -top-px left-10 right-10 h-px bg-gradient-to-r from-transparent ${feature.gradient.replace('from-', 'via-').split(' ')[0]} to-transparent opacity-0 group-hover:opacity-60 transition-opacity duration-500`} />

            {/* Icon Container with Gradient */}
            <div className="relative mb-5">
                <div className={`inline-flex p-4 rounded-2xl bg-gradient-to-br ${feature.gradient} shadow-lg ${feature.shadowColor}`}>
                    <Icon className="h-6 w-6 text-white" />
                </div>
            </div>

            {/* Content */}
            <div className="relative">
                <div className="flex items-center gap-2 mb-3">
                    <h3 className="text-lg font-bold text-gray-900 dark:text-white">
                        {feature.title}
                    </h3>
                    <ArrowUpRight className="h-4 w-4 text-gray-400 opacity-0 group-hover:opacity-100 group-hover:translate-x-0.5 group-hover:-translate-y-0.5 transition-all duration-300" />
                </div>
                <p className="text-sm text-gray-600 dark:text-gray-400 leading-relaxed">
                    {feature.description}
                </p>
            </div>

            {/* Bottom Border Gradient on Hover */}
            <div className={`absolute bottom-0 left-0 right-0 h-1 bg-gradient-to-r ${feature.gradient} opacity-0 group-hover:opacity-100 transition-opacity duration-500 rounded-b-3xl`} />
        </div>
    )
}
