import { PeopleRequest } from "@/types/peopleRequest"
import { ApiPeopleResponse } from "@/types/peopleResponse"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"
import { createNewPeople, deletePeople, getPeopleById, getPeopleByUser, updateSelectedPeople } from "../services/peopleService"

export const useGetAllPeople = () => {
  return useQuery({
    queryKey: ["people-get-all"],
    queryFn: () => getPeopleByUser(),

    staleTime: 1000 * 60 * 5, 
    gcTime: 1000 * 60 * 30,

    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,
  })
}

export const useGetPeopleById = (peopleId: string) => {
  return useQuery({
    queryKey: ["people-by-id"],
    queryFn: () => getPeopleById(peopleId),
    enabled: !!peopleId,

    staleTime: 1000 * 60 * 5,
  })
}

export const useCreatePeople = () => {
  const queryClient = useQueryClient()

  const createPeople = useMutation({
    mutationFn: createNewPeople,
    onSuccess: (data: ApiPeopleResponse) => {
      if (data.success) {
        toast.success("Pessoa cadastrada com sucesso!")
        queryClient.invalidateQueries({ queryKey: ["people-get-all"] })
      }
    },
    onError: () => {
      toast.error("Erro ao criar nova pessoa!")
    },
  })

  return { createPeople }
}


export const useUpdatePeople = () => {
  const queryClient = useQueryClient()

  const updatePeople = useMutation({
    mutationFn: ({ peopleId, request }: {
      peopleId: string
      request: PeopleRequest
    }) => updateSelectedPeople(peopleId, request),

    onSuccess: () => {
      toast.success("Pessoa editada com sucesso!")
      queryClient.invalidateQueries({ queryKey: ["people-get-all"] })
      queryClient.invalidateQueries({ queryKey: ["people-by-id"] })
    },
    onError: () => {
      toast.error("Erro ao atualizar as novas informações!")
    },
  })

  return { updatePeople }
}

export const useDeletePeople = () => {
  const queryClient = useQueryClient()

  const deleteSelectedPeople = useMutation({
    mutationFn: deletePeople,
    onSuccess: (data: ApiPeopleResponse) => {
      if (data.success) {
        toast.success("Pessoa deletada com sucesso!")
        queryClient.invalidateQueries({ queryKey: ["people-get-all"] })
      }
    },
    onError: () => {
      toast.error("Erro ao deletar a pessoa selecionada!")
    },
  })

  return { deleteSelectedPeople }
}

