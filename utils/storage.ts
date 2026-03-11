export const isClient = typeof window !== 'undefined'

export const safeLocalStorage = {
  getItem(key: string): string | null {
    if (isClient) {
      return localStorage.getItem(key)
    }
    return null
  },
  setItem(key: string, value: string): void {
    if (isClient) {
      localStorage.setItem(key, value)
    }
  },
  removeItem(key: string): void {
    if (isClient) {
      localStorage.removeItem(key)
    }
  }
}