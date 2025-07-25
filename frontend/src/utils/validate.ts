import { toast } from "react-toastify"
import { NewLesson } from "../types/lesson"
import { CreateSchedule } from "../types/schedule"
import { NewTeacher } from "../types/teacher"

export function checkLesson(lesson: NewLesson) : lesson is Required<NewLesson> {
  for (const value of Object.values(lesson)) {
    if (value === undefined) return false
  }
  return true
}

export function isValidCreateSchedule (schedule: CreateSchedule) {
  if (schedule.pages.length === 0) {
    toast('Добавьте год обучения')
    return false
  }
  if (!schedule.semester) {
    return false
  }
  for (const page of schedule.pages) {
    if (page.end.length === 0 || page.start.length === 0 || page.squads.length === 0) {
      toast('Заполните все поля')
      return false
    }
  }
  return true
}

export function isValidEditTeacher (teacher: NewTeacher) {
  if (teacher.name.length === 0 || teacher.rank.length === 0) return false
  for (const vacation of teacher.vacations) {
    if (vacation.endDate.length === 0 || vacation.startDate.length === 0) return false
  }
  return true
}