import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { zodResolver } from '@hookform/resolvers/zod'
import { Link, useNavigate } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { useAuth } from '../hooks/useAuth'

const registerSchema = z.object({
    username: z
        .string()
        .min(3, 'Username must be at least 3 characters')
        .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),
    email: z.string().email('Please enter a valid email address'),
    password: z
        .string()
        .min(8, 'Password must be at least 8 characters')
        .regex(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
            'Password must contain uppercase, lowercase, number and special char'
        ),
    // confirmPassword: z.string() // Add confirmation if needed, but keeping simple for now
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
                navigate({ to: '/login' }) // Redirect to login after registration
            },
        })
    }

    return (
        <div className="flex items-center justify-center min-h-screen px-4 bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50 dark:from-gray-900 dark:via-blue-900/20 dark:to-purple-900/20">
            <Card className="w-full max-w-md shadow-2xl border-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-xl">
                <CardHeader className="space-y-2 pb-6">
                    <CardTitle className="text-3xl font-bold text-center bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
                        Create an account
                    </CardTitle>
                    <CardDescription className="text-center text-base">
                        Enter your information to create your account
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
                        <div className="space-y-2">
                            <Label htmlFor="username" className="text-sm font-semibold">Username</Label>
                            <Input
                                id="username"
                                placeholder="johndoe"
                                {...register('username')}
                                disabled={isRegistering}
                                className="h-11 transition-all duration-200 focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.username && (
                                <p className="text-sm text-red-600 dark:text-red-400 font-medium">
                                    {errors.username.message}
                                </p>
                            )}
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="email" className="text-sm font-semibold">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="name@example.com"
                                {...register('email')}
                                disabled={isRegistering}
                                className="h-11 transition-all duration-200 focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.email && (
                                <p className="text-sm text-red-600 dark:text-red-400 font-medium">
                                    {errors.email.message}
                                </p>
                            )}
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="password" className="text-sm font-semibold">Password</Label>
                            <Input
                                id="password"
                                type="password"
                                {...register('password')}
                                disabled={isRegistering}
                                className="h-11 transition-all duration-200 focus:ring-2 focus:ring-blue-500"
                            />
                            {errors.password && (
                                <p className="text-sm text-red-600 dark:text-red-400 font-medium">
                                    {errors.password.message}
                                </p>
                            )}
                        </div>
                        <Button
                            type="submit"
                            className="w-full h-11 bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 shadow-lg hover:shadow-xl transition-all duration-200"
                            disabled={isRegistering}
                        >
                            {isRegistering && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            Create Account
                        </Button>
                    </form>
                </CardContent>
                <CardFooter className="flex flex-col space-y-2 text-center text-sm text-muted-foreground pt-6">
                    <div>
                        Already have an account?{' '}
                        <Link to="/login" className="font-semibold text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 transition-colors">
                            Sign in
                        </Link>
                    </div>
                </CardFooter>
            </Card>
        </div>
    )
}
