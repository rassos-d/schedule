import { memo } from 'react'
import styles from './vacationBlock.module.scss'
import { Icon } from '../icon'
import { Input } from '../input/Input'
import { Vacation } from '../../types/teacher'

type VacationBlockProps = {
  start: string
  end: string
  title: string
  onChangeDate: (newVacation: Vacation) => void
  onDelete: () => void
}

function VacationBlockComponent({ start, end, title, onChangeDate, onDelete }: VacationBlockProps) {
  return (
    <div className={styles.container}>
      <h5>{title}</h5>
      <div className={styles.content}>
        <div className={styles.content__dates}>
          <div className={styles.content__date}>
            <p>Дата начала</p>
            <Input type='date' value={start} onChange={(newValue) => onChangeDate({startDate: newValue, endDate: end})} />
          </div>
          <div className={styles.content__date}>
            <p>Дата конца</p>
            <Input type='date' value={end} onChange={(newValue) => onChangeDate({startDate: start, endDate: newValue})} />
          </div>
        </div>
        <div onClick={onDelete} className={styles.content__close}>
          <Icon glyph='trash' glyphColor='black' />
        </div>
      </div>
    </div>
  )
}

export const VacationBlock = memo(VacationBlockComponent)