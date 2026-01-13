"use client"

import { Button } from "@/components/ui/button"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { useAuth } from "@/hooks/useAuth"
import { loginSchema, registerSchema } from "@/schema/auth"
import { zodResolver } from "@hookform/resolvers/zod"
import { InfoIcon } from "lucide-react"
import { useForm } from "react-hook-form"
import { z } from "zod"
import {
  InputGroup,
  InputGroupAddon,
  InputGroupButton,
  InputGroupInput,
} from "./ui/input-group"
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip"

type AuthMode = "login" | "register"

type LoginData = z.infer<typeof loginSchema>
type RegisterData = z.infer<typeof registerSchema>

type FormData = LoginData | RegisterData

interface AuthFormProps {
  mode: AuthMode
  changeTab: (tab: AuthMode) => void
}

const loginDefaults = {
  email: "",
  password: "",
}

const registerDefaults = {
  username: "",
  email: "",
  password: "",
}

export function AuthForm({ mode, changeTab }: AuthFormProps) {
  const schema = mode === "login" ? loginSchema : registerSchema
  const { login, register } = useAuth()

  const form = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: mode === "login" ? loginDefaults : registerDefaults,
  })

  const handleSubmit = async (values: z.infer<typeof schema>) => {
    if (mode === "login") {
      login.mutate(values)
    } else {
      register.mutate(values)
    }
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(handleSubmit)}
        className="space-y-4 max-w-sm mx-auto"
      >
        {mode === "register" && (
          <FormField
            control={form.control}
            name="username"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Nome</FormLabel>
                <FormControl>
                  <Input placeholder="Nome de usuário" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        )}

        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input placeholder="email@gmail.com" type="email" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Senha</FormLabel>
              <FormControl>
                <InputGroup>
                  <InputGroupInput
                    placeholder="Digite a senha"
                    type="password"
                    {...field}
                  />
                  <InputGroupAddon align="inline-end">
                    <Tooltip>
                      <TooltipTrigger asChild>
                        <InputGroupButton
                          variant="ghost"
                          aria-label="Info"
                          size="icon-xs"
                        >
                          <InfoIcon />
                        </InputGroupButton>
                      </TooltipTrigger>
                      <TooltipContent>
                        <p>A senha deve ter pelo menos 6 caracteres</p>
                      </TooltipContent>
                    </Tooltip>
                  </InputGroupAddon>
                </InputGroup>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex gap-2">
          <Button type="submit">
            {mode === "login" ? "Login" : "Registrar"}
          </Button>
          <Button
            type="reset"
            variant="link"
            onClick={() => changeTab(mode === "login" ? "register" : "login")}
          >
            {mode === "login"
              ? "Ainda não possui uma conta?"
              : "Já possui uma conta registrada?"}
          </Button>
        </div>
      </form>
    </Form>
  )
}
