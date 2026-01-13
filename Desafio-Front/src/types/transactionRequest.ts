import { TransactionStatus, TransactionType } from "./transactionResponse"

export type TransactionRequest = {
  description: string,
  amount: string,
  peopleId: string,
  categoryId: string,

  transactionType: TransactionType,
  status: TransactionStatus

  isActive?: boolean
}