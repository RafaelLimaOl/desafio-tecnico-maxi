import { FormSelect } from "@/components/form-select"
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
import { Textarea } from "@/components/ui/textarea"
import { useCreateCategory } from "@/hooks/useCategory"
import { newCategorySchema } from "@/schema/newCategory"
import { CategoryType } from "@/types/categoryResponse"
import { zodResolver } from "@hookform/resolvers/zod"
import { Plus } from "lucide-react"
import { useForm } from "react-hook-form"
import z from "zod"

const categoryDefaultValues = {
  description: "",
  categoryType: CategoryType.DESPESA,
}

export function NewCategoryDialog({ emptyData }: { emptyData: boolean }) {
  const { createCategory } = useCreateCategory()

  const typeCategory = [
    {
      id: 1,
      label: "Despesa",
      value: CategoryType.DESPESA,
    },
    {
      id: 2,
      label: "Receita",
      value: CategoryType.RECEITA,
    },
    {
      id: 3,
      label: "Ambas",
      value: CategoryType.AMBAS,
    },
  ]

  const form = useForm({
    resolver: zodResolver(newCategorySchema),
    defaultValues: categoryDefaultValues,
  })

  const handleSubmit = async (values: z.infer<typeof newCategorySchema>) => {
    const newCategory = {
      description: values.description,
      categoryType: values.categoryType,
    }
    createCategory.mutate(newCategory)

    form.reset()
  }
  return (
    <Dialog>
      <DialogTrigger asChild>
        {emptyData ? (
          <Button>
            Cadastrar nova Categoria <Plus />
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
              <DialogTitle>Nova categoria</DialogTitle>
              <DialogDescription>
                Adicione uma nova categoria para organizar as transações das
                pessoas cadastradas
              </DialogDescription>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Descrição</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder="Descrição do que se trata a nova categoria"
                        {...field}
                        rows={2}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="categoryType"
                render={({ field }) => (
                  <FormSelect
                    label="Finalidade"
                    placeholder="Selecione..."
                    options={typeCategory}
                    value={field.value}
                    onChange={field.onChange}
                    getValue={(f) => f.value}
                    getLabel={(f) => f.label}
                  />
                )}
              />
            </div>
            <DialogFooter>
              <DialogClose asChild>
                <Button variant="outline">Cancelar</Button>
              </DialogClose>
              <Button type="submit">Adicionar categoria</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
