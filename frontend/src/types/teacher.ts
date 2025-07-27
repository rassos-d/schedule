import { AddInputList } from "./input"

export type Teacher = {
  name: string,
  rank: string,
  vacations: Vacation[],
  subjectIds: string[],
  id: string
}

export type NewTeacher = {
  name: string
  rank: string
  vacations: Vacation[],
  subjects: AddInputList[],
}

export type Vacation = {
  startDate: string
  endDate: string
}

export type StatisticTeacher = {
  id: string,
  name: string,
  rank: string,
  hoursCount: number,
  subjectsCount: number
}