export type BudgetStatus = 'on-track' | 'warning' | 'over'

export type KpiMetric = {
  title: string
  value: string
  trendLabel: string
  trendDelta: number
}

export type BudgetSummary = {
  id: string
  name: string
  category: string
  owner: string
  spent: number
  limit: number
  remaining: number
  status: BudgetStatus
  updatedAt: string
}

export type BudgetActivity = {
  id: string
  type: 'created' | 'edited' | 'alert' | 'comment'
  message: string
  createdAt: string
  actor: string
}

export type BudgetDetail = BudgetSummary & {
  notes: string
  period: 'weekly' | 'monthly' | 'quarterly'
  activity: BudgetActivity[]
}

export type DashboardData = {
  kpis: KpiMetric[]
  upcomingActions: Array<{
    id: string
    label: string
    description: string
  }>
  recentBudgets: BudgetSummary[]
}

export type AnalyticsPoint = {
  label: string
  amount: number
}

export type SettingsData = {
  profile: {
    fullName: string
    email: string
    timezone: string
    currency: string
  }
  preferences: {
    weeklyDigest: boolean
    budgetAlerts: boolean
    marketingEmails: boolean
  }
}
