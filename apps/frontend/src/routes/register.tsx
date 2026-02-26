import { RegisterForm } from '@/features/auth/components/RegisterForm'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/register')({
  component: RegisterForm,
})
