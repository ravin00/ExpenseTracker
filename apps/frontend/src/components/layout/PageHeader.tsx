// TODO: Implement PageHeader component
interface PageHeaderProps {
    title: string
    description?: string
    children?: React.ReactNode
}

export function PageHeader({ title, description, children }: PageHeaderProps) {
    return (
        <div className="flex items-center justify-between mb-8">
            <div>
                <h1 className="text-2xl font-bold text-gray-900 dark:text-white">{title}</h1>
                {description && (
                    <p className="text-gray-500 dark:text-gray-400 mt-1">{description}</p>
                )}
            </div>
            {children && <div className="flex items-center gap-3">{children}</div>}
        </div>
    )
}
