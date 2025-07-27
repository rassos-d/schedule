import { useState } from 'react'
import PopupContainer from '../popupContainer/popupContainer'
import styles from './statistic.module.scss'
import { Button } from '../button/button'

type StatisticProps = {
  onClose: () => void
  onCreateLesson: () => void
}

const TABS = ['Преподаватели', 'Взвода']

export function Statistic ({onClose}:StatisticProps) {

  const [selectedTab, setSelectedTab] = useState<string>(TABS[0])

  return (
    <PopupContainer onClose={onClose} displayClose>
      <div className={styles.container}>
        <div className={styles.container__tabs}>
          {TABS.map((tab)=>(
            <p onClick={()=>setSelectedTab(tab)} className={`${styles.container__tab} ${selectedTab === tab && styles.container__tab_active}`} key={tab}>{tab}</p>
          ))}
        </div>
        {selectedTab === 'Преподаватели' ? 
          <div className={styles.table}>
              <div className={`${styles.table__line} ${styles.table__line_teacher}`}> 
                <h5 className={styles.table__title}>ФИО</h5>
                <h5 className={styles.table__title}>Количество часов</h5>
                <h5 className={styles.table__title}>Количество дисциплин</h5>
              </div>
              <div className={`${styles.table__line} ${styles.table__line_teacher}`}> 
                <p className={styles.table__item}>ФИО</p>
                <p className={styles.table__item}>Количество часов</p>
                <p className={styles.table__item}>Количество дисциплин</p>
              </div>
              <div className={`${styles.table__line} ${styles.table__line_teacher}`}> 
                <p className={styles.table__item}>ФИО</p>
                <p className={styles.table__item}>Количество часов</p>
                <p className={styles.table__item}>Количество дисциплин</p>
              </div>
              <div className={`${styles.table__line} ${styles.table__line_teacher}`}> 
                <p className={styles.table__item}>ФИО</p>
                <p className={styles.table__item}>Количество часов</p>
                <p className={styles.table__item}>Количество дисциплин</p>
              </div>
          </div> : 
          <div className={styles.table}>
            <div className={`${styles.table__line} ${styles.table__line_squads}`}> 
              <h5 className={styles.table__title}>Предмет</h5>
              <h5 className={styles.table__title}>По плану</h5>
              <h5 className={styles.table__title}>Выполнено</h5>
              <h5 className={styles.table__title}>Отсутствуют</h5>
            </div>
            <div className={styles.table__line}>
              <h5 className={styles.table__title}>А-323</h5>
            </div>
            <div className={`${styles.table__line}  ${styles.table__line_squads}`}>
                <p className={styles.table__item}>ВТП</p>
                <p className={styles.table__item}>100</p>
                <p className={styles.table__item}>80</p>
                <div className={`${styles.table__item} ${styles.table__subjects}`}>
                  <div className={styles.table__subject}>
                    <p>Т1/2 - 2 лек.</p>
                    <Button onClick={()=>{}}>Создать</Button>
                  </div>
                  <div className={styles.table__subject}>
                    <p>Т1/2 - 2 лек.</p>
                    <Button onClick={()=>{}}>Создать</Button>
                  </div>
                  <div className={styles.table__subject}>
                    <p>Т1/2 - 2 лек.</p>
                    <Button onClick={()=>{}}>Создать</Button>
                  </div>                                    
                </div>
            </div>
          </div>
        }
      </div>
    </PopupContainer>
  )
}