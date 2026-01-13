import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import { useDeleteCategory } from "@/hooks/useCategory"

import { Trash2 } from "lucide-react"

export function DeleteCategoryDialog({
  categoryId,
  open,
  onClose,
  haveIcon,
}: {
  categoryId: string | null
  open: boolean
  onClose: () => void
  haveIcon: boolean
}) {
  const { deleteSelectedCategory } = useDeleteCategory()
  const handleSubmit = async () => {
    const values = {
      categoryId: categoryId!,
    }
    deleteSelectedCategory.mutate(values)
  }

  return (
    <AlertDialog open={open} onOpenChange={onClose}>
      {haveIcon && (
        <AlertDialogTrigger asChild>
          <Button variant="destructive">
            <Trash2 />
          </Button>
        </AlertDialogTrigger>
      )}
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Você tem certeza?</AlertDialogTitle>
          <AlertDialogDescription>
            Todas as transações vinculadas a essa categoria serão
            respectivamente deletadas também.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancelar</AlertDialogCancel>
          <AlertDialogAction
            onClick={() => handleSubmit()}
            className="bg-red-600 hover:bg-red-800/95 text-white"
          >
            Sim, Deletar Categoria
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
