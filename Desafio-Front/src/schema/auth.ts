import { z } from "zod"

export const baseSchema = {
  email: z.email("Email inválido"),
  password: z.string().min(6, "A senha precisa ter mais de 6 caracteres"),
}

export const loginSchema = z.object(baseSchema)

export const registerSchema = z.object({
  username: z.string().min(2, "O nome precisa de mais de 2 caracteres"),
  ...baseSchema,
})