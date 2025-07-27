import { useEffect, useState } from 'react'
import PopupContainer from '../popupContainer/popupContainer'
import styles from './statistic.module.scss'
import { Button } from '../button/button'
import axios, { PagesURl } from '../../services/api/api'
import { MissingLesson, StatisticSquad } from '../../types/squad'
import { StatisticTeacher } from '../../types/teacher'

type StatisticProps = {
  onClose: () => void
  updateStash: ()=>void
  scheduleId: string
  dayOfWeek: number
}

const TABS = ['Преподаватели', 'Взвода']

export function Statistic ({scheduleId, dayOfWeek, onClose, updateStash}:StatisticProps) {

  const [selectedTab, setSelectedTab] = useState<string>(TABS[0])
  const [squads, setSquads] = useState<StatisticSquad[]>()
  const [teachers, setTeachers] = useState<StatisticTeacher[]>()

  const handleGetSquads = async () => {
    const {data} = await axios.get<StatisticSquad[]>(PagesURl.SCHEDULE + `/${scheduleId}/${dayOfWeek}/statistics/squads`)
    setSquads(data)
  }
  const handleGetTeachers = async () => {
    const {data} = await axios.get<StatisticTeacher[]>(PagesURl.SCHEDULE + `/${scheduleId}/${dayOfWeek}/statistics/teachers`)
    setTeachers(data)
  }
  const handleCreateLesson = async (lesson:MissingLesson, squad: StatisticSquad, subjectId: string ) => {
    await axios.post(PagesURl.EVENT + `/schedules/${scheduleId}/${dayOfWeek}`, {
      subjectId,
      themeId: lesson.themeId,
      lessonId: lesson.lessonId,
      squadId: squad.id,
      teacherId: squad.teacherId,
      audienceId: squad.fixedAudienceId
    })
    handleGetSquads()
    updateStash()
    
  }

  useEffect(()=>{
    handleGetTeachers()
    handleGetSquads()
  },[])

  if (!teachers || !squads) return <PopupContainer onClose={onClose} displayClose><div className={styles.container}/></PopupContainer>

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
            {teachers.map((teacher) => (
              <div key={teacher.id} className={`${styles.table__line} ${styles.table__line_teacher}`}>
                <p className={styles.table__item}>{`${teacher.rank} ${teacher.name}`}</p>
                <p className={styles.table__item}>{teacher.hoursCount}</p>
                <p className={styles.table__item}>{teacher.subjectsCount}</p>
              </div>
            ))}
          </div> : 
          <div className={styles.table}>
            <div className={`${styles.table__line} ${styles.table__line_squads}`}> 
              <h5 className={styles.table__title}>Предмет</h5>
              <h5 className={styles.table__title}>По плану (ч)</h5>
              <h5 className={styles.table__title}>Выполнено (ч)</h5>
              <h5 className={styles.table__title}>Отсутствуют</h5>
            </div>
            {squads.map((squad, index) => (
              <div className={`${index !==0 ? styles.table__squad : ''} ${index%2 === 0 ? styles.table__squad_white : styles.table__squad_grey}`} key={squad.id}>
                <div className={styles.table__line}>
                  <h5 className={styles.table__title}>{squad.name}</h5>
                </div>
                {squad.subjects.map((subject) => (
                  <div key={subject.id} className={`${styles.table__line}  ${styles.table__line_squads}`}>
                    <p className={styles.table__item}>{subject.name}</p>
                    <p className={styles.table__item}>{subject.plannedHours}</p>
                    <p className={styles.table__item}>{subject.completedHours}</p>
                    <div className={`${styles.table__item} ${styles.table__subjects}`}>
                      {subject.missingLessons.map((lesson) => (
                        <div key={lesson.lessonId} className={styles.table__subject}>
                          <p>{`Т${lesson.themeNumber}/${lesson.lessonNumber}`}</p>
                          <Button onClick={() => {handleCreateLesson(lesson, squad, subject.id)}}>Создать</Button>
                        </div>
                      ))}
                      {subject.missingLessons.length === 0 && <div className={styles.table__subject_empty}>нет</div>}
                    </div>
                  </div>
                ))}
              </div>
            ))}
          </div>
        }
      </div>
    </PopupContainer>
  )
}