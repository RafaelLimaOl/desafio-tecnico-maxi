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
import { useDeletePeople } from "@/hooks/usePeople"
import { Trash2 } from "lucide-react"

export function DeletePeopleDialog({
  peopleId,
  open,
  onClose,
  haveIcon,
}: {
  peopleId: string | null
  open: boolean
  onClose: () => void
  haveIcon: boolean
}) {
  const { deleteSelectedPeople } = useDeletePeople()
  const handleSubmit = async () => {
    const values = {
      peopleId: peopleId!,
      userId: "90698B51-77EC-F011-A114-74563CF0FC1F",
    }
    deleteSelectedPeople.mutate(values)
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
            Todas as transações vinculadas a essa pessoa serão respectivamente
            deletadas também.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancelar</AlertDialogCancel>
          <AlertDialogAction
            onClick={() => handleSubmit()}
            className="bg-red-600 hover:bg-red-800/95 text-white"
          >
            Sim, Deletar Pessoa
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
