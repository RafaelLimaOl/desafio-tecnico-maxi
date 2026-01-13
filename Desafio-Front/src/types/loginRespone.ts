export type AuthResponse = {
  accessToken: string
  refreshToken: string
  userId: string
}

export type ApiAuthResponse = {
  success: boolean
  message: string
  data?: AuthResponse
}