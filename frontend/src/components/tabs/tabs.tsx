import { memo } from 'react'
import styles from './tabs.module.scss'

type TabsProps = {
  tabs: readonly number[]
  onClick: (nextTab: number) => void
  activeTab: number
}

function TabsComponent ({tabs, activeTab, onClick}:TabsProps) {
  return (
    <div className={styles.container}>
      {tabs.map((tab)=>(
        <p onClick={()=>onClick(tab)} className={`${styles.container__tab} ${activeTab === tab && styles.container__tab_active}`} key={tab}>{tab}</p>
      ))}
    </div>
  )
}

export const Tabs = memo(TabsComponent)