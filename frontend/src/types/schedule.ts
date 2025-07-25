import { AddInputList } from "./input"
import { SheduleLesson } from "./lesson"

export type SmallShedule = {
  id: string,
  name: string
}

export type YearDays = {
  dayOfWeek: string
  studyYear: number
}

export type CreateSchedule = {
  name: string
  id?: string
  semester?: AddInputList
  pages: CreateScheduleYear[]
}

export type CreateScheduleYear = {
  squads: ScheduleSquad[], 
  start: string, 
  end: string
}

export type UpdateSchedule = {
  id: string
  name: string
  semester: number
  pages: UpdateScheduleYear[]
}

export type UpdateScheduleYear = {
  studyYear: number, 
  squads: string[], 
  start: string, 
  end: string
}

export type ScheduleSquad = {name: string, id: string}


export type Schedule = {
  scheduleId: string
  semester: number
  name: string
  squads: Squad[]
  noName: Omit<SheduleLesson, 'number' | 'date'>[]
  conflicts: Conflict
}

type Squad = {
  id: string
  name: string
  daddy?: AddInputList
  direction?: AddInputList
  audience?: AddInputList
  events: Event
}

export type ChangeLessonReponse = {
  conflictEventIds: string[]
  message?: string
}

export type Conflict = {
  message?: string
  conflictEventIds: string[]
}

export type Event = Record<string, (SheduleLesson | {number: number})[]>