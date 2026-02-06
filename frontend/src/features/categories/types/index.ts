// Category Types

export interface Category {
    id: number
    userId: number
    name: string
    description?: string
    color?: string
    icon?: string
    isActive: boolean
    createdAt: string
    updatedAt: string
}

export interface CategoryDto {
    name: string
    description?: string
    color?: string
    icon?: string
}

// Predefined category icons
export const CATEGORY_ICONS = [
    'shopping-cart', 'utensils', 'car', 'home', 'heartbeat',
    'graduation-cap', 'plane', 'gift', 'music', 'gamepad',
    'dumbbell', 'coffee', 'book', 'film', 'briefcase',
] as const

// Predefined category colors
export const CATEGORY_COLORS = [
    '#3B82F6', '#EF4444', '#10B981', '#F59E0B', '#8B5CF6',
    '#EC4899', '#06B6D4', '#84CC16', '#F97316', '#6366F1',
] as const
