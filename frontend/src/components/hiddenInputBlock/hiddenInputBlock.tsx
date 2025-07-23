import { useState } from 'react'
import styles from './hiddenInputBlock.module.scss'
import { Icon } from '../icon'
import { HiddenInput } from '../input/Input'

type HiddenInputBlockProps = {
  value: string
  isWarning?: boolean
  isEdit?: boolean
  onEnter?: (newValue: string) => void
  onEdit: () => void
  onDelete: () => void
  onSelect?: () => void
}

export function HiddenInputBlock ({value, isEdit, isWarning, onEnter, onDelete, onEdit, onSelect}:HiddenInputBlockProps) {

  const [isHover, setIsHover] = useState(false)

  const confirmChanges = (newValue: string) => {
    onEnter && onEnter(newValue)
  }

  return (
    <div onMouseEnter={()=>{!isEdit && setIsHover(true)}} onMouseLeave={()=>{!isEdit && setIsHover(false)}} className={styles.hiddenInput}>
      {isEdit ? 
        <HiddenInput isWarning={isWarning} value={value} onEnter={confirmChanges} />
        : 
        <div onClick={onSelect ? onSelect : onEdit} className={styles.hiddenInput__line}>
          <p>{value}</p>
          {isHover && <div className={styles.hiddenInput__icons}>
            <Icon onClick={(e)=>{e.stopPropagation(); onEdit()}} glyph='edit' glyphColor='black' size={16}/>
            <Icon onClick={(e)=>{e.stopPropagation(); onDelete()}} glyph='close' glyphColor='black' size={12}/>
          </div>}
        </div>
      }
    </div>
  )
}