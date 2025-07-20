import { SheduleLesson } from "../types/lesson";
import { Event, Schedule } from "../types/schedule";

export function getSchedule (events: Event) {
  let result: Record<string, (SheduleLesson | {number: number})[]> = {} 
  for (const key in events) {
    let sortedDay = events[key]
    const existingNumbers = new Set(sortedDay.map(item => item.number))
    let resultDay:(SheduleLesson | {number: number})[]  = [...sortedDay]  
    for (let num = 1; num < 6; num++){
      if (!existingNumbers.has(num)) {
        resultDay.push({number: num})
      }
    }
    result[key] = resultDay.sort((a, b)=>a.number - b.number)
  }
  return result  
}

export function getFullSchedule (shedule:Schedule) {
  const newShedule:Schedule = JSON.parse(JSON.stringify(shedule))
  for (const squard of newShedule.squads) {
    squard.events = getSchedule(squard.events)
  }
  return newShedule
}

export function sortedDates(unsortedData: Event) {
  const obj = Object.entries(unsortedData).sort(([a], [b]) => {
    const [yearA, monthA, dayA] = a.split('-').map(Number);
    const [yearB, monthB, dayB] = b.split('-').map(Number);
    return yearA - yearB || monthA - monthB || dayA - dayB;
  });
  return obj;
} 