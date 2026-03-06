import { LandingPage } from '@/features/product/pages/landing-page'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
  component: IndexRoute,
})

function IndexRoute() {
  return <LandingPage />
}
