import z from "zod"
import { categoryTypeEnum } from "./newCategory"

export const updateCategorySchema = z.object({
  description: z.string().min(2, "A descrição precisa de mais de 2 caracteres").max(100, "A descrição não pode passar de 150 caracteres"),
  categoryType: categoryTypeEnum,
  isActive: z.boolean()
})