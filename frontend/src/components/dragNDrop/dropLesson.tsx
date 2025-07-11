import { memo } from "react";
import { useDrop } from "react-dnd";
import styles from './drop.module.scss'

type DropLessonProps = {
  date: string;
  number: number;
  squardIndex: number
};

function DropLessonComponent({ date, number, squardIndex }: DropLessonProps) {
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

  return <div className={styles.dropLesson} ref={drop} style={{ background: isOver ? "#ddd" : "transparent"  }}></div>;
}

export const DropLesson = memo(DropLessonComponent);