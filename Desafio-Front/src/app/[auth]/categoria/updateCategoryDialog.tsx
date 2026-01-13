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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { useGetCategoryById, useUpdateCategory } from "@/hooks/useCategory"
import { updateCategorySchema } from "@/schema/updateCategory"
import { CategoryRequest } from "@/types/categoryRequest"
import { zodResolver } from "@hookform/resolvers/zod"
import { Pencil } from "lucide-react"
import { useEffect } from "react"
import { useForm } from "react-hook-form"
import z from "zod"

export function EdiCategoryDialog({
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
  const { updateCategory } = useUpdateCategory()

  const { data, isSuccess } = useGetCategoryById(categoryId!)

  const form = useForm({
    resolver: zodResolver(updateCategorySchema),
    defaultValues: {
      description: "",
      categoryType: "DESPESA",
      isActive: true,
    },
  })

  const handleSubmit = async (values: z.infer<typeof updateCategorySchema>) => {
    const validCategory = categoryId ?? ""
    const newCategory: CategoryRequest = {
      description: values.description,
      categoryType: values.categoryType,
      isActive: values.isActive,
    }
    updateCategory.mutate({ categoryId: validCategory, request: newCategory })
  }

  useEffect(() => {
    if (isSuccess && data) {
      form.reset({
        description: data.data.description,
        categoryType: data.data.categoryType,
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
                name="description"
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
                name="categoryType"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tipo de categoria</FormLabel>
                    <FormControl>
                      <Select
                        value={field.value}
                        onValueChange={field.onChange}
                      >
                        <SelectTrigger className="w-full">
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
                            Status da categoria:{" "}
                            {field.value ? "Ativo" : "Inativo"}
                          </Label>
                          <p className="text-muted-foreground text-sm">
                            {field.value
                              ? "Desmarcando essa checkbox a categoria será desativa."
                              : "Marcando essa checkbox a categoria será ativada"}
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
