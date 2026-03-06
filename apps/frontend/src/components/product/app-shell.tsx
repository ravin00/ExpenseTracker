import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Input } from '@/components/ui/input'
import { cn } from '@/lib/utils'
import { Link, useNavigate, useRouterState } from '@tanstack/react-router'
import { BarChart3, LayoutDashboard, LogOut, Menu, Search, Settings, Target, Wallet } from 'lucide-react'
import { type ReactNode, useMemo, useState } from 'react'
import { toast } from 'sonner'
import { ModeToggle } from '../mode-toggle'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { ConfirmDialog } from './confirm-dialog'
import { AppDrawer } from './modal-drawer'

type NavItem = {
  to: string
  label: string
  icon: typeof LayoutDashboard
}

const navItems: NavItem[] = [
  { to: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/budgets', label: 'Budgets', icon: Target },
  { to: '/analytics', label: 'Analytics', icon: BarChart3 },
  { to: '/profile', label: 'Settings', icon: Settings },
]

type AppShellProps = {
  children: ReactNode
}

function MainNav({ onNavigate }: { onNavigate?: () => void }) {
  const pathname = useRouterState({
    select: (state) => state.location.pathname,
  })

  return (
    <nav className="space-y-1">
      {navItems.map((item) => {
        const Icon = item.icon
        const isActive = pathname === item.to || pathname.startsWith(`${item.to}/`)

        return (
          <Link
            key={item.to}
            to={item.to}
            onClick={onNavigate}
            className={cn(
              'focus-ring flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors',
              isActive
                ? 'bg-primary/12 text-primary dark:bg-primary/20 dark:text-primary-foreground'
                : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground',
            )}
          >
            <Icon className="h-4 w-4" />
            <span>{item.label}</span>
          </Link>
        )
      })}
    </nav>
  )
}

export function AppShell({ children }: AppShellProps) {
  const { user, isAuthenticated, logout } = useAuth()
  const navigate = useNavigate()
  const [isMobileNavOpen, setMobileNavOpen] = useState(false)
  const [isLogoutDialogOpen, setLogoutDialogOpen] = useState(false)

  const initials = useMemo(() => {
    const source = user?.username?.trim() || 'SW'
    return source.slice(0, 2).toUpperCase()
  }, [user?.username])

  return (
    <div className="min-h-screen bg-background">
      <div className="mx-auto grid min-h-screen max-w-[1500px] grid-cols-1 md:grid-cols-[256px_minmax(0,1fr)]">
        <aside className="hidden border-r border-border/70 bg-card/60 p-5 md:block">
          <Link to="/dashboard" className="focus-ring mb-8 flex items-center gap-2 rounded-lg p-1">
            <span className="rounded-xl bg-primary/15 p-2 text-primary">
              <Wallet className="h-5 w-5" />
            </span>
            <div>
              <p className="text-sm font-semibold text-foreground">SpendWise</p>
              <p className="text-xs text-muted-foreground">Financial OS</p>
            </div>
          </Link>
          <MainNav />
        </aside>

        <div className="flex min-h-screen flex-col">
          <header className="sticky top-0 z-20 border-b border-border/70 bg-background/90 px-4 py-3 backdrop-blur sm:px-6">
            <div className="flex items-center justify-between gap-3">
              <div className="flex items-center gap-2 sm:gap-3">
                <Button
                  type="button"
                  variant="outline"
                  size="icon"
                  className="md:hidden"
                  onClick={() => setMobileNavOpen(true)}
                  aria-label="Open navigation"
                >
                  <Menu className="h-4 w-4" />
                </Button>
                <div className="relative hidden w-[260px] sm:block">
                  <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                  <Input placeholder="Search budgets, categories..." className="pl-9" />
                </div>
              </div>

              <div className="flex items-center gap-2">
                <ModeToggle />
                {isAuthenticated ? (
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="outline" className="gap-2 px-2 sm:px-3">
                        <span className="flex h-7 w-7 items-center justify-center rounded-full bg-primary/15 text-xs font-semibold text-primary">
                          {initials}
                        </span>
                        <span className="hidden text-sm sm:inline">{user?.username ?? 'User'}</span>
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end" className="w-52">
                      <DropdownMenuLabel>
                        <p className="text-sm font-medium">{user?.username ?? 'User'}</p>
                        <p className="text-xs font-normal text-muted-foreground">{user?.email ?? 'No email available'}</p>
                      </DropdownMenuLabel>
                      <DropdownMenuSeparator />
                      <DropdownMenuItem onClick={() => navigate({ to: '/profile' })}>
                        <Settings className="h-4 w-4" />
                        Settings
                      </DropdownMenuItem>
                      <DropdownMenuItem onClick={() => setLogoutDialogOpen(true)} className="text-destructive focus:text-destructive">
                        <LogOut className="h-4 w-4" />
                        Log out
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                ) : (
                  <Button onClick={() => navigate({ to: '/login' })}>Sign in</Button>
                )}
              </div>
            </div>
          </header>

          <main className="flex-1 px-4 py-6 sm:px-6">{children}</main>
        </div>
      </div>

      <AppDrawer
        open={isMobileNavOpen}
        onOpenChange={setMobileNavOpen}
        title="Navigation"
        description="Move between core product areas."
      >
        <div className="mt-5 px-1">
          <MainNav onNavigate={() => setMobileNavOpen(false)} />
        </div>
      </AppDrawer>

      <ConfirmDialog
        open={isLogoutDialogOpen}
        onOpenChange={setLogoutDialogOpen}
        title="Log out"
        description="You will need to sign in again to access your dashboard."
        confirmLabel="Log out"
        destructive
        onConfirm={() => {
          logout()
          toast.success('Signed out')
          navigate({ to: '/login' })
        }}
      />
    </div>
  )
}
