import { Event, Lesson, Shedule } from "../types/shedule";

export function getShedule (events: Event) {
  let result: Record<string, (Lesson | {number: number})[]> = {} 
  for (const key in events) {
    let sortedDay = events[key].sort((a, b)=>a.number - b.number)
    const existingNumbers = new Set(sortedDay.map(item => item.number))
    let resultDay:(Lesson | {number: number})[]  = [...sortedDay]  
    for (let num = 1; num < 6; num++){
      if (!existingNumbers.has(num)) {
        resultDay.push({number: num})
      }
    }
    result[key] = resultDay.sort((a, b)=>a.number - b.number)
  }
  return result
}

export function getFullShedule (shedule:Shedule) {
  const newShedule:Shedule = JSON.parse(JSON.stringify(shedule))
  for (const squard of newShedule.squards) {
    squard.events = getShedule(squard.events)
  }
  return newShedule
}