import { ModeToggle } from '@/components/mode-toggle'
import { Reveal } from '@/components/product'
import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { Link } from '@tanstack/react-router'
import {
  ArrowRight,
  BarChart3,
  BellRing,
  Clock3,
  GitBranch,
  NotebookTabs,
  ShieldCheck,
  Sparkles,
  Target,
  Wallet,
} from 'lucide-react'

const navLinks = [
  { label: 'Features', href: '#features' },
  { label: 'Workflow', href: '#workflow' },
  { label: 'Pricing', href: '#pricing' },
  { label: 'FAQ', href: '#faq' },
]

const featureCards = [
  {
    title: 'Budget lifecycle control',
    description: 'Define limits, track usage, and close loops with clear month-end outcomes.',
    icon: Target,
  },
  {
    title: 'Live risk visibility',
    description: 'Detect overrun trends early and act before budgets break.',
    icon: BellRing,
  },
  {
    title: 'Analytics that explain behavior',
    description: 'Trend and category breakdowns focused on decisions, not noise.',
    icon: BarChart3,
  },
  {
    title: 'Recurring expense tracking',
    description: 'Track subscriptions and repeating payments without manual re-entry.',
    icon: Clock3,
  },
  {
    title: 'Category planning',
    description: 'Organize spend by lifestyle, essentials, savings, and custom categories.',
    icon: Target,
  },
  {
    title: 'Savings goal alignment',
    description: 'Connect budgets with savings goals to stay consistent month over month.',
    icon: ShieldCheck,
  },
]

const workflowSteps = [
  {
    title: 'Plan',
    detail: 'Create monthly budgets by category and assign ownership.',
    icon: NotebookTabs,
  },
  {
    title: 'Track',
    detail: 'Capture spending and monitor burn-rate against limits.',
    icon: Clock3,
  },
  {
    title: 'Intervene',
    detail: 'Use alerts and insights to adjust budgets before overrun.',
    icon: ShieldCheck,
  },
  {
    title: 'Review',
    detail: 'Close each cycle with trend analysis and carry-forward decisions.',
    icon: GitBranch,
  },
]

const faqs = [
  {
    question: 'Is SpendWise only for personal budgeting?',
    answer: 'No. It works for individuals, student teams, and small product teams that need clear budget governance.',
  },
  {
    question: 'Can I track both monthly and weekly budgets?',
    answer: 'Yes. SpendWise supports budget periods so you can plan by week, month, or longer cycles.',
  },
  {
    question: 'Does SpendWise help prevent overspending?',
    answer: 'Yes. You get clear alerts and visual progress indicators when spending approaches budget limits.',
  },
]

export function LandingPage() {
  const { isAuthenticated } = useAuth()

  return (
    <div className="min-h-screen bg-background">
      <header className="sticky top-0 z-30 border-b border-border/70 bg-background/90 backdrop-blur">
        <div className="mx-auto flex w-full max-w-6xl items-center justify-between gap-4 px-4 py-4 sm:px-6">
          <Link to="/" className="focus-ring flex items-center gap-2 rounded-lg p-1">
            <span className="rounded-xl bg-primary/15 p-2 text-primary">
              <Wallet className="h-5 w-5" />
            </span>
            <div>
              <p className="text-sm font-semibold text-foreground">SpendWise</p>
              <p className="text-xs text-muted-foreground">Financial Platform</p>
            </div>
          </Link>

          <nav aria-label="Landing sections" className="hidden items-center gap-5 md:flex">
            {navLinks.map((link) => (
              <a key={link.href} href={link.href} className="focus-ring rounded-md px-1 py-1 text-sm text-muted-foreground hover:text-foreground">
                {link.label}
              </a>
            ))}
          </nav>

          <div className="flex items-center gap-2">
            <ModeToggle />
            <Button variant="outline" asChild>
              <Link to="/login">Sign in</Link>
            </Button>
            <Button asChild>
              <Link to={isAuthenticated ? '/dashboard' : '/register'}>
                {isAuthenticated ? 'Open App' : 'Get Started'}
              </Link>
            </Button>
          </div>
        </div>
      </header>

      <main className="mx-auto w-full max-w-6xl px-4 pb-20 pt-12 sm:px-6 sm:pb-24 sm:pt-16">
        <Reveal>
          <section id="hero" className="scroll-mt-24 grid gap-10 lg:grid-cols-[1.1fr_0.9fr] lg:items-center">
          <div className="space-y-6">
            <p className="inline-flex items-center gap-2 rounded-full border border-border/80 bg-card px-3 py-1 text-xs font-medium text-muted-foreground">
              <Sparkles className="h-3.5 w-3.5" />
              Designed to make everyday budgeting simple and consistent
            </p>
            <h1 className="text-display text-foreground sm:text-5xl">A scrolling product-grade budgeting experience from first load.</h1>
            <p className="max-w-xl text-body">
              SpendWise gives teams a clean command center for planning spend, detecting risk, and making confident month-end decisions.
            </p>
            <div className="flex flex-wrap gap-3">
              <Button size="lg" asChild>
                <Link to={isAuthenticated ? '/dashboard' : '/register'}>
                  {isAuthenticated ? 'Go to Dashboard' : 'Create account'}
                  <ArrowRight className="h-4 w-4" />
                </Link>
              </Button>
              <Button size="lg" variant="outline" asChild>
                <a href="#features">Explore features</a>
              </Button>
            </div>
          </div>

          <Card className="motion-card surface-elevated border-border/80 bg-card/80">
            <CardContent className="grid gap-3 p-5 text-sm">
              <div className="rounded-xl border border-border/80 bg-background/80 p-4">
                <p className="text-muted-foreground">Monthly spend</p>
                <p className="mt-1 text-2xl font-semibold text-foreground">$2,290</p>
              </div>
              <div className="rounded-xl border border-border/80 bg-background/80 p-4">
                <p className="text-muted-foreground">Budgets at risk</p>
                <p className="mt-1 text-2xl font-semibold text-foreground">2 / 14</p>
              </div>
              <div className="rounded-xl border border-border/80 bg-background/80 p-4">
                <p className="text-muted-foreground">Projected savings</p>
                <p className="mt-1 text-2xl font-semibold text-foreground">$1,180</p>
              </div>
            </CardContent>
          </Card>
          </section>
        </Reveal>

        <Reveal delayMs={60}>
          <section className="scroll-mt-24 mt-14 grid gap-4 sm:grid-cols-3">
          <article className="motion-card surface rounded-2xl p-4">
            <p className="text-xs uppercase tracking-wide text-muted-foreground">Monthly planning</p>
            <p className="mt-2 text-lg font-semibold text-foreground">Set clear limits per category before spending starts</p>
          </article>
          <article className="motion-card surface rounded-2xl p-4">
            <p className="text-xs uppercase tracking-wide text-muted-foreground">Smart alerts</p>
            <p className="mt-2 text-lg font-semibold text-foreground">Get warnings before budgets are exceeded</p>
          </article>
          <article className="motion-card surface rounded-2xl p-4">
            <p className="text-xs uppercase tracking-wide text-muted-foreground">Goal progress</p>
            <p className="mt-2 text-lg font-semibold text-foreground">Track savings outcomes alongside daily expenses</p>
          </article>
          </section>
        </Reveal>

        <Reveal delayMs={80}>
          <section id="features" className="scroll-mt-24 mt-16 space-y-6">
          <div>
            <h2 className="text-heading text-foreground">Core product capabilities</h2>
            <p className="mt-2 text-body">Everything needed for a complete budget management cycle with clear ownership and accountability.</p>
          </div>
          <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
            {featureCards.map((feature, index) => {
              const Icon = feature.icon
              return (
                <Reveal key={feature.title} delayMs={index * 50}>
                  <article className="motion-card surface rounded-2xl p-5">
                  <span className="inline-flex rounded-lg border border-border/70 bg-background p-2 text-primary">
                    <Icon className="h-4 w-4" />
                  </span>
                  <h3 className="mt-4 text-lg font-semibold text-foreground">{feature.title}</h3>
                  <p className="mt-2 text-sm text-muted-foreground">{feature.description}</p>
                  </article>
                </Reveal>
              )
            })}
          </div>
          </section>
        </Reveal>

        <Reveal delayMs={90}>
          <section id="workflow" className="scroll-mt-24 mt-16 space-y-6">
          <div>
            <h2 className="text-heading text-foreground">How the workflow runs</h2>
            <p className="mt-2 text-body">A practical loop that keeps budgeting simple and measurable.</p>
          </div>
          <div className="grid gap-4 md:grid-cols-2">
            {workflowSteps.map((step, index) => {
              const Icon = step.icon
              return (
                <Reveal key={step.title} delayMs={index * 60}>
                  <article className="motion-card surface rounded-2xl p-5">
                  <div className="flex items-center gap-3">
                    <span className="inline-flex h-8 w-8 items-center justify-center rounded-full border border-border bg-background text-xs font-semibold text-muted-foreground">
                      {index + 1}
                    </span>
                    <span className="inline-flex rounded-lg border border-border/70 bg-background p-2 text-primary">
                      <Icon className="h-4 w-4" />
                    </span>
                    <h3 className="text-base font-semibold text-foreground">{step.title}</h3>
                  </div>
                  <p className="mt-3 text-sm text-muted-foreground">{step.detail}</p>
                  </article>
                </Reveal>
              )
            })}
          </div>
          </section>
        </Reveal>

        <Reveal delayMs={100}>
          <section id="pricing" className="scroll-mt-24 mt-16 space-y-6">
          <div>
            <h2 className="text-heading text-foreground">Simple pricing</h2>
            <p className="mt-2 text-body">Start free and upgrade when you need more budget coverage and deeper insights.</p>
          </div>
          <div className="grid gap-4 md:grid-cols-2">
            <article className="motion-card surface rounded-2xl p-6">
              <p className="text-xs uppercase tracking-wide text-muted-foreground">Starter</p>
              <p className="mt-3 text-3xl font-semibold text-foreground">Free</p>
              <ul className="mt-4 space-y-2 text-sm text-muted-foreground">
                <li>Up to 5 active budgets</li>
                <li>Core dashboard and analytics</li>
                <li>Email alerts</li>
              </ul>
            </article>
            <article className="motion-card surface-elevated rounded-2xl border-primary/30 p-6">
              <p className="text-xs uppercase tracking-wide text-primary">Pro</p>
              <p className="mt-3 text-3xl font-semibold text-foreground">$9 / month</p>
              <ul className="mt-4 space-y-2 text-sm text-muted-foreground">
                <li>Unlimited budgets and categories</li>
                <li>Advanced analytics views</li>
                <li>Audit trail and export-ready reports</li>
              </ul>
            </article>
          </div>
          </section>
        </Reveal>

        <Reveal delayMs={110}>
          <section id="faq" className="scroll-mt-24 mt-16 space-y-4">
          <h2 className="text-heading text-foreground">Frequently asked questions</h2>
          <div className="space-y-3">
            {faqs.map((faq, index) => (
              <Reveal key={faq.question} delayMs={index * 50}>
                <article className="motion-card surface rounded-2xl p-5">
                <h3 className="text-base font-semibold text-foreground">{faq.question}</h3>
                <p className="mt-2 text-sm text-muted-foreground">{faq.answer}</p>
                </article>
              </Reveal>
            ))}
          </div>
          </section>
        </Reveal>

        <Reveal delayMs={120}>
          <section className="mt-16">
          <Card className="motion-card surface-elevated border-primary/25 bg-primary/[0.06]">
            <CardContent className="flex flex-col gap-4 p-6 sm:flex-row sm:items-center sm:justify-between">
              <div>
                <h2 className="text-xl font-semibold text-foreground">Ready to run SpendWise?</h2>
                <p className="mt-2 text-sm text-muted-foreground">
                  Start with the live app experience and move directly into dashboard workflows.
                </p>
              </div>
              <div className="flex gap-2">
                <Button variant="outline" asChild>
                  <Link to="/login">Sign in</Link>
                </Button>
                <Button asChild>
                  <Link to={isAuthenticated ? '/dashboard' : '/register'}>
                    {isAuthenticated ? 'Open Dashboard' : 'Create account'}
                  </Link>
                </Button>
              </div>
            </CardContent>
          </Card>
          </section>
        </Reveal>
      </main>

      <footer className="border-t border-border/70">
        <div className="mx-auto flex w-full max-w-6xl flex-col gap-3 px-4 py-6 text-sm text-muted-foreground sm:flex-row sm:items-center sm:justify-between sm:px-6">
          <p>SpendWise</p>
          <p>Plan better, spend smarter, and grow your savings with confidence.</p>
        </div>
      </footer>
    </div>
  )
}
