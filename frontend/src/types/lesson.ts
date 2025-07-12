export type LessonResponse = {
  name: string,
  type: 2,
  subjectId: string,
  themeId: string,
  id: string
}

export type Lesson = {
  name: string
  subject: string
  theme: string
  subjectId: string,
  themeId: string,
  id: string
}

export type FreeLesson = Omit<Lesson, "number"> & {squardIndex: number}

export type NewLesson = {
  date: string
  number: number
  squardIndex: number
  lesson_name?: string
  teacher_name?: string
  audience_nane?: string
}