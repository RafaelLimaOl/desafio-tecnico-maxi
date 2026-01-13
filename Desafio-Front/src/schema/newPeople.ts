import z from "zod"

export const newPeopleSchema = z.object({
  peopleName: z.string().min(2, "O nome precisa de mais de 2 caracteres").max(150, "O nome não pode passar de 150 caracteres"),
  age: z.coerce.number("A idade inválida",).min(1, "A idade deve ser maior que 1 ano").max(120, "A idade não deve ultrapassar 120 anos")
})