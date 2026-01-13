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
import { ColumnDef } from "@tanstack/react-table"
import { ArrowUpRightIcon, Pencil, Trash2, Users } from "lucide-react"
import { useEffect, useState } from "react"
import { toast } from "sonner"
import { DeleteCategoryDialog } from "./deleteCategoryDialog"
import { NewCategoryDialog } from "./newCategoryDialog"
import { EdiCategoryDialog } from "./updateCategoryDialog"

export type Category = {
  id: string
  description: string
  categoryType: string
  isActive: boolean
}

const CategoryPage = () => {
  const { data, isSuccess, isLoading, error } = useGetAllCategory()

  const [editId, setEditId] = useState<string | null>(null)
  const [deleteId, setDeleteId] = useState<string | null>(null)

  const columns: ColumnDef<Category>[] = [
    {
      accessorKey: "description",
      header: "Descrição",
    },
    {
      accessorKey: "categoryType",
      header: "Finalidade",
      cell: ({ row }) => {
        const type = row.original.categoryType
        return (
          <Badge
            variant="default"
            className={`text-white capitalize
              ${
                type === "DESPESA"
                  ? "bg-green-600"
                  : type === "RECEITA"
                  ? "bg-blue-600"
                  : "bg-orange-600"
              }
              `}
          >
            {type.toLowerCase()}
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

  useEffect(() => {
    if (isSuccess) toast.success("Lista de Categorias retornada com sucesso!")

    if (error) toast.error("Erro ao tentar listar as categorias cadastradas")
  }, [isSuccess, error])

  if (isLoading) return <p>Carregando...</p>
  if (error) return <p>Erro ao carregar pessoas</p>

  return (
    <div className="container mx-auto py-10">
      <div className="flex items-center justify-between mb-8 py-2 rounded-md">
        <div>
          <h1 className="text-2xl font-semibold">Categorias Cadastradas</h1>
          <p className="text-muted-foreground text-sm">
            Gerencie todos as categorias que você cadastradou para uma melhor
            organização das transações
          </p>
        </div>
        <NewCategoryDialog emptyData={false} />
      </div>

      {data.data.length > 0 ? (
        <DataTable columns={columns} data={data.data} />
      ) : (
        <Empty>
          <EmptyHeader>
            <EmptyMedia variant="icon">
              <Users />
            </EmptyMedia>
            <EmptyTitle>Sem categoria cadastradas ainda</EmptyTitle>
            <EmptyDescription>
              Você ainda não cadastrou nenhuma categoria. Começe agora
              cadastrando a sua primeira categoria
            </EmptyDescription>
          </EmptyHeader>
          <EmptyContent>
            <div className="flex gap-2">
              <NewCategoryDialog emptyData={true} />
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

      <DeleteCategoryDialog
        categoryId={deleteId}
        open={!!deleteId}
        onClose={() => setDeleteId(null)}
        haveIcon={false}
      />

      <EdiCategoryDialog
        categoryId={editId}
        open={!!editId}
        onClose={() => setEditId(null)}
        haveIcon={false}
      />
    </div>
  )
}

export default CategoryPage
