import { AddInputList } from "./input"

export type Squad = {
  name: string,
  studyYear: number,
  daddyId: string,
  fixedAudienceId: string,
  directionId: string,
  id: string
}

export type EditSquad = {
  id: string
  name: string
  studyYear?: AddInputList
  daddy?: AddInputList
  fixedAudience?: AddInputList
  direction?: AddInputList
}

export type NewSquad = Omit<EditSquad, "id">

export type StatisticSquad = {
  id: string
  name: string
  teacherId: string
  fixedAudienceId: string
  subjects: {
    id: string
    name: string
    plannedHours: number
    completedHours: number
    missingLessons: MissingLesson[]
  }[]
}

export type MissingLesson = {
  lessonId: string,
  lessonNumber: number,
  themeId: string,
  themeNumber: number,
  hoursCount: number
}