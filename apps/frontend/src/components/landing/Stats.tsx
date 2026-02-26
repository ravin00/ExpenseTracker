import { useScrollReveal } from '@/hooks/useScrollReveal';
import { DollarSign, PiggyBank, Star, Users } from 'lucide-react';
import { useEffect, useRef, useState } from 'react';

const stats = [
    {
        label: 'Active Users',
        value: 50000,
        suffix: '+',
        prefix: '',
        icon: Users,
        color: 'from-blue-500 to-cyan-500',
    },
    {
        label: 'Expenses Tracked',
        value: 12,
        suffix: 'M+',
        prefix: '$',
        icon: DollarSign,
        color: 'from-emerald-500 to-teal-500',
    },
    {
        label: 'Money Saved',
        value: 2.4,
        suffix: 'M',
        prefix: '$',
        decimals: 1,
        icon: PiggyBank,
        color: 'from-violet-500 to-purple-500',
    },
    {
        label: 'App Rating',
        value: 4.9,
        suffix: '/5',
        prefix: '',
        decimals: 1,
        icon: Star,
        color: 'from-amber-500 to-orange-500',
    },
]

function AnimatedCounter({ value, suffix, prefix, decimals = 0, isVisible }: {
    value: number; suffix: string; prefix: string; decimals?: number; isVisible: boolean
}) {
    const [count, setCount] = useState(0)
    const hasAnimated = useRef(false)

    useEffect(() => {
        if (!isVisible || hasAnimated.current) return
        hasAnimated.current = true

        const duration = 2000
        const steps = 60
        const increment = value / steps
        let current = 0
        const timer = setInterval(() => {
            current += increment
            if (current >= value) {
                setCount(value)
                clearInterval(timer)
            } else {
                setCount(current)
            }
        }, duration / steps)

        return () => clearInterval(timer)
    }, [isVisible, value])

    return (
        <span>
            {prefix}{decimals > 0 ? count.toFixed(decimals) : Math.floor(count).toLocaleString()}{suffix}
        </span>
    )
}

export function Stats() {
    const { ref, isVisible } = useScrollReveal(0.2)

    return (
        <section className="py-24 sm:py-28 relative overflow-hidden">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div
                    ref={ref}
                    className={`relative rounded-[2.5rem] overflow-hidden transition-all duration-700 ${isVisible ? 'animate-scale-in' : 'opacity-0'}`}
                >
                    {/* Premium Gradient Background */}
                    <div className="absolute inset-0 bg-gradient-to-br from-gray-900 via-blue-950 to-violet-950" />

                    {/* Animated Mesh Gradient Overlay */}
                    <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top_right,rgba(120,119,198,0.3),transparent_50%)]" />
                    <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_bottom_left,rgba(59,130,246,0.2),transparent_50%)]" />

                    {/* Floating Orbs */}
                    <div className="absolute top-0 right-0 w-72 h-72 bg-blue-500/20 rounded-full blur-3xl -translate-y-1/2 translate-x-1/3 animate-blob" />
                    <div className="absolute bottom-0 left-0 w-72 h-72 bg-violet-500/20 rounded-full blur-3xl translate-y-1/2 -translate-x-1/3 animate-blob delay-700" />

                    {/* Grid Pattern */}
                    <div
                        className="absolute inset-0 opacity-[0.03]"
                        style={{
                            backgroundImage: `linear-gradient(rgba(255,255,255,0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,0.1) 1px, transparent 1px)`,
                            backgroundSize: '40px 40px',
                        }}
                    />

                    {/* Stats Grid */}
                    <div className="relative grid grid-cols-2 lg:grid-cols-4 gap-6 lg:gap-8 p-8 sm:p-12 lg:p-16">
                        {stats.map((stat, index) => {
                            const Icon = stat.icon
                            return (
                                <div
                                    key={stat.label}
                                    className="group relative text-center p-4 sm:p-6 rounded-2xl bg-white/5 backdrop-blur-sm border border-white/10 hover:bg-white/10 hover:border-white/20 transition-all duration-300"
                                    style={{ animationDelay: `${index * 150}ms` }}
                                >
                                    {/* Icon */}
                                    <div className={`inline-flex p-3 rounded-xl bg-gradient-to-br ${stat.color} mb-4 shadow-lg`}>
                                        <Icon className="h-5 w-5 text-white" />
                                    </div>

                                    {/* Value */}
                                    <p className="text-3xl sm:text-4xl lg:text-5xl font-black text-white mb-2 tracking-tight">
                                        <AnimatedCounter
                                            value={stat.value}
                                            suffix={stat.suffix}
                                            prefix={stat.prefix}
                                            decimals={stat.decimals}
                                            isVisible={isVisible}
                                        />
                                    </p>

                                    {/* Label */}
                                    <p className="text-blue-200/80 text-sm sm:text-base font-medium">{stat.label}</p>

                                    {/* Hover Glow */}
                                    <div className={`absolute inset-0 rounded-2xl bg-gradient-to-br ${stat.color} opacity-0 group-hover:opacity-10 transition-opacity duration-300 -z-10 blur-xl`} />
                                </div>
                            )
                        })}
                    </div>
                </div>
            </div>
        </section>
    )
}
