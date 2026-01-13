export enum TransactionType
{
  DESPESA= "DESPESA",
  RECEITA = "RECEITA"
}

export enum TransactionStatus
{
  PENDENTE = "PENDENTE",
  CONCLUIDA = "CONCLUIDA",
  ATRASADA = "ATRASADA",
  CANCELADA = "CANCELADA"
}

export type TransactionResponse = {
  id: string
  description: string
  amount: string
  isActive: boolean

  peopleId: string
  categoryId: string

  transactionType: TransactionType
  status: TransactionStatus
}

export type ApiTransactionResponse = {
  success: boolean
  message: string
  data?: TransactionResponse
}