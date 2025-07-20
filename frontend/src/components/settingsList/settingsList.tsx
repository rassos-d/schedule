import { memo, useState } from 'react'
import styles from './settingsList.module.scss'
import { Icon } from '../icon'

type SettingsListProps = {
  children: JSX.Element
  title: string
}

function SettingsListComponent ({children, title}:SettingsListProps) {

  const [isOpen, setIsOpen] = useState(false)

  return (
    <div className={styles.container}>
      {isOpen ? 
        <div className={styles.container__visible}>
          <div onClick={()=>setIsOpen(false)} className={`${styles.container__title} ${styles.container__title_active}`}>
            <p>{title}</p>
            <Icon glyph='arrow-up' glyphColor='white'/>
          </div>
          <div className={styles.container__content}>
            {children}
          </div>
        </div> : 
        <div onClick={()=>setIsOpen(true)} className={styles.container__title}>
          <p>{title}</p>
          <Icon glyph='arrow-down' glyphColor='black'/>
        </div>
      }
    </div>
  )
}

export const SettingsList = memo(SettingsListComponent)