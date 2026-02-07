import { useAuthStore } from '@/stores/auth-store'
import { useMutation } from '@tanstack/react-query'
import { toast } from 'sonner'
import { authApi } from '../services/auth.api'
import type { LoginDto, RegisterDto } from '../types'

export function useAuth() {
    const { login: setAuth, logout: clearAuth, user, isAuthenticated } = useAuthStore()


    const loginMutation = useMutation({
        mutationFn: (credentials: LoginDto) => authApi.login(credentials),
        onSuccess: (data: any) => {
            console.log('Login Response Data:', data) // DEBUG log

            // Handle Case Sensitivity (backend might return PascalCase)
            const token = data.token || data.Token
            const user = data.user || data.User

            console.log('Extracted:', { token, user }) // DEBUG log

            if (!token) {
                console.error('Login successful but no token found:', data)
                toast.error('Login failed: Server response invalid')
                return
            }

            setAuth(token, user)
            console.log('Auth State Updated via setAuth') // DEBUG log
            toast.success(`Welcome back, ${user?.username || user?.Username || 'User'}!`)
        },
        onError: (error: any) => {
            toast.error(error.message || 'Login failed')
        },
    })

    const registerMutation = useMutation({
        mutationFn: (data: RegisterDto) => authApi.register(data),
        onSuccess: () => {
            toast.success('Registration successful! Please login.')
        },
        onError: (error: any) => {
            toast.error(error.message || 'Registration failed')
        },
    })

    return {
        user,
        isAuthenticated,
        login: loginMutation.mutate,
        isLoggingIn: loginMutation.isPending,
        register: registerMutation.mutate,
        isRegistering: registerMutation.isPending,
        logout: clearAuth,
    }
}
