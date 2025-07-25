import { AddInputList } from "../types/input"

export const COURSES_YEAR = [1, 2, 3, 4, 5, 6]
export const SEMESTER_COUNT = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]

export const SEMESTR_YEAR:AddInputList[] = [{name: 'Осенний', id: 1}, {name: 'Весенний', id: 0}]

export const LESSON_TYPE = [
  {name: 'Групповое', shortName: 'г.з.'},
  {name: 'Лекция', shortName: 'лек.'},
  {name: 'Практика', shortName: 'п.з.'},
  {name: 'Семинар', shortName: 'сем.'},
  {name: 'Тренировка', shortName: 'трен.'},
  {name: 'Самоподготовка', shortName: 'СРС'}
]

export const DIRECTION_TYPE = ['Кадровые', 'Запас']

export const WEEK_DAYS = ['Воскресенье', "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"]