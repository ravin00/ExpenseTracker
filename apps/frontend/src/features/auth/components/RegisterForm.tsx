import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { FormField } from '@/components/product'
import { zodResolver } from '@hookform/resolvers/zod'
import { Link, useNavigate } from '@tanstack/react-router'
import { Loader2, UserPlus } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { useAuth } from '../hooks/useAuth'

const registerSchema = z.object({
  username: z
    .string()
    .min(3, 'Username must be at least 3 characters')
    .regex(/^[a-zA-Z0-9_]+$/, 'Use letters, numbers, and underscores only'),
  email: z.string().email('Enter a valid email address'),
  password: z
    .string()
    .min(8, 'Password must be at least 8 characters')
    .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$/, 'Include upper/lowercase, number, and symbol'),
})

type RegisterFormData = z.infer<typeof registerSchema>

export function RegisterForm() {
  const { register: signup, isRegistering } = useAuth()
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  })

  const onSubmit = (data: RegisterFormData) => {
    signup(data, {
      onSuccess: () => {
        navigate({ to: '/login' })
      },
    })
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-background p-6 sm:p-10">
      <Card className="w-full max-w-md border-border/80 bg-card/80 shadow-xl backdrop-blur">
        <CardHeader className="space-y-2">
          <div className="inline-flex h-10 w-10 items-center justify-center rounded-xl border border-border/80 bg-background text-primary">
            <UserPlus className="h-5 w-5" />
          </div>
          <CardTitle className="text-2xl">Create account</CardTitle>
          <CardDescription>Set up your workspace in under one minute.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4" noValidate>
            <FormField id="username" label="Username" error={errors.username?.message}>
              <Input
                id="username"
                placeholder="ravin"
                disabled={isRegistering}
                aria-invalid={Boolean(errors.username)}
                {...register('username')}
              />
            </FormField>

            <FormField id="email" label="Email" error={errors.email?.message}>
              <Input
                id="email"
                type="email"
                placeholder="name@example.com"
                disabled={isRegistering}
                aria-invalid={Boolean(errors.email)}
                {...register('email')}
              />
            </FormField>

            <FormField
              id="password"
              label="Password"
              hint="At least 8 chars with uppercase, lowercase, number, and symbol."
              error={errors.password?.message}
            >
              <Input
                id="password"
                type="password"
                disabled={isRegistering}
                aria-invalid={Boolean(errors.password)}
                {...register('password')}
              />
            </FormField>

            <Button type="submit" className="w-full" disabled={isRegistering}>
              {isRegistering ? <Loader2 className="h-4 w-4 animate-spin" /> : null}
              Create account
            </Button>
          </form>
        </CardContent>
        <CardFooter className="justify-center text-sm text-muted-foreground">
          Already have an account?{' '}
          <Link to="/login" className="ml-1 font-medium text-primary hover:underline">
            Sign in
          </Link>
        </CardFooter>
      </Card>
    </div>
  )
}
