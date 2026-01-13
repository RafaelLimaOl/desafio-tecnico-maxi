import { deleteUser, getUserSettings, updateUserSettings } from "@/services/userSettingsService"
import { EditUserRequest } from "@/types/editUserRequest"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"
import { useAuth } from "./useAuth"

export const useGetUserSettings = () => {
  return useQuery({
    queryKey: ["settings"],
    queryFn: () => getUserSettings(),

    staleTime: 1000 * 60 * 5, 
    gcTime: 1000 * 60 * 30,

    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,
  })
}

export const useUpdateUserSettings = () => {
  const queryClient = useQueryClient()

  const updateUser = useMutation({
    mutationFn: ({ request }: { request: EditUserRequest}) =>
      updateUserSettings(request),

    onSuccess: () => {
      toast.success("Informações editadas com sucesso!")
      queryClient.invalidateQueries({ queryKey: ["settings"] })
    },
    onError: () => {
      toast.error("Erro ao atualizar as novas informações!")
    }
  })

  return { updateUser }
}

export const useDeleteUser = () => {
  const { logout } = useAuth()
  
  const deleteAccount = useMutation({
    mutationFn: deleteUser,
    onSuccess: (data) => {
      if (data.success) {
        toast.success("Conta deletada com sucesso!")
        logout()
      }
    },
    onError: () => {
      toast.error("Erro ao deletar a conta selecionada!")
    }
  })

  return { deleteAccount }
}