import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { FormWrapper } from "./form-wrapper"

interface FormSelectProps<T> {
  label: string
  placeholder: string
  options: T[]
  value?: string
  onChange?: (value: string) => void
  getValue: (item: T) => string
  getLabel: (item: T) => string
}

export function FormSelect<T>({
  label,
  placeholder,
  options,
  value,
  onChange,
  getValue,
  getLabel,
}: FormSelectProps<T>) {
  return (
    <FormWrapper label={label}>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger
          className={`w-full ${
            options.length <= 0 ? "cursor-not-allowed bg-gray-400 w-full" : ""
          }
            `}
        >
          <SelectValue placeholder={placeholder} />
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectLabel>{label}</SelectLabel>
            {options.map((item) => (
              <SelectItem key={getValue(item)} value={getValue(item)}>
                {getLabel(item)}
              </SelectItem>
            ))}
          </SelectGroup>
        </SelectContent>
      </Select>
    </FormWrapper>
  )
}
