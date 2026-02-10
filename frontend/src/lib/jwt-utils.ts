interface JwtPayload {
    exp: number
    iat: number
    [key: string]: any
}

export function decodeJwt(token: string): JwtPayload | null {
    try {
        const base64Url = token.split('.')[1]
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        )
        return JSON.parse(jsonPayload)
    } catch (error) {
        console.error('Failed to decode JWT:', error)
        return null
    }
}

export function isTokenExpired(token: string): boolean {
    const payload = decodeJwt(token)
    if (!payload || !payload.exp) {
        // Only return true for expired tokens, not decode errors
        console.warn('Token decode failed or missing exp claim')
        return false // Don't log out on decode errors
    }

    // exp is in seconds, Date.now() is in milliseconds
    const expirationTime = payload.exp * 1000
    const currentTime = Date.now()

    // Reduce buffer to 5 seconds to prevent premature logout
    const isExpired = currentTime >= expirationTime - 5000

    if (isExpired) {
        console.log('Token expired at:', new Date(expirationTime))
    }

    return isExpired
}

export function isTokenValid(token: string): { valid: boolean; reason?: string } {
    if (!token) {
        return { valid: false, reason: 'No token provided' }
    }

    const payload = decodeJwt(token)
    if (!payload) {
        return { valid: false, reason: 'Failed to decode token' }
    }

    if (!payload.exp) {
        return { valid: false, reason: 'Token missing expiration claim' }
    }

    const expirationTime = payload.exp * 1000
    const currentTime = Date.now()

    if (currentTime >= expirationTime) {
        return { valid: false, reason: 'Token expired' }
    }

    return { valid: true }
}

export function getTokenExpirationTime(token: string): Date | null {
    const payload = decodeJwt(token)
    if (!payload || !payload.exp) {
        return null
    }
    return new Date(payload.exp * 1000)
}