import { UserProfile } from '@/features/auth/components/UserProfile'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/profile')({
    component: UserProfile,
})
