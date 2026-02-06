import {
    CTA,
    Features,
    Footer,
    Hero,
    HowItWorks,
    Navbar,
    Pricing,
    Stats,
    Testimonials,
} from '@/components/landing'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
    component: LandingPage,
})

function LandingPage() {
    return (
        <div className="min-h-screen bg-gradient-to-b from-white via-gray-50/30 to-white dark:from-gray-950 dark:via-gray-900/30 dark:to-gray-950 text-gray-900 dark:text-white overflow-x-hidden">
            <Navbar />
            <main>
                <Hero />
                <Features />
                <Stats />
                <HowItWorks />
                <Testimonials />
                <Pricing />
                <CTA />
            </main>
            <Footer />
        </div>
    )
}
