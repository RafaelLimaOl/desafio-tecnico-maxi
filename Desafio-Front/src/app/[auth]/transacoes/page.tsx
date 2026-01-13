"use client"

import { DataTable } from "@/components/data-table"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
  Empty,
  EmptyContent,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from "@/components/ui/empty"
import { useGetAllCategory } from "@/hooks/useCategory"
import { useGetAllPeople } from "@/hooks/usePeople"
import { useGetAllTransactionByUser } from "@/hooks/useTransaction"
import { TransactionStatus } from "@/types/transactionResponse"
import { ColumnDef } from "@tanstack/react-table"
import { ArrowUpRightIcon, Pencil, Trash2, Users } from "lucide-react"
import { useEffect, useState } from "react"
import { toast } from "sonner"
import { DeleteTransactionDialog } from "./deleteTransactionDialog"
import { NewTransactionDialog } from "./newTransactionDialog"
import { UpdateTransactionDialog } from "./updateTransactionDialog"

export type Transaction = {
  id: string
  description: string
  amount: string
  categoryId: string
  peopleId: string

  transactionType: string
  status: string

  isActive: boolean
}

const transactionStatusColors: Record<TransactionStatus, string> = {
  PENDENTE: "bg-yellow-500",
  CONCLUIDA: "bg-green-600",
  ATRASADA: "bg-red-600",
  CANCELADA: "bg-gray-500",
}

const TransactionPage = () => {
  const { data, isSuccess, isLoading, error } = useGetAllTransactionByUser()

  const [editId, setEditId] = useState<string | null>(null)
  const [deleteId, setDeleteId] = useState<string | null>(null)

  const { data: peopleData } = useGetAllPeople()
  const { data: categoryData } = useGetAllCategory()

  const columns: ColumnDef<Transaction>[] = [
    {
      accessorKey: "description",
      header: "Descrição",
    },
    {
      accessorKey: "amount",
      header: "Valor",
      cell: ({ row }) => {
        const amount = row.original.amount

        return <p>R$ {amount}</p>
      },
    },
    {
      accessorKey: "transactionStatus",
      header: "Finalidade",
      cell: ({ row }) => {
        const type = row.original.transactionType
        return (
          <Badge
            variant="default"
            className={`text-white capitalize
              ${type === "DESPESA" ? "bg-green-600" : "bg-blue-600"}
              `}
          >
            {type.toLowerCase()}
          </Badge>
        )
      },
    },
    {
      accessorKey: "status",
      header: "Status da Transação",
      cell: ({ row }) => {
        const status = row.original.status as TransactionStatus
        return (
          <Badge
            className={`text-white capitalize ${transactionStatusColors[status]}`}
          >
            {status.toLowerCase()}
          </Badge>
        )
      },
    },
    {
      accessorKey: "isActive",
      header: "Status",
      cell: ({ row }) => {
        const isActive = row.original.isActive
        return (
          <Badge variant={isActive ? "default" : "secondary"}>
            {isActive ? "Ativo" : "Inativo"}
          </Badge>
        )
      },
    },
    {
      id: "actions",
      header: () => <div className="text-end pr-10">Ações</div>,
      cell: ({ row }) => {
        const person = row.original

        return (
          <div className="flex justify-end gap-2">
            <Button onClick={() => setEditId(person.id)}>
              <Pencil />
            </Button>
            <Button
              variant="destructive"
              onClick={() => setDeleteId(person.id)}
            >
              <Trash2 />
            </Button>
          </div>
        )
      },
    },
  ]

  const people = peopleData?.data ?? []
  const categories = categoryData?.data ?? []

  const haveData = people.length > 0 && categories.length > 0

  useEffect(() => {
    if (isSuccess) toast.success("Lista de Transações retornada com sucesso!")

    if (error) toast.error("Erro ao tentar listar as transações cadastradas")
  }, [isSuccess, error])

  if (isLoading) return <p>Carregando...</p>
  if (error) return <p>Erro ao carregar pessoas</p>

  return (
    <div className="container mx-auto py-10">
      <div className="flex items-center justify-between mb-8 py-2 rounded-md">
        <div>
          <h1 className="text-2xl font-semibold">Transações Cadastradas</h1>
          <p className="text-muted-foreground text-sm">
            Gerencie todos as transações que você cadastradou até o momento
          </p>
        </div>
        <NewTransactionDialog
          emptyData={false}
          categoryData={categories}
          peopleData={people}
        />
      </div>

      {data.data.length > 0 ? (
        <DataTable columns={columns} data={data.data} />
      ) : (
        <Empty>
          <EmptyHeader>
            <EmptyMedia variant="icon">
              <Users />
            </EmptyMedia>
            <EmptyTitle>Sem transações cadastradas ainda</EmptyTitle>
            <EmptyDescription>
              Você ainda não cadastrou nenhuma transação.
            </EmptyDescription>
          </EmptyHeader>
          <EmptyContent>
            <div className="flex gap-2">
              {haveData && (
                <NewTransactionDialog
                  emptyData={true}
                  peopleData={peopleData}
                  categoryData={categoryData}
                />
              )}
            </div>
          </EmptyContent>
          <Button
            variant="link"
            asChild
            className="text-muted-foreground"
            size="sm"
          >
            <a href="#">
              Voltar para home <ArrowUpRightIcon />
            </a>
          </Button>
        </Empty>
      )}

      <DeleteTransactionDialog
        transactionId={deleteId}
        open={!!deleteId}
        onClose={() => setDeleteId(null)}
        haveIcon={false}
      />
      <UpdateTransactionDialog
        transactionId={editId}
        open={!!editId}
        onClose={() => setEditId(null)}
        haveIcon={false}
        categoryData={categories}
        peopleData={people}
      />
    </div>
  )
}

export default TransactionPage
