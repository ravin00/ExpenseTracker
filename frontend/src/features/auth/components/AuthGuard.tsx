import { useNavigate } from '@tanstack/react-router'
import type { ReactNode } from 'react'
import { useEffect } from 'react'
import { useAuth } from '../hooks/useAuth'

interface AuthGuardProps {
    children: ReactNode
}

export function AuthGuard({ children }: AuthGuardProps) {
    const { isAuthenticated } = useAuth()
    const navigate = useNavigate()

    useEffect(() => {
        if (!isAuthenticated) {
            navigate({ to: '/login' })
        }
    }, [isAuthenticated, navigate])

    if (!isAuthenticated) {
        return null // Don't render protected content while redirecting
    }

    return <>{children}</>
}
