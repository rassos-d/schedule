import { Lesson } from "./lesson"

export type Theme = {
  id: string
  name: string
  subjectId: string
  lessons: Lesson[]
  number: number
}

export type NewTheme = {
  number?: number
  name?: string
}