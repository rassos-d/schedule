import { memo, useEffect, useRef, useState } from 'react'
import styles from './drop.module.scss'
import { useDrag } from 'react-dnd'
import { FreeLesson } from '../../types/lesson'
import { LESSON_TYPE } from '../../consts'
import { Icon } from '../icon'

type LessonProps = {
  lesson: FreeLesson;
  onMove: (target:DropResult, lesson: FreeLesson) => void;
  onDelete: () => void
  squardIndex: number
}


type DropResult = {
  date: string;
  number: number;
}

function LessonComponent({ lesson, squardIndex, onMove, onDelete }: LessonProps) {

  const ref = useRef<HTMLDivElement>(null)

  const [isHover, setIsHover] = useState(false)

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
    <div onMouseEnter={()=>{setIsHover(true)}} onMouseLeave={()=>setIsHover(false)}  ref={drag} className={styles.freeLesson} style={{ opacity: isDragging ? 0.5 : 1 }}>
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