import { useScrollReveal } from '@/hooks/useScrollReveal'
import { BarChart3, CheckCircle2, CreditCard, UserPlus } from 'lucide-react'

const steps = [
    {
        icon: UserPlus,
        step: '01',
        title: 'Create Your Account',
        description: 'Sign up in seconds with just your email. No credit card required to get started.',
        color: 'from-blue-500 to-cyan-500',
        shadowColor: 'shadow-blue-500/25',
        features: ['Free forever plan', 'No credit card', 'Instant setup'],
    },
    {
        icon: CreditCard,
        step: '02',
        title: 'Track Your Expenses',
        description: 'Log expenses quickly with smart categorization. Set budgets and savings goals.',
        color: 'from-violet-500 to-purple-500',
        shadowColor: 'shadow-violet-500/25',
        features: ['Smart categories', 'Budget alerts', 'Quick entry'],
    },
    {
        icon: BarChart3,
        step: '03',
        title: 'Get Smart Insights',
        description: 'View powerful analytics and reports that help you understand and optimize spending.',
        color: 'from-emerald-500 to-teal-500',
        shadowColor: 'shadow-emerald-500/25',
        features: ['AI insights', 'Visual charts', 'Export reports'],
    },
]

export function HowItWorks() {
    const { ref, isVisible } = useScrollReveal()

    return (
        <section id="how-it-works" className="py-28 sm:py-36 relative overflow-hidden">
            {/* Background */}
            <div className="absolute inset-0 -z-10">
                <div className="absolute inset-0 bg-gradient-to-b from-white via-gray-50/50 to-white dark:from-gray-950 dark:via-gray-900/50 dark:to-gray-950" />
                <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[800px] h-[800px] bg-gradient-to-r from-blue-500/5 via-violet-500/5 to-emerald-500/5 rounded-full blur-3xl" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {/* Header */}
                <div ref={ref} className={`text-center max-w-3xl mx-auto mb-20 transition-all duration-700 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'}`}>
                    <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-violet-50 to-purple-50 dark:from-violet-950/60 dark:to-purple-950/60 border border-violet-200/50 dark:border-violet-800/50 mb-8 shadow-sm">
                        <span className="text-xs font-bold text-violet-700 dark:text-violet-300 tracking-wider uppercase">
                            How It Works
                        </span>
                    </div>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-black tracking-tight text-gray-900 dark:text-white mb-6 leading-[1.1]">
                        Get started in{' '}
                        <span className="bg-gradient-to-r from-violet-600 via-purple-600 to-fuchsia-600 bg-clip-text text-transparent">
                            3 simple steps
                        </span>
                    </h2>
                    <p className="text-lg sm:text-xl text-gray-600 dark:text-gray-400 leading-relaxed max-w-2xl mx-auto">
                        No complicated setup. Start tracking your finances in under a minute.
                    </p>
                </div>

                {/* Steps with Timeline */}
                <div className="relative">
                    {/* Progress Line (Desktop) */}
                    <div className="hidden lg:block absolute top-32 left-[16.67%] right-[16.67%] h-1 bg-gradient-to-r from-blue-200 via-violet-200 to-emerald-200 dark:from-blue-900 dark:via-violet-900 dark:to-emerald-900 rounded-full overflow-hidden">
                        <div className="h-full w-1/2 bg-gradient-to-r from-blue-500 via-violet-500 to-emerald-500 rounded-full animate-shimmer" style={{ backgroundSize: '200% 100%' }} />
                    </div>

                    <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 lg:gap-6">
                        {steps.map((step, index) => (
                            <StepCard key={step.step} step={step} index={index} />
                        ))}
                    </div>
                </div>
            </div>
        </section>
    )
}

function StepCard({ step, index }: { step: typeof steps[number]; index: number }) {
    const { ref, isVisible } = useScrollReveal(0.15)
    const Icon = step.icon

    return (
        <div
            ref={ref}
            className={`group relative transition-all duration-700 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'
                }`}
            style={{ animationDelay: `${index * 200}ms` }}
        >
            {/* Card */}
            <div className="relative bg-white dark:bg-gray-900/80 rounded-3xl border border-gray-200/80 dark:border-gray-800/80 p-8 hover:shadow-2xl hover:-translate-y-2 transition-all duration-500 overflow-hidden">
                {/* Top Gradient Border */}
                <div className={`absolute top-0 left-8 right-8 h-1 bg-gradient-to-r ${step.color} rounded-b-full opacity-60 group-hover:opacity-100 transition-opacity`} />

                {/* Background Glow on Hover */}
                <div className={`absolute inset-0 bg-gradient-to-br ${step.color} opacity-0 group-hover:opacity-[0.02] transition-opacity duration-500`} />

                {/* Step Number & Icon Container */}
                <div className="relative flex items-center justify-center mb-8">
                    <div className={`w-24 h-24 rounded-3xl bg-gradient-to-br ${step.color} flex items-center justify-center shadow-2xl ${step.shadowColor} group-hover:scale-105 transition-transform duration-300`}>
                        <Icon className="h-10 w-10 text-white" />
                    </div>
                    {/* Floating Step Badge */}
                    <div className="absolute -top-2 -right-2 w-10 h-10 rounded-full bg-white dark:bg-gray-800 border-2 border-gray-100 dark:border-gray-700 flex items-center justify-center shadow-lg">
                        <span className="text-sm font-black text-gray-800 dark:text-gray-200">{step.step}</span>
                    </div>
                </div>

                {/* Content */}
                <div className="text-center">
                    <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-3">{step.title}</h3>
                    <p className="text-gray-600 dark:text-gray-400 leading-relaxed mb-6">{step.description}</p>

                    {/* Feature Pills */}
                    <div className="flex flex-wrap justify-center gap-2">
                        {step.features.map((feature) => (
                            <div
                                key={feature}
                                className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full bg-gray-100 dark:bg-gray-800 text-xs font-medium text-gray-700 dark:text-gray-300"
                            >
                                <CheckCircle2 className={`h-3 w-3 text-${step.color.split('-')[1]}-500`} />
                                {feature}
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}
