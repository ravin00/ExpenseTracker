import { useAuthStore } from '@/stores/auth-store'
import { useMutation } from '@tanstack/react-query'
import { toast } from 'sonner'
import { authApi } from '../services/auth.api'
import type { AuthResponse, LoginDto, RegisterDto, User } from '../types'

type AuthResponseWithFallback = AuthResponse & {
    Token?: string
    User?: User
}

function getErrorMessage(error: unknown): string {
    if (error instanceof Error && error.message) {
        return error.message
    }

    return 'Request failed'
}

export function useAuth() {
    const { login: setAuth, logout: clearAuth, user, isAuthenticated } = useAuthStore()

    const loginMutation = useMutation({
        mutationFn: (credentials: LoginDto) => authApi.login(credentials),
        onSuccess: (data: AuthResponseWithFallback) => {
            const token = data.token ?? data.Token
            const authenticatedUser = data.user ?? data.User

            if (!token || !authenticatedUser) {
                toast.error('Login failed: Server response invalid')
                return
            }

            setAuth(token, authenticatedUser)
            toast.success(`Welcome back, ${authenticatedUser.username || 'User'}!`)
        },
        onError: (error: unknown) => {
            toast.error(getErrorMessage(error))
        },
    })

    const registerMutation = useMutation({
        mutationFn: (data: RegisterDto) => authApi.register(data),
        onSuccess: () => {
            toast.success('Registration successful! Please login.')
        },
        onError: (error: unknown) => {
            toast.error(getErrorMessage(error))
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
