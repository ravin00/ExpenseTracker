import { create } from 'zustand'

interface UIState {
    isSidebarOpen: boolean
    theme: 'light' | 'dark' | 'system'
    isCreateBudgetDialogOpen: boolean
    isEditBudgetDialogOpen: boolean
    isDeleteBudgetDialogOpen: boolean
    toggleSidebar: () => void
    setTheme: (theme: 'light' | 'dark' | 'system') => void
    openCreateBudgetDialog: () => void
    closeCreateBudgetDialog: () => void
    openEditBudgetDialog: () => void
    closeEditBudgetDialog: () => void
    openDeleteBudgetDialog: () => void
    closeDeleteBudgetDialog: () => void
}

export const useUIStore = create<UIState>((set) => ({
    isSidebarOpen: true,
    theme: 'system',
    isCreateBudgetDialogOpen: false,
    isEditBudgetDialogOpen: false,
    isDeleteBudgetDialogOpen: false,

    toggleSidebar: () => set((state) => ({ isSidebarOpen: !state.isSidebarOpen })),

    setTheme: (theme) => set({ theme }),

    openCreateBudgetDialog: () => set({ isCreateBudgetDialogOpen: true }),
    closeCreateBudgetDialog: () => set({ isCreateBudgetDialogOpen: false }),

    openEditBudgetDialog: () => set({ isEditBudgetDialogOpen: true }),
    closeEditBudgetDialog: () => set({ isEditBudgetDialogOpen: false }),

    openDeleteBudgetDialog: () => set({ isDeleteBudgetDialogOpen: true }),
    closeDeleteBudgetDialog: () => set({ isDeleteBudgetDialogOpen: false }),
}))
