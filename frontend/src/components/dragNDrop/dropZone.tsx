import styles from './drop.module.scss'
import { useDrop } from "react-dnd";

type DropZoneProps = {
  children: JSX.Element
  activeSquardIndex: number 
}

export function DropZone ({children, activeSquardIndex}: DropZoneProps) {
  const [{ isOver }, drop] = useDrop(() => ({
      accept: ['FREE', `LESSON-${activeSquardIndex}`],
      drop: () => {
        return {
          activeSquardIndex: activeSquardIndex
        }
      },
      collect: (monitor) => ({
        isOver: !!monitor.isOver(),
      }),
    }), [activeSquardIndex]);

  return (
    <div ref={drop} className={styles.dropZone} style={{ background: isOver ? "#ddd" : "transparent"  }}>
      {children}
    </div>
  )
}