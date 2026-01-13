import { createNewCategory, deleteCategory, getCategoryById, getCategoryByUser, updateSelectedCategory } from "@/services/categoryService"
import { CategoryRequest } from "@/types/categoryRequest"
import { ApiCategoryResponse } from "@/types/categoryResponse"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"

export const useGetAllCategory = () => {
  return useQuery({
    queryKey: ["category-get-all"],
    queryFn: () => getCategoryByUser(),

    staleTime: 1000 * 60 * 5, 
    gcTime: 1000 * 60 * 30,

    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,

  })
}

export const useGetCategoryById = (categoryId: string) => {
  return useQuery({
    queryKey: ["category-by-id"],
    queryFn: () => getCategoryById(categoryId),
    enabled: !!categoryId,

    staleTime: 1000 * 60 * 5,
  })
}

export const useCreateCategory = () => {
  const queryClient = useQueryClient()
  const createCategory = useMutation({
    mutationFn: createNewCategory,
    onSuccess: (data: ApiCategoryResponse) => {
      if (data.success) {
        toast.success("Categoria cadastrada com sucesso!")
        queryClient.invalidateQueries({queryKey: ["category-get-all"]})
      }
    },
    onError: () => {
      toast.error("Erro ao criar nova categoria!")
    }
  })

  return { createCategory }
}

export const useUpdateCategory = () => {
  const queryClient = useQueryClient()

   const updateCategory = useMutation({
    mutationFn: ({ categoryId, request }: {categoryId: string, request: CategoryRequest}) =>
      updateSelectedCategory(categoryId, request),

    onSuccess: () => {
      toast.success("Categoria editada com sucesso!")
      queryClient.invalidateQueries({ queryKey: ["category-get-all"] })
      queryClient.invalidateQueries({ queryKey: ["category-by-id"] })
    },
    onError: () => {
      toast.error("Erro ao atualizar as novas informações!")
    }
  })

  return { updateCategory }
}

export const useDeleteCategory = () => {
  const queryClient = useQueryClient()
  const deleteSelectedCategory = useMutation({
    mutationFn: deleteCategory,
    onSuccess: (data: ApiCategoryResponse) => {
      if (data.success) {
          toast.success("Categoria deletada com sucesso!")
      }
      queryClient.invalidateQueries({
        queryKey: ["category-get-all"]
      })
    },
    onError: () => {
        toast.error("Erro ao deletar a categoria selecionada!")
    }
  })

  return { deleteSelectedCategory}
}