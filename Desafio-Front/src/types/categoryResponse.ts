export type CategoryResponse = {
  id: string
  description: string
  categoryType: string
  isActive: boolean
}

export type ApiCategoryResponse = {
  success: boolean
  message: string
  data?: CategoryResponse
}