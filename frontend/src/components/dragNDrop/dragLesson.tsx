import { memo, useEffect, useRef } from 'react'
import styles from './drop.module.scss'
import { useDrag, useDrop } from 'react-dnd'
import { SheduleLesson } from '../../types/lesson'

type LessonProps = {
  lesson: SheduleLesson;
  onMove: (target: DropResult, date: string, number: number) => void;
  onStartDragging: () => void
  date: string
  number: number
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
  lesson?: SheduleLesson
} | { activeSquardIndex: number }

function LessonComponent({ lesson, date, number, squardIndex, onMove, onStartDragging }: LessonProps) {

  const ref = useRef(null)

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
  }), [squardIndex, number, squardIndex, lesson]);

  const [, drop] = useDrop(() => ({
    accept: [`LESSON-${squardIndex}`, 'FREE'],
    drop: () => {
      return {
        date,
        number,
        lesson
      }
    },
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
    }),
  }), [squardIndex, number, squardIndex, lesson]);

  drag(drop(ref))

  useEffect(()=>{
    if (isDragging) {
      onStartDragging()
    }
  }, [isDragging])

  return (
    <div className={styles.dragLessonContainer}>
      <div ref={ref} style={{ opacity: isDragging ? 0.5 : 1 }}>
        <p>ТСП</p>
        <p>{lesson.lessonName}</p>
        <p>{lesson.audienceName}</p>
        <p>{lesson.teacherName}</p>
      </div>
    </div>
  );
}

export const DragLesson = memo(LessonComponent)