import { Button } from '@/components/ui/button'
import { useScrollReveal } from '@/hooks/useScrollReveal'
import { ArrowRight, Check, Crown, Sparkles, Zap } from 'lucide-react'

const plans = [
    {
        name: 'Free',
        price: '$0',
        period: 'forever',
        description: 'Perfect for getting started with expense tracking.',
        features: [
            'Track up to 100 expenses/month',
            'Basic analytics & charts',
            '3 budget categories',
            '1 savings goal',
            'Email support',
        ],
        cta: 'Get Started Free',
        popular: false,
        gradient: 'from-gray-600 to-slate-700',
        icon: Zap,
        shadowColor: 'shadow-gray-500/20',
    },
    {
        name: 'Pro',
        price: '$9',
        period: '/month',
        description: 'For individuals serious about financial control.',
        features: [
            'Unlimited expenses',
            'Advanced analytics & AI insights',
            'Unlimited budget categories',
            'Unlimited savings goals',
            'Export to CSV/PDF',
            'Priority support',
            'Custom categories',
        ],
        cta: 'Start Pro Trial',
        popular: true,
        gradient: 'from-blue-600 via-violet-600 to-fuchsia-600',
        icon: Crown,
        shadowColor: 'shadow-blue-500/30',
    },
    {
        name: 'Family',
        price: '$19',
        period: '/month',
        description: 'Manage finances together as a household.',
        features: [
            'Everything in Pro',
            'Up to 5 family members',
            'Shared budgets & goals',
            'Family spending reports',
            'Bill splitting',
            'Dedicated account manager',
            'API access',
        ],
        cta: 'Start Family Trial',
        popular: false,
        gradient: 'from-emerald-500 to-teal-600',
        icon: Sparkles,
        shadowColor: 'shadow-emerald-500/20',
    },
]

export function Pricing() {
    const { ref, isVisible } = useScrollReveal()

    return (
        <section id="pricing" className="py-28 sm:py-36 relative overflow-hidden">
            {/* Background */}
            <div className="absolute inset-0 -z-10">
                <div className="absolute inset-0 bg-gradient-to-b from-white via-gray-50/50 to-white dark:from-gray-950 dark:via-gray-900/50 dark:to-gray-950" />
                <div className="absolute top-0 left-1/2 -translate-x-1/2 w-[1000px] h-[600px] bg-gradient-to-r from-blue-500/5 via-violet-500/10 to-fuchsia-500/5 rounded-full blur-3xl" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {/* Header */}
                <div ref={ref} className={`text-center max-w-3xl mx-auto mb-20 transition-all duration-700 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'}`}>
                    <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-emerald-50 to-teal-50 dark:from-emerald-950/60 dark:to-teal-950/60 border border-emerald-200/50 dark:border-emerald-800/50 mb-8 shadow-sm">
                        <Sparkles className="h-4 w-4 text-emerald-600 dark:text-emerald-400" />
                        <span className="text-xs font-bold text-emerald-700 dark:text-emerald-300 tracking-wider uppercase">
                            Simple Pricing
                        </span>
                    </div>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-black tracking-tight text-gray-900 dark:text-white mb-6 leading-[1.1]">
                        Simple, transparent{' '}
                        <span className="bg-gradient-to-r from-emerald-500 via-teal-500 to-cyan-500 bg-clip-text text-transparent">
                            pricing
                        </span>
                    </h2>
                    <p className="text-lg sm:text-xl text-gray-600 dark:text-gray-400 leading-relaxed max-w-2xl mx-auto">
                        Start for free. Upgrade when you need more power. No hidden fees, ever.
                    </p>
                </div>

                {/* Pricing Cards */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 lg:gap-5 items-stretch">
                    {plans.map((plan, index) => (
                        <PricingCard key={plan.name} plan={plan} index={index} />
                    ))}
                </div>

                {/* Money-back guarantee */}
                <div className="mt-16 text-center">
                    <p className="inline-flex items-center gap-2 text-sm text-gray-500 dark:text-gray-400">
                        <Check className="h-4 w-4 text-emerald-500" />
                        30-day money-back guarantee • Cancel anytime • No hidden fees
                    </p>
                </div>
            </div>
        </section>
    )
}

function PricingCard({ plan, index }: { plan: typeof plans[number]; index: number }) {
    const { ref, isVisible } = useScrollReveal(0.1)
    const Icon = plan.icon

    return (
        <div
            ref={ref}
            className={`relative rounded-3xl transition-all duration-500 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'
                } ${plan.popular
                    ? 'border-2 border-blue-500/50 dark:border-blue-400/50 shadow-2xl ' + plan.shadowColor + ' lg:scale-105 z-10'
                    : 'border border-gray-200/80 dark:border-gray-800/80 hover:border-gray-300 dark:hover:border-gray-700'
                } bg-white dark:bg-gray-900/90 backdrop-blur-sm overflow-hidden hover:shadow-xl hover:-translate-y-1`}
            style={{ animationDelay: `${index * 150}ms` }}
        >
            {/* Popular Badge */}
            {plan.popular && (
                <div className="absolute -top-px left-0 right-0 h-1 bg-gradient-to-r from-blue-600 via-violet-600 to-fuchsia-600" />
            )}

            {/* Popular Label */}
            {plan.popular && (
                <div className="absolute top-0 left-1/2 -translate-x-1/2 -translate-y-1/2">
                    <div className="bg-gradient-to-r from-blue-600 via-violet-600 to-fuchsia-600 text-white text-xs font-bold px-5 py-2 rounded-full shadow-xl shadow-blue-500/30 flex items-center gap-2">
                        <Crown className="h-3.5 w-3.5" />
                        Most Popular
                    </div>
                </div>
            )}

            <div className="p-8 pt-10">
                {/* Icon & Name */}
                <div className="flex items-center gap-3 mb-4">
                    <div className={`p-2.5 rounded-xl bg-gradient-to-br ${plan.gradient} shadow-lg`}>
                        <Icon className="h-5 w-5 text-white" />
                    </div>
                    <h3 className="text-xl font-bold text-gray-900 dark:text-white">{plan.name}</h3>
                </div>

                {/* Price */}
                <div className="flex items-baseline gap-1 mb-3">
                    <span className="text-5xl font-black text-gray-900 dark:text-white tracking-tight">{plan.price}</span>
                    <span className="text-gray-500 dark:text-gray-400 text-base font-medium">{plan.period}</span>
                </div>

                <p className="text-sm text-gray-600 dark:text-gray-400 mb-8">{plan.description}</p>

                {/* Features */}
                <ul className="space-y-3.5 mb-8">
                    {plan.features.map((feature) => (
                        <li key={feature} className="flex items-start gap-3">
                            <div className={`mt-0.5 flex-shrink-0 w-5 h-5 rounded-full bg-gradient-to-br ${plan.gradient} flex items-center justify-center shadow-sm`}>
                                <Check className="h-3 w-3 text-white" />
                            </div>
                            <span className="text-sm text-gray-700 dark:text-gray-300">{feature}</span>
                        </li>
                    ))}
                </ul>

                {/* CTA Button */}
                <Button
                    className={`group w-full py-6 rounded-xl text-sm font-bold transition-all duration-300 ${plan.popular
                        ? `bg-gradient-to-r ${plan.gradient} bg-[length:200%_auto] hover:bg-right text-white shadow-lg ${plan.shadowColor} hover:shadow-xl border border-white/20`
                        : 'bg-gray-100 dark:bg-gray-800 text-gray-900 dark:text-white hover:bg-gray-200 dark:hover:bg-gray-700 border border-gray-200 dark:border-gray-700'
                        }`}
                >
                    <span className="flex items-center justify-center gap-2">
                        {plan.cta}
                        <ArrowRight className="h-4 w-4 group-hover:translate-x-1 transition-transform" />
                    </span>
                </Button>
            </div>
        </div>
    )
}
