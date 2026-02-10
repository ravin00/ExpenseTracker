import type { User } from '@/features/auth/types'
import { isTokenExpired } from '@/lib/jwt-utils'
import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface AuthState {
    token: string | null
    user: User | null
    isAuthenticated: boolean
    login: (token: string, user: User) => void
    logout: () => void
    updateUser: (user: Partial<User>) => void
    checkTokenValidity: () => void
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set, get) => ({
            token: null,
            user: null,
            isAuthenticated: false,

            login: (token: string, user: User) => {
                set({
                    token,
                    user,
                    isAuthenticated: true,
                })
            },

            logout: () => {
                console.log('Logging out user')
                set({
                    token: null,
                    user: null,
                    isAuthenticated: false,
                })
            },

            updateUser: (updatedUser: Partial<User>) => {
                const currentUser = get().user
                if (currentUser) {
                    set({
                        user: { ...currentUser, ...updatedUser },
                    })
                }
            },

            checkTokenValidity: () => {
                const { token } = get()
                if (!token) {
                    return // No token to validate
                }

                if (isTokenExpired(token)) {
                    console.log('Token expired, logging out')
                    get().logout()
                } else {
                    console.debug('Token is valid')
                }
            },
        }),
        {
            name: 'auth-storage',
            // Validate token on hydration
            onRehydrateStorage: () => (state) => {
                if (state?.token && isTokenExpired(state.token)) {
                    console.log('Stored token expired, clearing auth state')
                    state.logout()
                }
            },
        }
    )
)