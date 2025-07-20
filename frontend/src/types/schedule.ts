import { SheduleLesson } from "./lesson"

export type SmallShedule = {
  id: string,
  name: string
}

export type CreateSchedule = {
  name: string
  pages: CreateScheduleYear[]
}

export type CreateScheduleYear = {
  studyYear: number, 
  squads: ScheduleSquad[], 
  start: string, 
  end: string
}

export type ScheduleSquad = {name: string, id: string}


export type Schedule = {
  scheduleId: string
  name: string
  squads: Squad[]
  noName: Omit<SheduleLesson, 'number' | 'date'>[]
}

type Squad = {
  id: string
  name: string
  teacherName?: string
  directionName?: string
  audienceName?: string
  events: Event
}

export type ChangeLessonReponse = {
  conflictEventIds: string[]
  message?: string
}

export type Event = Record<string, (SheduleLesson | {number: number})[]>