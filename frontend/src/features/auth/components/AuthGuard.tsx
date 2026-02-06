// TODO: Implement AuthGuard component
// This component protects routes that require authentication

import type { ReactNode } from 'react'

interface AuthGuardProps {
    children: ReactNode
}

export function AuthGuard({ children }: AuthGuardProps) {
    // TODO: Check authentication status and redirect if not authenticated
    return <>{children}</>
}
