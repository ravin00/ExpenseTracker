import { useQuery } from '@tanstack/react-query'
import { analyticsSeries, budgetDetails, budgets, dashboardData, settingsData } from './mock-data'
import type { BudgetSummary } from './types'

type BudgetSort = 'name' | 'spent' | 'remaining' | 'updatedAt'

function wait(ms: number): Promise<void> {
  return new Promise((resolve) => setTimeout(resolve, ms))
}

export async function fetchDashboardData() {
  await wait(250)
  return dashboardData
}

export async function fetchBudgets(search: string, statusFilter: 'all' | 'on-track' | 'warning' | 'over', sortBy: BudgetSort) {
  await wait(250)
  const normalizedSearch = search.trim().toLowerCase()

  let items = budgets.filter((budget) => {
    if (statusFilter === 'all') {
      return true
    }
    return budget.status === statusFilter
  })

  if (normalizedSearch) {
    items = items.filter((budget) => {
      return (
        budget.name.toLowerCase().includes(normalizedSearch) ||
        budget.category.toLowerCase().includes(normalizedSearch) ||
        budget.owner.toLowerCase().includes(normalizedSearch)
      )
    })
  }

  const comparators: Record<BudgetSort, (left: BudgetSummary, right: BudgetSummary) => number> = {
    name: (left, right) => left.name.localeCompare(right.name),
    spent: (left, right) => right.spent - left.spent,
    remaining: (left, right) => right.remaining - left.remaining,
    updatedAt: (left, right) => new Date(right.updatedAt).getTime() - new Date(left.updatedAt).getTime(),
  }

  items.sort(comparators[sortBy])
  return items
}

export async function fetchBudgetDetail(id: string) {
  await wait(250)
  return budgetDetails[id] ?? null
}

export async function fetchAnalytics() {
  await wait(250)
  return analyticsSeries
}

export async function fetchSettings() {
  await wait(250)
  return settingsData
}

export function useDashboardQuery() {
  return useQuery({
    queryKey: ['dashboard-data'],
    queryFn: fetchDashboardData,
  })
}

export function useBudgetListQuery(search: string, statusFilter: 'all' | 'on-track' | 'warning' | 'over', sortBy: BudgetSort) {
  return useQuery({
    queryKey: ['budget-list', search, statusFilter, sortBy],
    queryFn: () => fetchBudgets(search, statusFilter, sortBy),
  })
}

export function useBudgetDetailQuery(id: string) {
  return useQuery({
    queryKey: ['budget-detail', id],
    queryFn: () => fetchBudgetDetail(id),
  })
}

export function useAnalyticsQuery() {
  return useQuery({
    queryKey: ['analytics-series'],
    queryFn: fetchAnalytics,
  })
}

export function useSettingsQuery() {
  return useQuery({
    queryKey: ['settings-data'],
    queryFn: fetchSettings,
  })
}
