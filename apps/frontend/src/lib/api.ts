/// <reference types="vite/client" />
import { useAuthStore } from '@/stores/auth-store'
import { isTokenExpired } from './jwt-utils'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5100'

class ApiError extends Error {
    constructor(
        message: string,
        public status: number,
        public data?: unknown
    ) {
        super(message)
        this.name = 'ApiError'
    }
}

async function fetchWithAuth<T>(
    endpoint: string,
    options: RequestInit = {}
): Promise<T> {
    const token = useAuthStore.getState().token

    // Check if token is expired before making request
    if (token && isTokenExpired(token)) {
        console.log('Token expired before request, logging out')
        useAuthStore.getState().logout()
        window.location.href = '/login'
        throw new ApiError('Token expired', 401)
    }

    const config: RequestInit = {
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
            ...options.headers,
        },
    }

    const response = await fetch(`${API_BASE_URL}/api${endpoint}`, config)

    if (response.status === 401) {
        useAuthStore.getState().logout()
        window.location.href = '/login'
        throw new ApiError('Unauthorized', 401)
    }

    if (!response.ok) {
        const error = await response.json().catch(() => ({}))
        throw new ApiError(
            error.message || 'An error occurred',
            response.status,
            error
        )
    }

    if (response.status === 204) {
        return undefined as T
    }

    return response.json()
}

export const api = {
    get: <T>(endpoint: string) =>
        fetchWithAuth<T>(endpoint, { method: 'GET' }),

    post: <T>(endpoint: string, data?: unknown) =>
        fetchWithAuth<T>(endpoint, {
            method: 'POST',
            body: JSON.stringify(data),
        }),

    put: <T>(endpoint: string, data?: unknown) =>
        fetchWithAuth<T>(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data),
        }),

    delete: <T>(endpoint: string) =>
        fetchWithAuth<T>(endpoint, { method: 'DELETE' }),
}

export { ApiError }
