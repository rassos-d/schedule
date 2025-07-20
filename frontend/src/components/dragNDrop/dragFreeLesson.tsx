import { memo, useEffect } from 'react'
import styles from './drop.module.scss'
import { useDrag } from 'react-dnd'
import { FreeLesson } from '../../types/lesson'

type LessonProps = {
  lesson: FreeLesson;
  onMove: (target:DropResult, lesson: FreeLesson) => void;
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
}

function LessonComponent({ lesson, squardIndex, onMove }: LessonProps) {

  const [{ isDragging }, drag] = useDrag(() => ({
    type: `LESSON-${squardIndex}`,
    item: lesson,
    end: (item, monitor) => {
      const dropResult = monitor.getDropResult<DropResult>();
      if (item && dropResult) {
        onMove(dropResult, lesson);
      }
    },
    collect: (monitor) => {
      return {isDragging: !!monitor.isDragging()}
    },
  }), [squardIndex]);

  useEffect(()=>{
    if (isDragging) {
      //onStartDragging()
    }
  }, [isDragging])

  return (
    <div ref={drag} className={styles.freeLesson} style={{ opacity: isDragging ? 0.5 : 1 }}>
      <p>ТСП</p>
      <p>{lesson.lessonName}</p>
      <p>{lesson.audienceName}</p>
      <p>{lesson.teacherName}</p>
    </div>
  );
}

export const DragFreeLesson = memo(LessonComponent)