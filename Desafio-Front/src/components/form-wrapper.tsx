import { Label } from "@/components/ui/label"
import { ReactNode } from "react"

interface FormWrapperProps {
  label: string
  children: ReactNode
}

export function FormWrapper({ label, children }: FormWrapperProps) {
  return (
    <div className="flex flex-col gap-2">
      <Label className="text-sm text-muted-foreground">{label}</Label>
      {children}
    </div>
  )
}
