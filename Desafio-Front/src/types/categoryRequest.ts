type CategoryType = "DESPESA" | "RECEITA" | "AMBAS"

export type CategoryRequest = {
  description: string
  categoryType: CategoryType
  isActive?: boolean
}