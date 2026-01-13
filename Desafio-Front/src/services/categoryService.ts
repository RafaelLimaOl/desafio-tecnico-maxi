import { CategoryRequest } from '@/types/categoryRequest';
import { DeleteCategoryRequest } from '@/types/deleteCategoryRequest';
import { api } from './api';


export const getCategoryByUser = async () => {
  const response = await api.get(`/category`);
  return response.data;
}

export const getCategoryById = async (categoryId: string) => {
  const response = await api.get(`/category/${categoryId}`)
  return response.data
}

export const updateSelectedCategory = async (categoryId: string, request: CategoryRequest) => {
  const response = await api.put(`/category/${categoryId}`, request)
  return response.data
}

export const createNewCategory = async (request: CategoryRequest) => {
  const response = await api.post("/category", request)
  return response.data
}

export const deleteCategory = async ({ categoryId }: DeleteCategoryRequest) => {
  const response = await api.delete(`/category/${categoryId}`)
  return response.data
}
