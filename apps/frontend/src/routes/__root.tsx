import { AppShell } from '@/components/product'
import { useAuthStore } from '@/stores/auth-store'
import { Outlet, createRootRoute, useRouterState } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { useEffect } from 'react'

export const Route = createRootRoute({
  component: RootLayout,
})

const shellExcludedRoutes = new Set(['/', '/login', '/register'])

function RootLayout() {
  const pathname = useRouterState({
    select: (state) => state.location.pathname,
  })
  const checkTokenValidity = useAuthStore((state) => state.checkTokenValidity)

  useEffect(() => {
    checkTokenValidity()
  }, [pathname, checkTokenValidity])

  useEffect(() => {
    const interval = setInterval(() => {
      checkTokenValidity()
    }, 5 * 60 * 1000)

    return () => clearInterval(interval)
  }, [checkTokenValidity])

  const isShellExcludedRoute = shellExcludedRoutes.has(pathname)
  const animatedOutlet = (
    <div key={pathname} className="motion-page">
      <Outlet />
    </div>
  )

  return (
    <>
      {isShellExcludedRoute ? (
        animatedOutlet
      ) : (
        <AppShell>
          {animatedOutlet}
        </AppShell>
      )}
      <TanStackRouterDevtools />
    </>
  )
}
