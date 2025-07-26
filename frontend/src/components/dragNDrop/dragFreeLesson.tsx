import { memo, useEffect, useRef, useState } from 'react'
import styles from './drop.module.scss'
import { useDrag } from 'react-dnd'
import { FreeLesson } from '../../types/lesson'
import { LESSON_TYPE } from '../../consts'
import { Icon } from '../icon'

type LessonProps = {
  lesson: FreeLesson;
  enableColor: boolean
  onMove: (target:DropResult, lesson: FreeLesson) => void;
  onDragging: (squardId: string) => void
  onStopDragging: () => void
  onDelete: () => void
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
}

function LessonComponent({ lesson, squardIndex, enableColor, onMove, onDelete, onDragging, onStopDragging }: LessonProps) {

  const ref = useRef<HTMLDivElement>(null)

  const [isHover, setIsHover] = useState(false)

  const [{ isDragging }, drag] = useDrag(() => ({
    type: `LESSON-${squardIndex}`,
    item: lesson,
    end: (item, monitor) => {
      const dropResult = monitor.getDropResult<DropResult>();
      onStopDragging()
      if (item && dropResult) {
        onMove(dropResult, lesson);
      }
    },
    collect: (monitor) => {
      if (monitor.isDragging()) {
        onDragging(lesson.squad.id.toString())
      }
      return {isDragging: !!monitor.isDragging()}
    },
  }), [squardIndex]);

  const addClass = () => {
    if (ref.current) {
      ref.current.classList.add(styles.dragLessonContainer_new);

      setTimeout(() => {
        if (ref.current) {
          ref.current.classList.remove(styles.dragLessonContainer_new);
        }
      }, 1000);
    }
  }

  useEffect(()=>{
    if (ref.current && lesson.isUpdate) {
      addClass()
    }
  },[ref, lesson])



  return (
    <div 
      onMouseEnter={()=>{setIsHover(true)}} 
      onMouseLeave={()=>setIsHover(false)}  
      ref={drag} 
      className={styles.freeLesson} 
      style={{ opacity: isDragging ? 0.5 : 1, backgroundColor: enableColor ? lesson.subject.color : '' } }>
        {lesson.lesson && lesson.lesson.lessonType === 5 ? LESSON_TYPE[lesson.lesson.lessonType].shortName : ''}
        <p>{lesson.subject.name}</p>
        <p>{`
          ${lesson.theme ? 'т. ' + lesson.theme.number + '/' : ''}${lesson.lesson && lesson.lesson.number !== undefined ? lesson.lesson.number : ''}
          ${lesson.lesson && lesson.lesson.lessonType !== 5 ? LESSON_TYPE[lesson.lesson.lessonType].shortName : ''} `}</p>
        <p>{lesson.audience?.name}</p>
        <p>{lesson.teacher?.name}</p>
        {isHover && <div onClick={(e)=>{e.stopPropagation();onDelete()}} className={styles.dragLessonContainer__delete}><Icon size={16} glyph='trash' glyphColor='error'/></div>}
    </div>
  );
}

export const DragFreeLesson = memo(LessonComponent)