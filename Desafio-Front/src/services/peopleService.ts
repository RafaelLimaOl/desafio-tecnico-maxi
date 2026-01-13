import { DeletePeopleRequest } from '@/types/deletePeopleRequest'
import { PeopleRequest } from '@/types/peopleRequest'
import { api } from './api'


export const getPeopleByUser = async () => {
  const response = await api.get(`/people`)
  return response.data
}

export const getPeopleById = async (peopleId: string) => {
  const response = await api.get(`/people/${peopleId}`)
  return response.data
}

export const updateSelectedPeople = async (peopleId: string, request: PeopleRequest) => {
  const response = await api.put(`/people/${peopleId}`, request)
  return response.data
}

export const createNewPeople = async (request: PeopleRequest) => {
  const response = await api.post("/people", request)
  return response.data
}

export const deletePeople = async ({  peopleId }: DeletePeopleRequest) => {
  const response = await api.delete(`/people/${peopleId}`)
  return response.data
}
