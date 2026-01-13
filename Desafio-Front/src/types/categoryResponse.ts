export enum CategoryType
{
  AMBAS = "AMBAS",
  DESPESA = "DESPESA",
  RECEITA = "RECEITA"
}

export type CategoryResponse = {
  id: string
  description: string
  categoryType: CategoryType
  isActive: boolean
}

export type ApiCategoryResponse = {
  success: boolean
  message: string
  data?: CategoryResponse
}