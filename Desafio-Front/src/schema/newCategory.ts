import z from "zod"

export const categoryTypeEnum = z.enum(["DESPESA", "RECEITA", "AMBAS"])

export const newCategorySchema = z.object({
  description: z.string().min(2, "O nome precisa de mais de 2 caracteres").max(100, "A descriçãoao não pode passar de 100 caracteres"),
  categoryType: categoryTypeEnum
})