export type Shedule = {
  id: string
  name: string
  squards: Squard[] 
}

type Squard = {
  id: string
  name: string
  events: Event
  noname?: Lesson[]
}

export type Event = Record<string, (Lesson | {number: number})[]>

export type Lesson = {
  lesson_id: string
  lesson_name: string
  teacher_id: string
  teacher_name: string
  audience_name: string
  number: number
}

export type FreeLesson = Omit<Lesson, "number"> & {squardIndex: number}