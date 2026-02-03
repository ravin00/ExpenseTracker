import { budgetApi } from '@/services/budget.service'
import { useBudgetStore } from '@/stores/budget-store'
import { useUIStore } from '@/stores/ui-store'
import { BudgetDto } from '@/types/budget.types'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from '@tanstack/react-router'
import { toast } from 'sonner'

export function useBudgetMutations() {
    const queryClient = useQueryClient()
    const navigate = useNavigate()
    const { setSelectedBudget } = useBudgetStore()
    const { closeCreateBudgetDialog, closeEditBudgetDialog, closeDeleteBudgetDialog } = useUIStore()

    const createBudget = useMutation({
        mutationFn: (data: BudgetDto) => budgetApi.create(data),
        onSuccess: (budget) => {
            queryClient.invalidateQueries({ queryKey: ['budgets'] })
            setSelectedBudget(budget)
            closeCreateBudgetDialog()
            toast.success('Budget created successfully')
            navigate({ to: '/budgets/$budgetId', params: { budgetId: String(budget.id) } })
        },
        onError: (error: Error) => {
            toast.error(error.message || 'Failed to create budget')
        },
    })

    const updateBudget = useMutation({
        mutationFn: ({ id, data }: { id: number; data: BudgetDto }) =>
            budgetApi.update(id, data),
        onSuccess: (budget, variables) => {
            queryClient.invalidateQueries({ queryKey: ['budgets'] })
            queryClient.invalidateQueries({ queryKey: ['budget', variables.id] })
            setSelectedBudget(budget)
            closeEditBudgetDialog()
            toast.success('Budget updated successfully')
        },
        onError: (error: Error) => {
            toast.error(error.message || 'Failed to update budget')
        },
    })

    const deleteBudget = useMutation({
        mutationFn: (id: number) => budgetApi.delete(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['budgets'] })
            setSelectedBudget(null)
            closeDeleteBudgetDialog()
            toast.success('Budget deleted successfully')
            navigate({ to: '/budgets' })
        },
        onError: (error: Error) => {
            toast.error(error.message || 'Failed to delete budget')
        },
    })

    return {
        createBudget,
        updateBudget,
        deleteBudget,
    }
}
