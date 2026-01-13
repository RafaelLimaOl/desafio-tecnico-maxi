import { createNewTransaction, deleteTransaction, getTransactionById, getTransactionByUser, updateSelectedTransaction } from "@/services/transactionService"
import { TransactionRequest } from "@/types/transactionRequest"
import { ApiTransactionResponse } from "@/types/transactionResponse"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { AxiosError } from "axios"
import { toast } from "sonner"

export const useGetAllTransactionByUser = () => {
  return useQuery({
    queryKey: ["transaction-get-user"],
    queryFn: () => getTransactionByUser(),

    staleTime: 1000 * 60 * 5, 
    gcTime: 1000 * 60 * 30,

    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,
  })
}

export const useGetTransactionById = (transactionId: string) => {
  return useQuery({
    queryKey: ["transaction-by-id", transactionId],
    queryFn: () => getTransactionById(transactionId),
    enabled: !!transactionId,

    staleTime: 1000 * 60 * 5,
  })
}

export const useUpdateTransaction = () => {
  const queryClient = useQueryClient()

  const updateTransaction = useMutation({
    mutationFn: ({ transactionId, request }: {transactionId: string, request: TransactionRequest}) =>
      updateSelectedTransaction(transactionId, request),

    onSuccess: () => {
      toast.success("Transação editada com sucesso!")
      queryClient.invalidateQueries({ queryKey: ["transaction-get-user"] })
      queryClient.invalidateQueries({ queryKey: ["transaction-by-id"] })
    },
    onError: () => {
      toast.error("Erro ao atualizar as novas informações!")
    }
  })

  return { updateTransaction }
}

export const useCreateTransaction = () => {
  const queryClient = useQueryClient()
  const createTransaction = useMutation({
    mutationFn: createNewTransaction,
    onSuccess: (data: ApiTransactionResponse) => {
      if (data.success) {
        toast.success("Transação cadastrada com sucesso!")
      }
      queryClient.invalidateQueries({
        queryKey: ["transaction-get-user"]
      })
    },
    onError: (error: AxiosError<{ message: string }>) => {
      const apiMessage = error.response?.data?.message
      toast.error(apiMessage ?? "Erro ao criar nova transação")
    }
  })

  return { createTransaction }
}

export const useDeleteTransaction = () => {
  const queryClient = useQueryClient()
  const deleteSelectedTransaction = useMutation({
    mutationFn: deleteTransaction,
    onSuccess: (data: ApiTransactionResponse) => {
      if (data.success) {
          toast.success("Transação deletada com sucesso!")
      }
      queryClient.invalidateQueries({
        queryKey: ["transaction-get-user"]
      })
    },
    onError: () => {
        toast.error("Erro ao deletar a transação selecionada!")
    }
  })

  return { deleteSelectedTransaction }
}
