import { Lesson } from "./lesson"

export type Theme = {
  id: string
  name: string
  subjectId: string
  lessons: Lesson[]
}