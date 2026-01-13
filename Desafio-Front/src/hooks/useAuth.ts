"use client"

import { clearTokens, saveTokens } from "@/lib/auth"
import { loginUser, registerUser } from "@/services/authService"
import { ApiAuthResponse } from "@/types/loginRespone"
import { useMutation } from "@tanstack/react-query"
import { useRouter } from "next/navigation"
import { toast } from "sonner"

export function useAuth() {

    const router = useRouter()

    const login = useMutation({
        mutationFn: loginUser,
        onSuccess: (data: ApiAuthResponse) => {
            if (data.data?.accessToken && data.data?.refreshToken && data.data?.userId) {
                saveTokens(data.data.accessToken, data.data.refreshToken)
                router.push("/dash")
                toast.success("Login realizado com sucesso!")
            }
        },
        onError: () => {
            toast.error("Senha ou email inválidos!")
        }
    })

    const register = useMutation({
        mutationFn: registerUser,
        onSuccess: () => {
            toast.success("Conta cadastrada com sucesso!")
        },
        onError: () => {
            toast.error("Erro ao cadastrar a conta!")
        },
    })

    const logout = () => {
        clearTokens()
        router.push('/')
        toast.info("Você saiu da conta")
    }

    return { login, register, logout }
}
