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
}

export type Vacation = {
  startDate: string
  endDate: string
}