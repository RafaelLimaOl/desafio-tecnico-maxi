export const saveTokens = (accessToken: string, refreshToken: string) => {
  if (typeof window === 'undefined') return

  localStorage.setItem('accessToken', accessToken)
  localStorage.setItem('refreshToken', refreshToken)
}

export const getAccessToken = () => {
  if (typeof window === 'undefined') return null
  return localStorage.getItem('accessToken')
}

export const getRefreshToken = () => {
  if (typeof window === 'undefined') return null
  return localStorage.getItem('refreshToken')
}

export const clearTokens = () => {
  if (typeof window === 'undefined') return

  localStorage.removeItem('accessToken')
  localStorage.removeItem('refreshToken')
}

export const isAuthenticated = () => {
  return !!getAccessToken()
}
