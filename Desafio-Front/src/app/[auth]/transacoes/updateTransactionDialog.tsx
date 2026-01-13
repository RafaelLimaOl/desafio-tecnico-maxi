import { FormSelect } from "@/components/form-select"
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
import {
  InputGroup,
  InputGroupAddon,
  InputGroupInput,
  InputGroupText,
} from "@/components/ui/input-group"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import {
  useGetTransactionById,
  useUpdateTransaction,
} from "@/hooks/useTransaction"
import { updateTransactionSchema } from "@/schema/updateTransaction"
import { CategoryResponse } from "@/types/categoryResponse"
import { PeopleResponse } from "@/types/peopleResponse"
import { TransactionRequest } from "@/types/transactionRequest"
import { TransactionStatus, TransactionType } from "@/types/transactionResponse"
import { zodResolver } from "@hookform/resolvers/zod"
import { Pencil } from "lucide-react"
import { useEffect } from "react"
import { useForm } from "react-hook-form"
import z from "zod"

export function UpdateTransactionDialog({
  transactionId,
  open,
  onClose,
  haveIcon,
  peopleData = [],
  categoryData = [],
}: {
  transactionId: string | null
  open: boolean
  onClose: () => void
  haveIcon: boolean
  peopleData?: PeopleResponse[]
  categoryData?: CategoryResponse[]
}) {
  const { updateTransaction } = useUpdateTransaction()
  const activePeople = peopleData.filter((p) => p.isActive)
  const activeCategories = categoryData.filter((c) => c.isActive)

  const { data, isSuccess } = useGetTransactionById(transactionId!)

  const form = useForm({
    resolver: zodResolver(updateTransactionSchema),
    defaultValues: {
      description: "",
      amount: "",
      isActive: true,
      categoryId: "",
      peopleId: "",
      transactionType: TransactionType.DESPESA,
      status: TransactionStatus.PENDENTE,
    },
  })

  const handleSubmit = async (
    values: z.infer<typeof updateTransactionSchema>
  ) => {
    const validTransactionId = transactionId ?? ""
    const updatedTransaction: TransactionRequest = {
      description: values.description,
      amount: values.amount,
      isActive: values.isActive,
      categoryId: values.categoryId,
      peopleId: values.peopleId,
      status: values.status,
      transactionType: values.transactionType,
    }
    updateTransaction.mutate({
      transactionId: validTransactionId,
      request: updatedTransaction,
    })
  }

  useEffect(() => {
    if (isSuccess && data) {
      form.reset({
        description: data.data.description,
        amount: data.data.amount,

        categoryId: data.data.categoryId,
        peopleId: data.data.peopleId,

        status: data.data.status,
        transactionType: data.data.transactionType,

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
              <DialogTitle>Editar transação</DialogTitle>
              <DialogDescription>
                Edite as seguintes informações da transação
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
                        {...field}
                        placeholder="Descrição da transação"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="amount"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Valor</FormLabel>
                    <FormControl>
                      <InputGroup>
                        <InputGroupAddon>
                          <InputGroupText>R$</InputGroupText>
                        </InputGroupAddon>
                        <InputGroupInput
                          type="number"
                          step="0.01"
                          value={field.value}
                          onChange={(e) => field.onChange(e.target.value)}
                          name={field.name}
                          ref={field.ref}
                        />
                        <InputGroupAddon align="inline-end">
                          <InputGroupText>Reais</InputGroupText>
                        </InputGroupAddon>
                      </InputGroup>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="transactionType"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tipo de categoria</FormLabel>
                    <FormControl>
                      <Select
                        value={String(field.value)}
                        onValueChange={field.onChange}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue />
                        </SelectTrigger>

                        <SelectContent>
                          <SelectItem value="DESPESA">Despesa</SelectItem>
                          <SelectItem value="RECEITA">Receita</SelectItem>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="status"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Tipo de categoria</FormLabel>
                    <FormControl>
                      <Select
                        value={String(field.value)}
                        onValueChange={field.onChange}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue />
                        </SelectTrigger>

                        <SelectContent>
                          <SelectItem value="PENDENTE">Pendente</SelectItem>
                          <SelectItem value="CONCLUIDA">Concluída</SelectItem>
                          <SelectItem value="ATRASADA">Atrasada</SelectItem>
                          <SelectItem value="CANCELADA">Cancelada</SelectItem>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="categoryId"
                render={({ field }) => (
                  <FormSelect
                    label="Categoria"
                    placeholder="Selecione..."
                    options={activeCategories}
                    value={field.value}
                    onChange={field.onChange}
                    getValue={(c) => c.id}
                    getLabel={(c) => c.description}
                  />
                )}
              />
              <FormField
                control={form.control}
                name="peopleId"
                render={({ field }) => (
                  <FormSelect
                    label="Pessoa"
                    placeholder="Selecione..."
                    options={activePeople}
                    value={field.value}
                    onChange={field.onChange}
                    getValue={(p) => p.id}
                    getLabel={(p) => p.name}
                  />
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
                            Status da transação:{" "}
                            {field.value ? "Ativo" : "Inativo"}
                          </Label>
                          <p className="text-muted-foreground text-sm">
                            {field.value
                              ? "Desmarcando essa checkbox a transação será desativa."
                              : "Marcando essa checkbox a transação será ativada"}
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
              <Button type="submit">Editar Transação</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
