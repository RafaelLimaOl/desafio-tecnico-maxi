import { Button } from "@/components/ui/button"
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
import { useCreatePeople } from "@/hooks/usePeople"
import { newPeopleSchema } from "@/schema/newPeople"
import { zodResolver } from "@hookform/resolvers/zod"
import { Plus } from "lucide-react"
import { useForm } from "react-hook-form"
import z from "zod"

const peopleDefaultValues = {
  peopleName: "",
  age: undefined,
}

export function NewPeopleDialog({ emptyData }: { emptyData: boolean }) {
  const { createPeople } = useCreatePeople()

  const form = useForm({
    resolver: zodResolver(newPeopleSchema),
    defaultValues: peopleDefaultValues,
  })

  const handleSubmit = async (values: z.infer<typeof newPeopleSchema>) => {
    const newPeople = {
      name: values.peopleName,
      age: values.age,
    }
    createPeople.mutate(newPeople)
    form.reset()
  }
  return (
    <Dialog>
      <DialogTrigger asChild>
        {emptyData ? (
          <Button>
            Cadastrar nova Pessoa <Plus />
          </Button>
        ) : (
          <Button>
            <Plus />
          </Button>
        )}
      </DialogTrigger>

      <DialogContent className="sm:max-w-106.25">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)}>
            <DialogHeader>
              <DialogTitle>Nova pessoa</DialogTitle>
              <DialogDescription>
                Adicione uma nova pessoa para cadastrar seus gastos
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
                        value={String(field.value) ?? ""}
                        onChange={(e) =>
                          field.onChange(
                            e.target.value === ""
                              ? undefined
                              : Number(e.target.value)
                          )
                        }
                      />
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
              <Button type="submit">Adicionar pessoa</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
