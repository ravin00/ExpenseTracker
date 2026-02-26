import { useScrollReveal } from '@/hooks/useScrollReveal'
import { Quote, Star, Verified } from 'lucide-react'

const testimonials = [
    {
        name: 'Sarah Johnson',
        role: 'Freelance Designer',
        avatar: 'SJ',
        content: 'SpendWise completely changed how I manage my finances. The budget alerts saved me from overspending multiple times. I\'ve saved over $3,000 in just 6 months!',
        rating: 5,
        color: 'from-blue-500 to-cyan-500',
        verified: true,
        savings: '$3,000+',
    },
    {
        name: 'Marcus Chen',
        role: 'Software Engineer',
        avatar: 'MC',
        content: 'The analytics are incredible. I never realized how much I was spending on subscriptions until SpendWise broke it all down for me. Clean UI and super intuitive.',
        rating: 5,
        color: 'from-violet-500 to-purple-500',
        verified: true,
        savings: '$1,200+',
    },
    {
        name: 'Emily Rodriguez',
        role: 'Small Business Owner',
        avatar: 'ER',
        content: 'I use SpendWise for both personal and business expenses. The category tracking and monthly reports make tax time so much easier. Highly recommend!',
        rating: 5,
        color: 'from-emerald-500 to-teal-500',
        verified: true,
        savings: '$5,000+',
    },
]

export function Testimonials() {
    const { ref, isVisible } = useScrollReveal()

    return (
        <section id="testimonials" className="py-28 sm:py-36 relative overflow-hidden">
            {/* Background */}
            <div className="absolute inset-0 -z-10">
                <div className="absolute inset-0 bg-gradient-to-b from-white via-amber-50/30 to-white dark:from-gray-950 dark:via-gray-900/50 dark:to-gray-950" />
                <div className="absolute top-1/4 right-0 w-96 h-96 bg-amber-500/10 rounded-full blur-3xl" />
                <div className="absolute bottom-1/4 left-0 w-96 h-96 bg-orange-500/10 rounded-full blur-3xl" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {/* Header */}
                <div ref={ref} className={`text-center max-w-3xl mx-auto mb-20 transition-all duration-700 ${isVisible ? 'animate-fade-in-up' : 'opacity-0'}`}>
                    <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-amber-50 to-orange-50 dark:from-amber-950/60 dark:to-orange-950/60 border border-amber-200/50 dark:border-amber-800/50 mb-8 shadow-sm">
                        <Star className="h-4 w-4 text-amber-600 dark:text-amber-400 fill-current" />
                        <span className="text-xs font-bold text-amber-700 dark:text-amber-300 tracking-wider uppercase">
                            Customer Stories
                        </span>
                    </div>
                    <h2 className="text-4xl sm:text-5xl lg:text-6xl font-black tracking-tight text-gray-900 dark:text-white mb-6 leading-[1.1]">
                        Loved by{' '}
                        <span className="bg-gradient-to-r from-amber-500 via-orange-500 to-red-500 bg-clip-text text-transparent">
                            thousands
                        </span>
                    </h2>
                    <p className="text-lg sm:text-xl text-gray-600 dark:text-gray-400 leading-relaxed max-w-2xl mx-auto">
                        See what our users have to say about their experience with SpendWise.
                    </p>
                </div>

                {/* Cards with Enhanced Design */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 lg:gap-8">
                    {testimonials.map((t, index) => (
                        <TestimonialCard key={t.name} testimonial={t} index={index} />
                    ))}
                </div>

                {/* Trust Indicators */}
                <div className="mt-16 text-center">
                    <div className="inline-flex items-center gap-3 px-6 py-3 rounded-full bg-gray-100 dark:bg-gray-800/80 border border-gray-200/50 dark:border-gray-700/50">
                        <div className="flex -space-x-3">
                            {['from-blue-500 to-cyan-500', 'from-violet-500 to-purple-500', 'from-emerald-500 to-teal-500', 'from-amber-500 to-orange-500'].map((color, i) => (
                                <div key={i} className={`w-8 h-8 rounded-full bg-gradient-to-br ${color} border-2 border-white dark:border-gray-800 flex items-center justify-center text-white text-xs font-bold shadow-sm`}>
                                    {['SJ', 'MC', 'ER', 'JD'][i]}
                                </div>
                            ))}
                        </div>
                        <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                            Join <span className="font-bold text-gray-900 dark:text-white">50,000+</span> happy users
                        </span>
                    </div>
                </div>
            </div>
        </section>
    )
}

function TestimonialCard({ testimonial, index }: { testimonial: typeof testimonials[number]; index: number }) {
    const { ref, isVisible } = useScrollReveal(0.1)

    return (
        <div
            ref={ref}
            className={`group relative rounded-3xl border border-gray-200/80 dark:border-gray-800/80 bg-white dark:bg-gray-900/80 p-8 hover:shadow-2xl transition-all duration-500 hover:-translate-y-2 overflow-hidden ${isVisible ? 'animate-fade-in-up' : 'opacity-0'
                }`}
            style={{ animationDelay: `${index * 150}ms` }}
        >
            {/* Top Gradient Border */}
            <div className={`absolute top-0 left-6 right-6 h-1 bg-gradient-to-r ${testimonial.color} rounded-b-full opacity-50 group-hover:opacity-100 transition-opacity`} />

            {/* Quote Icon */}
            <div className="absolute top-6 right-6">
                <Quote className={`h-10 w-10 text-gray-100 dark:text-gray-800 fill-current`} />
            </div>

            {/* Stars */}
            <div className="flex gap-1 mb-5">
                {[...Array(testimonial.rating)].map((_, i) => (
                    <Star key={i} className="h-5 w-5 text-amber-400 fill-current drop-shadow-sm" />
                ))}
            </div>

            {/* Content */}
            <p className="text-gray-700 dark:text-gray-300 leading-relaxed mb-8 text-base relative z-10">
                "{testimonial.content}"
            </p>

            {/* Savings Badge */}
            <div className={`inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-gradient-to-r ${testimonial.color} bg-opacity-10 mb-6`}>
                <span className={`text-xs font-bold bg-gradient-to-r ${testimonial.color} bg-clip-text text-transparent`}>
                    Saved {testimonial.savings}
                </span>
            </div>

            {/* Author */}
            <div className="flex items-center gap-4 pt-6 border-t border-gray-100 dark:border-gray-800">
                <div className={`relative w-12 h-12 rounded-xl bg-gradient-to-br ${testimonial.color} flex items-center justify-center text-white text-sm font-bold shadow-lg`}>
                    {testimonial.avatar}
                    {testimonial.verified && (
                        <div className="absolute -bottom-1 -right-1 w-5 h-5 rounded-full bg-white dark:bg-gray-900 flex items-center justify-center">
                            <Verified className="h-4 w-4 text-blue-500 fill-current" />
                        </div>
                    )}
                </div>
                <div>
                    <p className="font-bold text-gray-900 dark:text-white">{testimonial.name}</p>
                    <p className="text-sm text-gray-500 dark:text-gray-400">{testimonial.role}</p>
                </div>
            </div>
        </div>
    )
}
