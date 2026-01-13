import { TransactionStatus, TransactionType } from "@/types/transactionResponse"
import z from "zod"

export const TransactionTypeEnum = z.enum(TransactionType)

export const TransactionStatusEnum = z.enum(TransactionStatus)

export const updateTransactionSchema = z.object({
  description: z
    .string()
    .min(2, "A descrição deve ter mais de 2 caracteres")
    .max(200, "A descrição não pode passar de 200 caracteres"),

  amount: z.string(),

  peopleId: z
    .string("Pessoa inválida"),

  categoryId: z
    .string("Categoria inválida"),
  isActive: z.boolean(),

  transactionType: TransactionTypeEnum,
  status: TransactionStatusEnum
})