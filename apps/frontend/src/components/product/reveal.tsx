import { cn } from '@/lib/utils'
import { type ReactNode, useEffect, useRef, useState } from 'react'

type RevealProps = {
  children: ReactNode
  className?: string
  delayMs?: number
  threshold?: number
}

export function Reveal({ children, className, delayMs = 0, threshold = 0.2 }: RevealProps) {
  const ref = useRef<HTMLDivElement>(null)
  const [isVisible, setIsVisible] = useState(false)

  useEffect(() => {
    const node = ref.current
    if (!node) return

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsVisible(true)
          observer.unobserve(entry.target)
        }
      },
      { threshold },
    )

    observer.observe(node)

    return () => {
      observer.disconnect()
    }
  }, [threshold])

  return (
    <div
      ref={ref}
      className={cn('motion-reveal', isVisible && 'is-visible', className)}
      style={{ transitionDelay: `${delayMs}ms` }}
    >
      {children}
    </div>
  )
}
