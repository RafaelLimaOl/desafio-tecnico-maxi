import { DeleteTransactionRequest } from '@/types/deleteTransaction'
import { TransactionRequest } from '@/types/transactionRequest'
import { api } from './api'

export const getTransactionByUser = async () => {
  const response = await api.get(`/transaction/by-user`)
  return response.data
}
export const getTransactionByPeople = async (peopleId: string) => {
  const response = await api.get(`/transaction/by-people?peopleId=${peopleId}`)
  return response.data
}
export const getTransactionByCategory = async (categoryId: string) => {
  const response = await api.get(`/transaction/by-category?categoryId=${categoryId}`)
  return response.data
}

export const getTransactionById = async (transactionId: string) => {
  const response = await api.get(`/transaction/${transactionId}`)
  return response.data
}

export const updateSelectedTransaction = async (transactionId: string, request: TransactionRequest) => {
  const response = await api.put(`/transaction/${transactionId}`, request)
  return response.data
}

export const createNewTransaction = async (request: TransactionRequest) => {
  const response = await api.post("/transaction", request)
  return response.data
}

export const deleteTransaction = async ({ transactionId }: DeleteTransactionRequest) => {
  const response = await api.delete(`/transaction/${transactionId}`)
  return response.data
}
