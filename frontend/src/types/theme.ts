import { Lesson } from "./lesson"

export type Theme = {
  name: string
  subjectId: string
  lessons: Lesson[]
}