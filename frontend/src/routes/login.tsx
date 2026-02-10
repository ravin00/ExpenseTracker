import { LoginForm } from '@/features/auth/components/LoginForm'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/login')({
  component: LoginForm,
})
