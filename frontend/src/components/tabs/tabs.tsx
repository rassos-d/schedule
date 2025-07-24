import { memo } from 'react'
import styles from './tabs.module.scss'
import {YearDays} from "../../types/schedule.ts";

type TabsProps = {
  tabs: readonly YearDays[]
  onClick: (nextTab: YearDays) => void
  activeTab: YearDays
}

function TabsComponent ({tabs, activeTab, onClick}:TabsProps) {
  return (
    <div className={styles.container}>
      {tabs.map((tab)=>(
        <p onClick={()=>onClick(tab)} className={`${styles.container__tab} ${activeTab.studyYear === tab.studyYear && styles.container__tab_active}`} key={tab.dayOfWeek}>{tab.dayOfWeek}</p>
      ))}
    </div>
  )
}

export const Tabs = memo(TabsComponent)