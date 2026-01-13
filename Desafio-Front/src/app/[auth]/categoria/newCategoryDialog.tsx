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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import { useCreateCategory } from "@/hooks/useCategory"
import { categoryTypeEnum, newCategorySchema } from "@/schema/newCategory"
import { zodResolver } from "@hookform/resolvers/zod"
import { Plus } from "lucide-react"
import { useForm } from "react-hook-form"
import z from "zod"

const categoryDefaultValues = {
  description: "",
  categoryType: categoryTypeEnum,
}

export function NewCategoryDialog({ emptyData }: { emptyData: boolean }) {
  const { createCategory } = useCreateCategory()

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
                  <FormItem>
                    <FormLabel>Tipo de categoria</FormLabel>
                    <FormControl>
                      <Select
                        value={field.value}
                        onValueChange={field.onChange}
                      >
                        <SelectTrigger className="w-45">
                          <SelectValue />
                        </SelectTrigger>

                        <SelectContent>
                          <SelectItem value="DESPESA">Despesa</SelectItem>
                          <SelectItem value="RECEITA">Receita</SelectItem>
                          <SelectItem value="AMBAS">Ambas</SelectItem>
                        </SelectContent>
                      </Select>
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
              <Button type="submit">Adicionar categoria</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
