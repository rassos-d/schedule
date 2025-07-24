import { Lesson } from "./lesson"

export type Theme = {
  id: string
  subjectId: string
  lessons: Lesson[]
  number: number
}

export type NewTheme = {
  number?: number
}