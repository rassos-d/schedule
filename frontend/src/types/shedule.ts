import { Lesson } from "./lesson"

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


export type ScheduleResponse = {
  scheduleId: string
  studyYear: number
  dates: string[]
  squads: string[]
  events: Event[]
}

export type Shedule = {
  id: string
  name: string
  squads: Squard[] 
}

type Squard = {
  id: string
  name: string
  events: Event
  noname?: Lesson[]
}

export type Event = Record<string, (Lesson | {number: number})[]>