import { memo, useEffect } from 'react'
//import styles from './drop.module.scss'
import { useDrag } from 'react-dnd'
import { Lesson as TLesson } from '../../types/shedule'

type LessonProps = {
  lesson: TLesson;
  onMove: (target: DropResult, date: string, number: number) => void;
  onStartDragging: () => void
  date: string
  number: number
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
} | { activeSquardIndex: number }

function LessonComponent({ lesson, date, number, squardIndex, onMove }: LessonProps) {

  const [{ isDragging }, drag] = useDrag(() => ({
    type: `LESSON-${squardIndex}`,
    item: lesson,
    end: (item, monitor) => {
      const dropResult = monitor.getDropResult<DropResult>();
      if (item && dropResult) {
        onMove(dropResult, date, number);
      }
    },
    collect: (monitor) => {
      return {isDragging: !!monitor.isDragging()}
    },
  }));

  useEffect(()=>{
    if (isDragging) {
      //onStartDragging()
    }
  }, [isDragging])

  return (
    <div ref={drag} style={{ opacity: isDragging ? 0.5 : 1 }}>
      <p>ТСП</p>
      <p>т 8/2 лек</p>
      <p>ВО-404</p>
      <p>п-к Кизюн Н.Н.</p>
    </div>
  );
}

export const DragLesson = memo(LessonComponent)