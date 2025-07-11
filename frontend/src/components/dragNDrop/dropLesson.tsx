import { memo, useState } from "react";
import { useDrop } from "react-dnd";
import styles from './drop.module.scss'
import { Icon } from "../icon";

type DropLessonProps = {
  date: string;
  number: number;
  squardIndex: number
  onCreateLesson: (day: string, number: number, squardIndex: number) => void
};

function DropLessonComponent({ date, number, squardIndex, onCreateLesson }: DropLessonProps) {

  const [isHover, setIsHover] = useState(false)

  const [{ isOver }, drop] = useDrop(() => ({
    accept: [`LESSON-${squardIndex}`, 'FREE'],
    drop: () => {
      return {
        date: date,
        number: number
      }
    },
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
    }),
  }));

  return <div 
      onMouseEnter={()=>setIsHover(true)} 
      onMouseLeave={()=>setIsHover(false)} 
      className={styles.dropLesson} 
      ref={drop} 
      style={{ background: isOver ? "#ddd" : "transparent"  }}
    >
    {isHover && <div onClick={()=>onCreateLesson(date, number, squardIndex)}><Icon glyph="add" glyphColor="black" size={45}/></div>}
  </div>;
}

export const DropLesson = memo(DropLessonComponent);