"use client"

import { DataTable } from "@/components/data-table"
import { Button } from "@/components/ui/button"
import {
  Empty,
  EmptyContent,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from "@/components/ui/empty"
import { useGetAllPeople } from "@/hooks/usePeople"
import { ArrowUpRightIcon, Pencil, Trash2, Users } from "lucide-react"
import { useEffect, useState } from "react"
import { toast } from "sonner"

import { Badge } from "@/components/ui/badge"

import { ColumnDef } from "@tanstack/react-table"
import { DeletePeopleDialog } from "./deletePeopleDialog"
import { NewPeopleDialog } from "./newPeopleDialog"
import { EditPeopleDialog } from "./updatePeopleDialog"

export type People = {
  id: string
  name: string
  age: number
  isActive: boolean
}

const PeoplePage = () => {
  const { data, isSuccess, isLoading, error } = useGetAllPeople()

  const [editId, setEditId] = useState<string | null>(null)
  const [deleteId, setDeleteId] = useState<string | null>(null)
  const columns: ColumnDef<People>[] = [
    {
      accessorKey: "name",
      header: "Nome",
    },
    {
      accessorKey: "age",
      header: "Idade",
      cell: ({ row }) => {
        const age = row.original.age
        return (
          <span>
            {age} {age > 1 ? "anos" : "ano"}
          </span>
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

  useEffect(() => {
    if (isSuccess) toast.success("Lista de Pessoas retornada com sucesso!")

    if (error) toast.error("Erro ao tentar listar as pessoas cadastradas")
  }, [isSuccess, error])

  if (isLoading) return <p>Carregando...</p>
  if (error) return <p>Erro ao carregar pessoas</p>

  return (
    <div className="container mx-auto py-10">
      <div className="flex items-center justify-between mb-8 py-2 rounded-md">
        <div>
          <h1 className="text-2xl font-semibold">Pessoas Cadastradas</h1>
          <p className="text-muted-foreground text-sm">
            Gerencie todos as pessoas que você cadastradou até o momento
          </p>
        </div>
        <NewPeopleDialog emptyData={false} />
      </div>

      {data.data.length > 0 ? (
        <DataTable columns={columns} data={data.data} />
      ) : (
        <Empty>
          <EmptyHeader>
            <EmptyMedia variant="icon">
              <Users />
            </EmptyMedia>
            <EmptyTitle>Sem pessoas cadastradas ainda</EmptyTitle>
            <EmptyDescription>
              Você ainda não cadastrou nenhuma pessoa. Começe agora cadastrando
              a sua primeira pessoa
            </EmptyDescription>
          </EmptyHeader>
          <EmptyContent>
            <div className="flex gap-2">
              <NewPeopleDialog emptyData={true} />
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

      <EditPeopleDialog
        peopleId={editId}
        open={!!editId}
        onClose={() => setEditId(null)}
        haveIcon={false}
      />
      <DeletePeopleDialog
        peopleId={deleteId}
        open={!!deleteId}
        onClose={() => setDeleteId(null)}
        haveIcon={false}
      />
    </div>
  )
}

export default PeoplePage
