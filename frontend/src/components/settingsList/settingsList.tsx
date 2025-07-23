import { memo, useEffect, useState } from 'react'
import styles from './settingsList.module.scss'
import { Icon } from '../icon'

type SettingsListProps = {
  children: JSX.Element
  title: string
  isOpenList?: boolean
  changeIsOpen?: (newOpen: boolean) => void
}

function SettingsListComponent ({children, title, isOpenList, changeIsOpen}:SettingsListProps) {

  const [isOpen, setIsOpen] = useState(false)

  useEffect(()=>{
    setIsOpen(isOpenList!==undefined ? isOpenList : false)
  }, [isOpenList])

  return (
    <div className={styles.container}>
      {isOpen ? 
        <div className={styles.container__visible}>
          <div onClick={()=>{setIsOpen(false);changeIsOpen && changeIsOpen(false)}} className={`${styles.container__title} ${styles.container__title_active}`}>
            <p>{title}</p>
            <Icon glyph='arrow-up' glyphColor='white'/>
          </div>
          <div className={styles.container__content}>
            {children}
          </div>
        </div> : 
        <div onClick={()=>{setIsOpen(true);changeIsOpen && changeIsOpen(true)}} className={styles.container__title}>
          <p>{title}</p>
          <Icon glyph='arrow-down' glyphColor='black'/>
        </div>
      }
    </div>
  )
}

export const SettingsList = memo(SettingsListComponent)