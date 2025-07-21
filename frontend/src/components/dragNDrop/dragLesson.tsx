import { memo, useEffect, useRef, useState } from 'react'
import styles from './drop.module.scss'
import { useDrag, useDrop } from 'react-dnd'
import { SheduleLesson } from '../../types/lesson'
import { Icon } from '../icon'

type LessonProps = {
  lesson: SheduleLesson;
  isConflict?: boolean
  onMove: (target: DropResult, date: string, number: number) => void;
  onStartDragging: (squardIndex:  number) => void
  onSelect: (lesson: SheduleLesson) => void
  onDelete: () => void
  date: string
  number: number
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
  lesson?: SheduleLesson
} | { activeSquardIndex: number }

function LessonComponent({ lesson, date, number, squardIndex, isConflict, onMove, onStartDragging, onSelect, onDelete }: LessonProps) {

  const ref = useRef<HTMLDivElement>(null)
  
  const [isHover, setIsHover] = useState(false)

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
  }), [squardIndex, number, lesson]);

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
  }), [squardIndex, number, lesson]);

  drag(drop(ref))

  useEffect(()=>{
    if (isDragging) {
      onStartDragging(squardIndex)
    }
  }, [isDragging])

  return (
    <div 
      onMouseEnter={()=>{setIsHover(true)}} 
      onMouseLeave={()=>setIsHover(false)} 
      onClick={()=>onSelect(lesson)} 
      className={`${styles.dragLessonContainer} ${isConflict && styles.dragLessonContainer_error}`}
    >
      <div ref={ref} className={styles.dragLessonContainer__content} style={{ opacity: isDragging ? 0.5 : 1 }}>
        <p>{lesson.subject.name}</p>
        <p>{`${lesson.theme ? 'т. ' + lesson.theme.number + '/' : ''}${lesson.lesson ? lesson.lesson.number + ' ' + lesson.lesson.type : ''} `}</p>
        <p>{lesson.audience?.name}</p>
        <p>{lesson.teacher?.name}</p>
      </div>
      {isHover && <div onClick={(e)=>{e.stopPropagation();onDelete()}} className={styles.dragLessonContainer__delete}><Icon size={16} glyph='trash' glyphColor='error'/></div>}
    </div>
  );
}

export const DragLesson = memo(LessonComponent)