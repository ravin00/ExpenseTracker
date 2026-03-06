import type { AnalyticsPoint, BudgetDetail, BudgetStatus, BudgetSummary, DashboardData, SettingsData } from './types'

const now = new Date('2026-03-02T09:00:00Z')

function isoDaysAgo(days: number): string {
  const date = new Date(now)
  date.setDate(date.getDate() - days)
  return date.toISOString()
}

function statusFromRatio(ratio: number): BudgetStatus {
  if (ratio >= 1) return 'over'
  if (ratio >= 0.85) return 'warning'
  return 'on-track'
}

const baseBudgets: Array<Omit<BudgetSummary, 'remaining' | 'status'>> = [
  {
    id: 'bgt-101',
    name: 'Household Essentials',
    category: 'Home',
    owner: 'Ravin',
    spent: 820,
    limit: 1200,
    updatedAt: isoDaysAgo(1),
  },
  {
    id: 'bgt-102',
    name: 'Transportation',
    category: 'Commute',
    owner: 'Ravin',
    spent: 540,
    limit: 600,
    updatedAt: isoDaysAgo(0),
  },
  {
    id: 'bgt-103',
    name: 'Growth & Learning',
    category: 'Education',
    owner: 'Ravin',
    spent: 240,
    limit: 450,
    updatedAt: isoDaysAgo(2),
  },
  {
    id: 'bgt-104',
    name: 'Leisure & Dining',
    category: 'Lifestyle',
    owner: 'Ravin',
    spent: 690,
    limit: 650,
    updatedAt: isoDaysAgo(0),
  },
]

export const budgets: BudgetSummary[] = baseBudgets.map((budget) => {
  const remaining = Math.max(budget.limit - budget.spent, 0)
  const status = statusFromRatio(budget.spent / budget.limit)
  return {
    ...budget,
    remaining,
    status,
  }
})

export const dashboardData: DashboardData = {
  kpis: [
    {
      title: 'Monthly Spend',
      value: '$2,290',
      trendLabel: 'vs last month',
      trendDelta: -8.2,
    },
    {
      title: 'Active Budgets',
      value: '4',
      trendLabel: 'stable coverage',
      trendDelta: 0,
    },
    {
      title: 'At Risk Budgets',
      value: '2',
      trendLabel: 'requires action',
      trendDelta: 12,
    },
    {
      title: 'Projected Savings',
      value: '$1,180',
      trendLabel: 'this quarter',
      trendDelta: 5.8,
    },
  ],
  upcomingActions: [
    {
      id: 'act-1',
      label: 'Review Transportation cap',
      description: 'Budget is above 90% with 8 days left in cycle.',
    },
    {
      id: 'act-2',
      label: 'Approve category rollover',
      description: 'Move remaining Education balance to Savings Goal.',
    },
    {
      id: 'act-3',
      label: 'Update recurring subscriptions',
      description: 'Two annual subscriptions renew this week.',
    },
  ],
  recentBudgets: budgets,
}

export const budgetDetails: Record<string, BudgetDetail> = {
  'bgt-101': {
    ...budgets[0],
    notes: 'Focus on reducing grocery variance by batching purchases weekly.',
    period: 'monthly',
    activity: [
      {
        id: 'ev-101-1',
        type: 'edited',
        message: 'Adjusted budget limit from $1,100 to $1,200.',
        createdAt: isoDaysAgo(5),
        actor: 'Ravin',
      },
      {
        id: 'ev-101-2',
        type: 'comment',
        message: 'Tracking pantry bulk orders separately this month.',
        createdAt: isoDaysAgo(3),
        actor: 'Ravin',
      },
    ],
  },
  'bgt-102': {
    ...budgets[1],
    notes: 'Ride-share spike due to travel week. Monitor next cycle.',
    period: 'monthly',
    activity: [
      {
        id: 'ev-102-1',
        type: 'alert',
        message: 'Spend reached 90% threshold.',
        createdAt: isoDaysAgo(0),
        actor: 'System',
      },
      {
        id: 'ev-102-2',
        type: 'edited',
        message: 'Changed category mapping from General to Commute.',
        createdAt: isoDaysAgo(6),
        actor: 'Ravin',
      },
    ],
  },
  'bgt-103': {
    ...budgets[2],
    notes: 'Healthy trend. Candidate for quarterly allocation increase.',
    period: 'quarterly',
    activity: [
      {
        id: 'ev-103-1',
        type: 'created',
        message: 'Budget created for certification track.',
        createdAt: isoDaysAgo(16),
        actor: 'Ravin',
      },
      {
        id: 'ev-103-2',
        type: 'comment',
        message: 'Add conference pass as one-time expense.',
        createdAt: isoDaysAgo(2),
        actor: 'Ravin',
      },
    ],
  },
  'bgt-104': {
    ...budgets[3],
    notes: 'Exceeded due to family event. Consider temporary exception policy.',
    period: 'monthly',
    activity: [
      {
        id: 'ev-104-1',
        type: 'alert',
        message: 'Budget exceeded by $40.',
        createdAt: isoDaysAgo(0),
        actor: 'System',
      },
      {
        id: 'ev-104-2',
        type: 'edited',
        message: 'Added additional expense categories for dining events.',
        createdAt: isoDaysAgo(4),
        actor: 'Ravin',
      },
    ],
  },
}

export const analyticsSeries: AnalyticsPoint[] = [
  { label: 'Week 1', amount: 620 },
  { label: 'Week 2', amount: 540 },
  { label: 'Week 3', amount: 610 },
  { label: 'Week 4', amount: 520 },
]

export const settingsData: SettingsData = {
  profile: {
    fullName: 'Ravin Bandara',
    email: 'ravin@example.com',
    timezone: 'America/New_York',
    currency: 'USD',
  },
  preferences: {
    weeklyDigest: true,
    budgetAlerts: true,
    marketingEmails: false,
  },
}
