import { useState } from 'react'
import styles from './hiddenInputBlock.module.scss'
import { Icon } from '../icon'
import { HiddenInput } from '../input/Input'

type HiddenInputBlockProps = {
  value: string
  onEnter?: (newValue: string) => void
  onEdit?: () => void
  onDelete: () => void
}

export function HiddenInputBlock ({value, onEnter, onDelete, onEdit}:HiddenInputBlockProps) {

  const [isEdit, setIsEdit] = useState(false)
  const [isHover, setIsHover] = useState(false)

  const confirmChanges = (newValue: string) => {
    if (value !== newValue && onEnter) {
      onEnter(newValue)
    }
    setIsEdit(false)
  }

  return (
    <div onMouseEnter={()=>{!isEdit && setIsHover(true)}} onMouseLeave={()=>{!isEdit && setIsHover(false)}} className={styles.hiddenInput}>
      {isEdit ? 
        <HiddenInput  value={value} onEnter={confirmChanges} />
        : 
        <div onClick={onEdit ? onEdit : ()=>{setIsEdit(true)}} className={styles.hiddenInput__line}>
          <p>{value}</p>
          {isHover && <div className={styles.hiddenInput__icons}>
            <Icon onClick={onEdit ? onEdit : ()=>{setIsEdit(true)}} glyph='edit' glyphColor='black' size={16}/>
            <Icon onClick={(e)=>{e.stopPropagation(); onDelete()}} glyph='close' glyphColor='black' size={12}/>
          </div>}
        </div>
      }
    </div>
  )
}