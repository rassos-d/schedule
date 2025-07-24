import { AddInputList } from "./input"

export type Direction = {
  name: string
  id: string
  type: number
}

export type NewDirection = {
  name: string
  id: string
  type: AddInputList
}