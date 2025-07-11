import { memo, useEffect, useRef } from 'react'
import styles from './drop.module.scss'
import { useDrag, useDrop } from 'react-dnd'
import { Lesson as TLesson } from '../../types/shedule'

type LessonProps = {
  lesson: TLesson;
  onMove: (target: DropResult, date: string, number: number) => void;
  onStartDragging: () => void
  isDraggingAnything: boolean
  date: string
  number: number
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
} | { activeSquardIndex: number } | {date: string, number: number, lesson: TLesson}

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
  }), [squardIndex]);

  const [, drop] = useDrop(() => ({
    accept: [`LESSON-${squardIndex}`, 'FREE'],
    drop: () => {
      return {
        date: date,
        number: number,
        lesson: lesson
      }
    },
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
    }),
  }), [squardIndex]);

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
        <p>т 8/2 лек</p>
        <p>ВО-404</p>
        <p>п-к Кизюн Н.Н.</p>
      </div>
      {/* <ChangeZone isDisplay={isDraggingAnything} date={date} number={number} lesson={lesson} squardIndex={squardIndex}/> */}
    </div>
  );
}

type ChangeZoneProps = {
  squardIndex: number
  date: string
  number: number
  lesson: TLesson;
  isDisplay: boolean
}

function ChangeZone({squardIndex, date, number, lesson, isDisplay}: ChangeZoneProps) {
  const [{ isOver }, drop] = useDrop(() => ({
    accept: [`LESSON-${squardIndex}`, 'FREE'],
    drop: () => {
      return {
        date: date,
        number: number,
        lesson: lesson
      }
    },
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
    }),
  }), [squardIndex, isDisplay]);

  return <div style={{zIndex: isDisplay ? '2' : '-1', background: isOver ? "#ddd" : "transparent"}} className={styles.changeZone} ref={drop}></div>;
}

export const DragLesson = memo(LessonComponent)