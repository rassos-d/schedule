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

/* export type Squad = {
  name: string,
  studyYear: number,
  daddyId: string,
  daddy: string
  fixedAudienceId: string,
  fixedAudience: string
  directionId: string,
  direction: string
  id: string
} */