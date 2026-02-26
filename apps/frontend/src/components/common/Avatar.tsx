// Avatar Component
interface AvatarProps {
    src?: string
    name?: string
    size?: 'sm' | 'md' | 'lg' | 'xl'
    className?: string
}

const sizeClasses = {
    sm: 'h-8 w-8 text-xs',
    md: 'h-10 w-10 text-sm',
    lg: 'h-12 w-12 text-base',
    xl: 'h-16 w-16 text-lg',
}

function getInitials(name: string): string {
    return name
        .split(' ')
        .map(n => n[0])
        .join('')
        .toUpperCase()
        .slice(0, 2)
}

function getColorFromName(name: string): string {
    const colors = [
        'bg-blue-500', 'bg-emerald-500', 'bg-violet-500',
        'bg-amber-500', 'bg-rose-500', 'bg-cyan-500',
    ]
    const index = name.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0)
    return colors[index % colors.length]
}

export function Avatar({ src, name = '', size = 'md', className = '' }: AvatarProps) {
    if (src) {
        return (
            <img
                src={src}
                alt={name}
                className={`rounded-full object-cover ${sizeClasses[size]} ${className}`}
            />
        )
    }

    return (
        <div
            className={`rounded-full flex items-center justify-center text-white font-semibold ${getColorFromName(name)} ${sizeClasses[size]} ${className}`}
        >
            {getInitials(name) || '?'}
        </div>
    )
}
