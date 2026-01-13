"use client"

import {
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import {
  useDeleteUser,
  useGetUserSettings,
  useUpdateUserSettings,
} from "@/hooks/useUserSettings"
import { UserSettingsRequest } from "@/types/settingsRequest"
import { zodResolver } from "@hookform/resolvers/zod"
import { AlertDialog } from "@radix-ui/react-alert-dialog"
import { Label } from "@radix-ui/react-label"
import { useEffect } from "react"
import { useForm } from "react-hook-form"
import { toast } from "sonner"
import z from "zod"

export type People = {
  id: string
  userName: string
  age: number
  isActive: boolean
}

const SettingsPage = () => {
  const { data, isSuccess, isLoading, error } = useGetUserSettings()
  const { deleteAccount } = useDeleteUser()
  const { updateUser } = useUpdateUserSettings()

  const formSchema = z.object({
    userName: z.string().min(2, {
      message: "O nome deve ter mais de 2 caracteres.",
    }),
    email: z.email(),
    isActive: z.boolean(),
  })

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      userName: "",
      email: "",
      isActive: true,
    },
  })

  const handleSubmit = async (values: z.infer<typeof formSchema>) => {
    const updatedUser: UserSettingsRequest = {
      userName: values.userName,
      email: values.email,
      isActive: values.isActive,
    }
    updateUser.mutate({ request: updatedUser })
  }
  const handleDelete = () => {
    deleteAccount.mutate()
  }
  useEffect(() => {
    if (isSuccess && data) {
      toast.success("Seus dados foram retornados com sucesso!")
      form.reset({
        userName: data.data.userName,
        email: data.data.email,
        isActive: data.data.isActive,
      })
    }
    if (error) toast.error("Erro ao tentar retornar seus dados")
  }, [isSuccess, error, data, form])

  if (isLoading) return <p>Carregando...</p>
  if (error) return <p>Erro ao carregar pessoas</p>

  return (
    <>
      <div className="min-h-screen py-12">
        <div className="max-w-2xl mx-auto">
          <div className="bg-background rounded-2xl shadow-sm border p-8 space-y-10">
            <div>
              <h1 className="text-3xl font-bold tracking-tight">Seus dados</h1>
              <p className="text-muted-foreground">
                Gerencie as informações da sua conta
              </p>
            </div>

            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(handleSubmit)}
                className="space-y-8"
              >
                <FormField
                  control={form.control}
                  name="userName"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Nome de usuário</FormLabel>
                      <FormControl>
                        <Input placeholder="nome" {...field} />
                      </FormControl>
                      <FormDescription>
                        This is your public display name.
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email</FormLabel>
                      <FormControl>
                        <Input placeholder="email@gmail.com" {...field} />
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
                              Status da sua conta:{" "}
                              {field.value ? "Ativo" : "Inativo"}
                            </Label>
                            <p className="text-muted-foreground text-sm">
                              {field.value
                                ? "Desmarcando essa checkbox a sua conta será desativa."
                                : "Marcando essa checkbox a sua conta será ativada"}
                            </p>
                          </div>
                        </div>
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <Button type="submit">Atualizar novos dados</Button>
              </form>
            </Form>

            <div className="rounded-xl border border-red-500/40 bg-red-500/5 p-6 space-y-4">
              <div>
                <h2 className="text-lg font-semibold text-red-600">
                  Zona de perigo
                </h2>
                <p className="text-sm text-muted-foreground">
                  Ações irreversíveis para sua conta
                </p>
              </div>

              <div className="flex items-center justify-between">
                <p className="text-sm">
                  Excluir permanentemente sua conta e todos os dados.
                </p>
                <AlertDialog>
                  <AlertDialogTrigger asChild>
                    <Button variant="destructive">Deletar conta</Button>
                  </AlertDialogTrigger>
                  <AlertDialogContent>
                    <AlertDialogHeader>
                      <AlertDialogTitle>Você tem certeza?</AlertDialogTitle>
                      <AlertDialogDescription>
                        Todas as informações vinculadas a sua conta serão{" "}
                        <strong>PERMANENTEMENTE</strong> deletadas.
                      </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                      <AlertDialogCancel>Cancelar</AlertDialogCancel>
                      <AlertDialogAction
                        onClick={() => handleDelete()}
                        className="bg-red-600 hover:bg-red-800/95 text-white"
                      >
                        Sim, Deletar Conta
                      </AlertDialogAction>
                    </AlertDialogFooter>
                  </AlertDialogContent>
                </AlertDialog>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}

export default SettingsPage
