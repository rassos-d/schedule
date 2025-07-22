import { AddInputList } from "./input"

export type Lesson = {
  name: string
  subjectId: string,
  themeId: string,
  id: string
  type: number
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
  lessonType?: AddInputList
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
  lesson?: AddInputList & {number: number, type: string, lessonType: number},
  squad: AddInputList,
  theme?: AddInputList & {number: number},
  subject: AddInputList,
  number: number,
  date: string
}