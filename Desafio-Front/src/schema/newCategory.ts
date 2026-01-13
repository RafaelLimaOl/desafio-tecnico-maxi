import { CategoryType } from "@/types/categoryResponse"
import z from "zod"

export const categoryType = z.nativeEnum(CategoryType)

export const newCategorySchema = z.object({
  description: z.string().min(2, "O nome precisa de mais de 2 caracteres").max(100, "A descriçãoao não pode passar de 100 caracteres"),
  categoryType: categoryType
})