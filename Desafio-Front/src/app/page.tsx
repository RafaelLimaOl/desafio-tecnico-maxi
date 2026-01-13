"use client"

import { AuthForm } from "@/components/auth-form"
import { ThemeToggle } from "@/components/theme-toggle"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { useState } from "react"

export default function Home() {
  const [tab, setTab] = useState("register")
  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 dark:bg-black">
      <div className="flex w-full max-w-sm flex-col gap-6">
        <Tabs value={tab} onValueChange={setTab} defaultValue="register">
          <TabsList>
            <TabsTrigger value="register">Registrar</TabsTrigger>
            <TabsTrigger value="login">Login</TabsTrigger>
          </TabsList>

          <TabsContent value="register">
            <Card>
              <CardHeader>
                <CardTitle>Registre uma nova conta</CardTitle>
                <CardDescription>Crie uma nova conta </CardDescription>
              </CardHeader>
              <CardContent>
                <AuthForm mode="register" changeTab={setTab} />
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="login">
            <Card>
              <CardHeader>
                <CardTitle>Login</CardTitle>
                <CardDescription>
                  Seja bem vindo de volta, faça o login para voltar aos negócios
                </CardDescription>
              </CardHeader>
              <CardContent>
                <AuthForm mode="login" changeTab={setTab} />
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
        <div className="absolute top-10 right-10  flex justify-center gap-4 opacity-80">
          <ThemeToggle />
        </div>
      </div>
    </div>
  )
}
