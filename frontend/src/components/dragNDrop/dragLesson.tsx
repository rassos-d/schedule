import { memo, useEffect, useRef, useState } from 'react'
import styles from './drop.module.scss'
import { useDrag, useDrop } from 'react-dnd'
import { SheduleLesson } from '../../types/lesson'
import { Icon } from '../icon'
import { LESSON_TYPE } from '../../consts'

type LessonProps = {
  lesson: SheduleLesson;
  isConflict?: boolean
  isNew?: boolean
  onMove: (target: DropResult, date: string, number: number) => void;
  onStartDragging: (squardIndex:  number) => void
  checkOpenStash: (y: number) => void
  onSelect: (lesson: SheduleLesson) => void
  onDelete: () => void
  onDragging: (squardId: string) => void
  onStopDragging: () => void
  enableColor?: boolean
  date: string
  number: number
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
  lesson?: SheduleLesson
} | { activeSquardIndex: number }

function LessonComponent({ lesson, date, number, squardIndex, isConflict, isNew, enableColor, onMove, onStartDragging, onSelect, onDelete, onDragging, onStopDragging, checkOpenStash }: LessonProps) {

  const ref = useRef<HTMLDivElement>(null)
  
  const [isHover, setIsHover] = useState(false)

  const addClass = () => {
    if (ref.current) {
      ref.current.classList.add(styles.dragLessonContainer_new);

      setTimeout(() => {
        if (ref.current) {
          ref.current.classList.remove(styles.dragLessonContainer_new);
        }
      }, 2000);
    }
  }

  const [{ isDragging }, drag] = useDrag(() => ({
    type: `LESSON-${squardIndex}`,
    item: lesson,
    end: (item, monitor) => {
      const dropResult = monitor.getDropResult<DropResult>();
      onStopDragging()
      if (item && dropResult) {
        onMove(dropResult, date, number);
      }
    },
    collect: (monitor) => {
      if (monitor.isDragging()) {
        onDragging(lesson.squad.id.toString())
      }
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

  const checkDragging = (e: MouseEvent) => {
    if (isDragging) {
      checkOpenStash(e.clientY)
    }
  }

  useEffect(()=>{
    if (isDragging) {
      onStartDragging(squardIndex)
    }
  }, [isDragging])

  useEffect(()=>{
    if (ref.current && lesson.isUpdate || isNew) {
      addClass()
    }
  },[])

  useEffect(()=>{
    document.addEventListener('drag', checkDragging)
  },[isDragging])


  return (
    <div 
      ref={ref}
      style={enableColor ? {backgroundColor: lesson.subject?.color} : {} }
      onMouseEnter={()=>{setIsHover(true)}} 
      onMouseLeave={()=>setIsHover(false)} 
      onClick={()=>onSelect(lesson)} 
      className={`${styles.dragLessonContainer} ${isConflict && styles.dragLessonContainer_error} `}
    >
      <div className={styles.dragLessonContainer__content} style={{ opacity: isDragging ? 0.5 : 1 }}>
        {lesson.lesson && lesson.lesson.lessonType === 5 ? LESSON_TYPE[lesson.lesson.lessonType].shortName : ''}
        <p>{lesson.subject?.name}</p>
        <p>{`
          ${lesson.theme ? 'т. ' + lesson.theme.number + '/' : ''}${lesson.lesson && lesson.lesson.number !== undefined ? lesson.lesson.number : ''}
          ${lesson.lesson && lesson.lesson.lessonType !== 5 ? LESSON_TYPE[lesson.lesson.lessonType].shortName : ''} `}</p>
        <p>{lesson.audience?.name}</p>
        <p>{lesson.teacher?.name}</p>
      </div>
      {isHover && <div onClick={(e)=>{e.stopPropagation();onDelete()}} className={styles.dragLessonContainer__delete}><Icon size={16} glyph='trash' glyphColor={isConflict ? 'black' : 'error'}/></div>}
    </div>
  );
}

export const DragLesson = memo(LessonComponent)