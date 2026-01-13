// components/ui/form-field.tsx
import { Label } from "@/components/ui/label"
import { ReactNode } from "react"

interface FormItemProps {
  label: string
  children: ReactNode
}

export function CustomFormField({ label, children }: FormItemProps) {
  return (
    <div className="flex flex-col gap-2">
      <Label className="text-sm text-muted-foreground">{label}</Label>
      {children}
    </div>
  )
}
