// TODO: Implement useRegister hook
// This hook handles user registration mutation

export function useRegister() {
    // TODO: Implement register mutation
    return {
        register: async (_data: { email: string; password: string; username: string }) => { },
        isLoading: false,
        error: null,
    }
}
