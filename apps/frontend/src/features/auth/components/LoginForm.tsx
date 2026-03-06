import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { FormField } from '@/components/product'
import { zodResolver } from '@hookform/resolvers/zod'
import { Link, useNavigate } from '@tanstack/react-router'
import { Loader2, ShieldCheck, Wallet } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { useAuth } from '../hooks/useAuth'

const loginSchema = z.object({
  email: z.string().email('Enter a valid email address'),
  password: z.string().min(1, 'Password is required'),
})

type LoginFormData = z.infer<typeof loginSchema>

export function LoginForm() {
  const { login, isLoggingIn } = useAuth()
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  })

  const onSubmit = (data: LoginFormData) => {
    login(data, {
      onSuccess: () => {
        navigate({ to: '/dashboard' })
      },
    })
  }

  return (
    <div className="grid min-h-screen grid-cols-1 bg-background lg:grid-cols-2">
      <section className="relative hidden lg:block">
        <div className="absolute inset-0 bg-gradient-to-br from-primary/20 via-primary/5 to-transparent" />
        <div className="relative flex h-full flex-col justify-between p-10">
          <div className="flex items-center gap-2 text-foreground">
            <Wallet className="h-5 w-5 text-primary" />
            <span className="text-sm font-semibold tracking-wide">SpendWise</span>
          </div>
          <div className="max-w-md space-y-4">
            <h1 className="text-4xl font-semibold tracking-tight text-foreground">Control spend with clarity.</h1>
            <p className="text-sm text-muted-foreground">
              Unified budgets, alerts, and analytics in a clean workflow your team can trust.
            </p>
          </div>
        </div>
      </section>

      <section className="flex items-center justify-center p-6 sm:p-10">
        <Card className="w-full max-w-md border-border/80 bg-card/80 shadow-xl backdrop-blur">
          <CardHeader className="space-y-2">
            <div className="inline-flex h-10 w-10 items-center justify-center rounded-xl border border-border/80 bg-background text-primary">
              <ShieldCheck className="h-5 w-5" />
            </div>
            <CardTitle className="text-2xl">Sign in</CardTitle>
            <CardDescription>Access your budgeting workspace.</CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4" noValidate>
              <FormField id="email" label="Email" error={errors.email?.message}>
                <Input
                  id="email"
                  type="email"
                  placeholder="name@example.com"
                  disabled={isLoggingIn}
                  aria-invalid={Boolean(errors.email)}
                  {...register('email')}
                />
              </FormField>

              <FormField id="password" label="Password" error={errors.password?.message}>
                <Input
                  id="password"
                  type="password"
                  disabled={isLoggingIn}
                  aria-invalid={Boolean(errors.password)}
                  {...register('password')}
                />
              </FormField>

              <Button type="submit" className="w-full" disabled={isLoggingIn}>
                {isLoggingIn ? <Loader2 className="h-4 w-4 animate-spin" /> : null}
                Continue
              </Button>
            </form>
          </CardContent>
          <CardFooter className="justify-center text-sm text-muted-foreground">
            New to SpendWise?{' '}
            <Link to="/register" className="ml-1 font-medium text-primary hover:underline">
              Create account
            </Link>
          </CardFooter>
        </Card>
      </section>
    </div>
  )
}
