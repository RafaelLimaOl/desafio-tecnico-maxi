import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useGetPeopleById, useUpdatePeople } from "@/hooks/usePeople"
import { updatePeopleSchema } from "@/schema/updatePeople"
import { PeopleRequest } from "@/types/peopleRequest"
import { zodResolver } from "@hookform/resolvers/zod"
import { Pencil } from "lucide-react"
import { useEffect } from "react"
import { useForm } from "react-hook-form"
import z from "zod"

export function EditPeopleDialog({
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
  const { updatePeople } = useUpdatePeople()

  const { data, isSuccess } = useGetPeopleById(peopleId!)

  const form = useForm({
    resolver: zodResolver(updatePeopleSchema),
    defaultValues: {
      peopleName: "",
      age: undefined,
      isActive: true,
    },
  })

  const handleSubmit = async (values: z.infer<typeof updatePeopleSchema>) => {
    const validPeopleId = peopleId ?? ""
    const updatedPeople: PeopleRequest = {
      name: values.peopleName,
      age: values.age,
      isActive: values.isActive,
    }
    updatePeople.mutate({ peopleId: validPeopleId, request: updatedPeople })
  }

  useEffect(() => {
    if (isSuccess && data) {
      form.reset({
        peopleName: data.data.name,
        age: data.data.age,
        isActive: data.data.isActive,
      })
    }
  }, [isSuccess, data, form])

  return (
    <Dialog open={open} onOpenChange={onClose}>
      {haveIcon && (
        <DialogTrigger asChild>
          <Button>
            <Pencil />
          </Button>
        </DialogTrigger>
      )}

      <DialogContent className="sm:max-w-106.25">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)}>
            <DialogHeader>
              <DialogTitle>Editar pessoa</DialogTitle>
              <DialogDescription>
                Edite as seguintes informações da pessoa selecionada
              </DialogDescription>
            </DialogHeader>

            <div className="grid gap-4 py-4">
              <FormField
                control={form.control}
                name="peopleName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Nome</FormLabel>
                    <FormControl>
                      <Input placeholder="Nome" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="age"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Idade</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        placeholder="Idade"
                        value={String(field.value)}
                        onChange={(e) => field.onChange(e.target.valueAsNumber)}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="isActive"
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <div className="flex gap-2">
                        <Checkbox
                          checked={field.value}
                          onCheckedChange={field.onChange}
                          className="data-[state=checked]:border-blue-600 data-[state=checked]:bg-blue-600 data-[state=checked]:text-white dark:data-[state=checked]:border-blue-700 dark:data-[state=checked]:bg-blue-700"
                        />
                        <div className="grid gap-2">
                          <Label>
                            Status da pessoa:{" "}
                            {field.value ? "Ativo" : "Inativo"}
                          </Label>
                          <p className="text-muted-foreground text-sm">
                            {field.value
                              ? "Desmarcando essa checkbox a pessoa será desativa."
                              : "Marcando essa checkbox a pessoa será ativada"}
                          </p>
                        </div>
                      </div>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <DialogFooter>
              <DialogClose asChild>
                <Button variant="outline">Cancelar</Button>
              </DialogClose>
              <Button type="submit">Editar pessoa</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
