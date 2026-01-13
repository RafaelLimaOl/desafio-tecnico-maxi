import { TransactionStatus, TransactionType } from "@/types/transactionResponse"
import z from "zod"

export const TransactionTypeEnum = z.nativeEnum(TransactionType)

export const TransactionStatusEnum = z.nativeEnum(TransactionStatus)

export const newTransactionSchema = z.object({
  description: z
    .string()
    .min(2, "A descrição deve ter mais de 2 caracteres")
    .max(200, "A descrição não pode passar de 200 caracteres"),

  amount: z.coerce.number().positive("O valor deve ser maior que zero"),
  peopleId: z
    .string("Pessoa inválida"),

  categoryId: z
    .string("Categoria inválida"),

  userId: z
    .string("Usuário inválido"),

  transactionType: TransactionTypeEnum,

})