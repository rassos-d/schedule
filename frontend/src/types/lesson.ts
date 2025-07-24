import { AddInputList } from "./input"

export type Lesson = {
  name: string
  subjectId: string,
  themeId: string,
  id: string
  type: number
  number: number
  semester: number
}

export type EditLesson = {
  id: string
  type: AddInputList
  number: number
  semester: AddInputList
}

export type NewSmallLesson = {
  name?: string
  type?: AddInputList
  number?: number
  semester?: AddInputList
}

export type FreeLesson = Omit<SheduleLesson, "number" | "date"> & {squardIndex: number}

export type NewLesson = {
  id?: string
  date?: string
  number?: number
  squad?: AddInputList
  lesson?: AddInputList
  teacher?: AddInputList
  audience?: AddInputList
  theme?:AddInputList
  subject?: AddInputList
}

export type NewLessonRequest = {
  subjectId: string,
  themeId: string,
  lessonType?: number
  lessonId: string,
  squadId: string,
  teacherId: string,
  audienceId: string,
  number: number,
  date: string
}

export type SheduleLesson = {
  id: string,
  teacher?: AddInputList,
  audience?: AddInputList,
  lesson: {number: number, type: string, lessonType: number, id: string},
  squad: AddInputList,
  theme: {number: number, id: string},
  subject: AddInputList,
  number: number,
  date: string
  isUpdate: boolean
}