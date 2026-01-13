"use client"

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
import {
  InputGroup,
  InputGroupAddon,
  InputGroupInput,
  InputGroupText,
} from "@/components/ui/input-group"
import { Textarea } from "@/components/ui/textarea"
import { useCreateTransaction } from "@/hooks/useTransaction"
import { newTransactionSchema } from "@/schema/newTransaction"
import { CategoryResponse } from "@/types/categoryResponse"
import { PeopleResponse } from "@/types/peopleResponse"
import { TransactionStatus, TransactionType } from "@/types/transactionResponse"
import { zodResolver } from "@hookform/resolvers/zod"
import { Plus } from "lucide-react"
import { useForm } from "react-hook-form"
import z from "zod"

const transactionDefaultValues = {
  description: "",
  categoryId: "",
  peopleId: "",
  userId: "",
  amount: undefined,
  transactionType: TransactionType.DESPESA,
}

export function NewTransactionDialog({
  emptyData,
  peopleData = [],
  categoryData = [],
}: {
  emptyData: boolean
  peopleData?: PeopleResponse[]
  categoryData?: CategoryResponse[]
}) {
  const { createTransaction } = useCreateTransaction()
  const activePeople = peopleData.filter((p) => p.isActive)
  const activeCategories = categoryData.filter((c) => c.isActive)
  const finalidade = [
    {
      id: 1,
      label: "Despesa",
      value: "DESPESA",
    },
    {
      id: 2,
      label: "Receita",
      value: "RECEITA",
    },
  ]

  const form = useForm({
    resolver: zodResolver(newTransactionSchema),
    defaultValues: transactionDefaultValues,
  })

  const handleSubmit = async (values: z.infer<typeof newTransactionSchema>) => {
    const newTransaction = {
      description: values.description,
      amount: String(values.amount),
      transactionType: values.transactionType,
      status: TransactionStatus.PENDENTE,
      peopleId: values.peopleId,
      categoryId: values.categoryId,
    }

    createTransaction.mutate(newTransaction)
    form.reset()
  }

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="default">
          <Plus />
          {emptyData ? "Cadastrar nova transação" : ""}
        </Button>
      </DialogTrigger>

      <DialogContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)}>
            <DialogHeader>
              <DialogTitle>Nova transação</DialogTitle>
              <DialogDescription>Adicione uma nova transação</DialogDescription>
            </DialogHeader>
            <div className="grid gap-6 pb-4">
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
                          value={String(field.value)}
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
                  <FormSelect
                    label="Finalidade"
                    placeholder="Selecione..."
                    options={finalidade}
                    value={field.value}
                    onChange={field.onChange}
                    getValue={(f) => f.value}
                    getLabel={(f) => f.label}
                  />
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
            </div>

            <DialogFooter>
              <DialogClose asChild>
                <Button variant="outline">Cancelar</Button>
              </DialogClose>
              <Button type="submit">Adicionar transação</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
