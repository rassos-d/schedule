import { toast } from "react-toastify"
import { NewLesson } from "../types/lesson"
import { CreateSchedule } from "../types/schedule"

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
  for (const page of schedule.pages) {
    if (page.end.length === 0 || page.start.length === 0 || page.squads.length === 0 || !page.semester) {
      toast('Заполните все поля')
      return false
    }
  }
}