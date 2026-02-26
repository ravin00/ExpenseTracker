// TODO: Implement AppLayout component
// Main application layout with sidebar and header

import type { ReactNode } from 'react'

interface AppLayoutProps {
    children: ReactNode
}

export function AppLayout({ children }: AppLayoutProps) {
    return (
        <div className="min-h-screen bg-gray-50 dark:bg-gray-950">
            {/* TODO: Add Sidebar, Header, and main content area */}
            <main className="p-4">
                {children}
            </main>
        </div>
    )
}
