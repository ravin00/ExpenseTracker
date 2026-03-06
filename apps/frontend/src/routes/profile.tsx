import { AuthGuard } from '@/features/auth/components/AuthGuard'
import { SettingsPage } from '@/features/product/pages/settings-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/profile')({
  component: ProfileRoute,
})

function ProfileRoute() {
  return (
    <AuthGuard>
      <SettingsPage />
    </AuthGuard>
  )
}
