import { UserSettingsRequest } from "@/types/settingsRequest"
import { api } from "./api"

export const getUserSettings = async () => {
  const response = await api.get(`/settings`)
  return response.data
}

export const updateUserSettings = async (request: UserSettingsRequest) => {
  const response = await api.put(`/settings`, request)
  return response.data
}

export const deleteUser = async() => {
  const response = await api.delete(`/settings`)
  return response.data
}