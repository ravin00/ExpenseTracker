import { Button } from '@/components/ui/button'
import { useScrollReveal } from '@/hooks/useScrollReveal'
import { ArrowRight, Check, Shield, Sparkles, Zap } from 'lucide-react'

const CTA_PARTICLES = Array.from({ length: 15 }, (_, i) => ({
    left: (i * 37) % 100,
    top: (i * 53) % 100,
    delay: i * 0.4,
    duration: 5 + (i % 6),
}))

export function CTA() {
    const { ref, isVisible } = useScrollReveal()

    return (
        <section className="py-24 sm:py-32 relative overflow-hidden">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div
                    ref={ref}
                    className={`relative rounded-[2.5rem] overflow-hidden transition-all duration-700 ${isVisible ? 'animate-scale-in' : 'opacity-0'}`}
                >
                    {/* Premium Dark Background */}
                    <div className="absolute inset-0 bg-gradient-to-br from-gray-900 via-slate-900 to-gray-950" />

                    {/* Mesh Gradient Overlays */}
                    <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top_right,rgba(59,130,246,0.2),transparent_60%)]" />
                    <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_bottom_left,rgba(139,92,246,0.2),transparent_60%)]" />
                    <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_center,rgba(236,72,153,0.1),transparent_50%)]" />

                    {/* Animated Floating Orbs */}
                    <div className="absolute top-0 right-0 w-[400px] h-[400px] bg-blue-500/20 rounded-full -translate-y-1/2 translate-x-1/4 blur-3xl animate-blob" />
                    <div className="absolute bottom-0 left-0 w-[400px] h-[400px] bg-violet-500/20 rounded-full translate-y-1/2 -translate-x-1/4 blur-3xl animate-blob delay-700" />
                    <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[300px] bg-fuchsia-500/10 rounded-full blur-3xl animate-pulse" />

                    {/* Grid Pattern */}
                    <div
                        className="absolute inset-0 opacity-[0.03]"
                        style={{
                            backgroundImage: `radial-gradient(circle, rgba(255,255,255,0.3) 1px, transparent 1px)`,
                            backgroundSize: '30px 30px',
                        }}
                    />

                    {/* Floating Particles */}
                    <div className="absolute inset-0 overflow-hidden">
                        {CTA_PARTICLES.map((particle, i) => (
                            <div
                                key={i}
                                className="absolute w-1.5 h-1.5 bg-white/20 rounded-full animate-float"
                                style={{
                                    left: `${particle.left}%`,
                                    top: `${particle.top}%`,
                                    animationDelay: `${particle.delay}s`,
                                    animationDuration: `${particle.duration}s`,
                                }}
                            />
                        ))}
                    </div>

                    <div className="relative px-8 sm:px-16 py-20 sm:py-24 text-center">
                        {/* Badge */}
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/10 backdrop-blur-sm border border-white/20 mb-8 shadow-lg">
                            <Sparkles className="h-4 w-4 text-amber-400" />
                            <span className="text-sm font-semibold text-white/90">Start your journey today</span>
                        </div>

                        {/* Headline */}
                        <h2 className="text-4xl sm:text-5xl lg:text-6xl font-black text-white mb-6 tracking-tight leading-[1.1]">
                            Ready to take control of
                            <br />
                            <span className="bg-gradient-to-r from-blue-400 via-violet-400 to-fuchsia-400 bg-clip-text text-transparent">
                                your finances?
                            </span>
                        </h2>

                        <p className="text-lg sm:text-xl text-blue-100/70 max-w-2xl mx-auto mb-10 leading-relaxed">
                            Join thousands of users who have transformed their financial habits with SpendWise.
                            Start your journey to financial freedom today.
                        </p>

                        {/* CTA Buttons */}
                        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 mb-12">
                            <Button
                                size="lg"
                                className="group relative bg-white text-gray-900 hover:bg-gray-100 shadow-2xl shadow-black/20 px-10 py-7 text-base font-bold rounded-2xl transition-all duration-300 hover:scale-105 border border-white/50 overflow-hidden"
                            >
                                <span className="relative z-10 flex items-center gap-2">
                                    Start Tracking Free
                                    <ArrowRight className="h-5 w-5 group-hover:translate-x-1.5 transition-transform duration-300" />
                                </span>
                                <div className="absolute inset-0 bg-gradient-to-r from-gray-100 via-white to-gray-100 opacity-0 group-hover:opacity-100 transition-opacity" />
                            </Button>
                            <Button
                                variant="outline"
                                size="lg"
                                className="px-8 py-7 text-base font-semibold rounded-2xl bg-white/10 backdrop-blur-sm border-2 border-white/30 text-white hover:bg-white/20 hover:border-white/50 transition-all duration-300"
                            >
                                Schedule a Demo
                            </Button>
                        </div>

                        {/* Trust Indicators */}
                        <div className="flex flex-wrap items-center justify-center gap-6 pt-6 border-t border-white/10">
                            <div className="flex items-center gap-2 text-white/70">
                                <Check className="h-4 w-4 text-emerald-400" />
                                <span className="text-sm font-medium">No credit card required</span>
                            </div>
                            <div className="flex items-center gap-2 text-white/70">
                                <Shield className="h-4 w-4 text-blue-400" />
                                <span className="text-sm font-medium">Bank-level security</span>
                            </div>
                            <div className="flex items-center gap-2 text-white/70">
                                <Zap className="h-4 w-4 text-amber-400" />
                                <span className="text-sm font-medium">Free forever plan</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}
