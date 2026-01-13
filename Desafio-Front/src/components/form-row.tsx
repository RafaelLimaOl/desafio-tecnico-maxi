import { ReactNode } from "react"

export function FormRow({ children }: { children: ReactNode }) {
  return <div className="grid grid-cols-2 gap-4">{children}</div>
}
