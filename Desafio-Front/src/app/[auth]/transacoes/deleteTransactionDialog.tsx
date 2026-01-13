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
import { useDeleteTransaction } from "@/hooks/useTransaction"
import { Trash2 } from "lucide-react"

export function DeleteTransactionDialog({
  transactionId,
  open,
  onClose,
  haveIcon,
}: {
  transactionId: string | null
  open: boolean
  onClose: () => void
  haveIcon: boolean
}) {
  const { deleteSelectedTransaction } = useDeleteTransaction()
  const handleSubmit = async () => {
    const values = {
      transactionId: transactionId!,
    }
    deleteSelectedTransaction.mutate(values)
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
            A seguinte ação não pode ser desfeita. Portante todas as informações
            da transação serão deletadas permanentemente.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancelar</AlertDialogCancel>
          <AlertDialogAction
            onClick={() => handleSubmit()}
            className="bg-red-600 hover:bg-red-800/95 text-white"
          >
            Deletar Transação
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
