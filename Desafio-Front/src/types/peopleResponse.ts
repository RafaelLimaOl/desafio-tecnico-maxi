export type PeopleResponse = {
  id: string
  name: string
  age: number
  isActive: boolean
}

export type ApiPeopleResponse = {
  success: boolean
  message: string
  data?: PeopleResponse
}