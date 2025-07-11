import { Helmet } from 'react-helmet-async'
import styles from './shedule.module.scss'
import { Button } from '../../components/button/button'
import { FreeLesson, Lesson, Shedule } from '../../types/shedule'
import { useState } from 'react'
import { getFullShedule, getShedule } from '../../utils/shedule'
import { Icon } from '../../components/icon'
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'
import { DragLesson } from '../../components/dragNDrop/dragLesson'
import { DropLesson } from '../../components/dragNDrop/dropLesson'
import { DropZone } from '../../components/dragNDrop/dropZone'

const TIMES = [
    {
        number: '1 - 2',
        time: '08.30 - 10.00'
    },
    {
        number: '3 - 4',
        time: '10.15 - 11.45'
    },
    {
        number: 'ТРЕН',
        time: '12.00 - 12.40'
    },
    {
        number: '5 - 6',
        time: '13.30 - 15.00'
    },
    {
        number: '7 - 8',
        time: '15.15 - 16.45'
    },
]

const SHEDULE: Shedule = {
    id: '1',
    name: 'name',
    squards: [
        {
            id: '2',
            name: 'А',
            events: {
                "05.09": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "12.09": [{ lesson_id: '12', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "19.09": [{ lesson_id: '13', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "26.09": [{ lesson_id: '14', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "03.10": [{ lesson_id: '15', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "10.10": [{ lesson_id: '16', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "17.10": [{ lesson_id: '17', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
                "24.10": [{ lesson_id: '18', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "31.10": [{ lesson_id: '19', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "07.10": [{ lesson_id: '20', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "14.11": [{ lesson_id: '21', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "21.11": [{ lesson_id: '22', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "28.11": [{ lesson_id: '23', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "05.12": [{ lesson_id: '24', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "12.12": [{ lesson_id: '25', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "19.12": [{ lesson_id: '26', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "26.12": [{ lesson_id: '27', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
            }
        },
        {
            id: '3',
            name: 'Д',
            events: {
                "05.09": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "12.09": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "19.09": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "26.09": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "03.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "10.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "17.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
                "24.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "31.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "07.10": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "14.11": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "21.11": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "28.11": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "05.12": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "12.12": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "19.12": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "26.12": [{ lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
            }
        }
    ]
}

const FREE_LESSONS = [
    { lesson_id: '51', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 0 },
    { lesson_id: '52', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 0 },
    { lesson_id: '53', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 0 },
    { lesson_id: '54', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 1 },
]

export default function ShedulePage() {

    const [shedule, setShedule] = useState(getFullShedule(SHEDULE))
    const [isSidebarOpen, setIsSidebarOpen] = useState(false)
    const [activeSquardIndex, setActiveSquardIndex] = useState(1)
    const [freeLessons, setFreeLessons] = useState<FreeLesson[]>(FREE_LESSONS)


    const onMoveLessonFromTableToTable = (squardIndex: number, date: string, number: number, oldDate: string, oldNumber: number, lesson:Lesson) => {
        setShedule((prev) => {
            const newShedule:Shedule = JSON.parse(JSON.stringify(prev))
            newShedule.squards[squardIndex].events[date][number - 1] = {...lesson, number: number}
            newShedule.squards[squardIndex].events[oldDate][oldNumber - 1] = {number: oldNumber}
            return newShedule
        })
    }
    const onMoveLessonToFree = (activeSquardIndex: number, date: string, lesson: Lesson) => {
        console.log(activeSquardIndex)
        if (activeSquardIndex === -1) return
        setFreeLessons((prev)=>{
            const newLessons = [...prev]
            newLessons.push({...lesson, squardIndex: activeSquardIndex})
            return newLessons
        })
        setShedule((prev)=>{
            const newShedule:Shedule = JSON.parse(JSON.stringify(prev))
            newShedule.squards[activeSquardIndex].events[date][lesson.number - 1] = {number: lesson.number}
            return newShedule
        })
    }

    const startDragging = () => {
        setIsSidebarOpen(true)
    }

    return (
        <>
            <Helmet>
                <title>Расписание</title>
            </Helmet>
            <div className={styles.container}>
                <h1 className={styles.container__title}>Расписание</h1>
                <DndProvider backend={HTML5Backend}>
                    {shedule.squards.map((item, squardIndex) => (
                        <div key={item.id} className={styles.container__tableContainer}>
                            <div className={styles.container__table}>
                                <div className={styles.table__firstLine}>
                                    <p className={styles.table__header}>Учебный взвод</p>
                                    <p className={styles.table__header}>Учебный час, время</p>
                                    {Object.keys(item.events).map((date) => (
                                        <p className={styles.table__date} key={date}>{date}</p>
                                    ))}
                                </div>
                                <div className={`${styles.table__content}`}>
                                    <div className={styles.table__content_first}>
                                        <h3>А-323</h3>
                                        <p>ВУС 0941000</p>
                                        <p>Ответственный преподаватель подполковник Фролов И. В.</p>
                                    </div>
                                    <div className={styles.table__time}>
                                        {TIMES.map((el, index) => (
                                            <div key={el.time} className={`${styles.table__time_row} ${index === 2 && styles.table__time_row_grey}`}>
                                                <p className={`${styles.table__time_number} ${index === 2 && styles.table__time_number_bold}`}>{el.number}</p>
                                                <p className={styles.table__time_time}>{el.time}</p>
                                            </div>
                                        ))}
                                    </div>
                                    {Object.entries(getShedule(item.events)).map(([dayKey, dayLessons], index) => (
                                        <div key={`day-${index}`} className={styles.table__day}>
                                            {dayLessons.map((lesson) => (
                                                <div key={lesson.number} className={`${styles.table__lesson} ${lesson.number === 3 && styles.table__time_row_grey}`}>
                                                    {"lesson_id" in lesson ?
                                                        <DragLesson
                                                            squardIndex={squardIndex} 
                                                            lesson={lesson} 
                                                            date={dayKey}
                                                            number={lesson.number}
                                                            onStartDragging={startDragging}
                                                            onMove={(target, oldDate, oldNumber) => {
                                                                 "activeSquardIndex" in target ? onMoveLessonToFree(activeSquardIndex, dayKey, lesson) :
                                                                onMoveLessonFromTableToTable(squardIndex, target.date, target.number, oldDate, oldNumber,  lesson)
                                                            }} 
                                                        /> :
                                                        <DropLesson squardIndex={squardIndex} date={dayKey} number={lesson.number}/>
                                                    }
                                                </div>
                                            ))}
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    ))}
                    <div className={styles.container__buttons}>
                        <Button>ЭКСПОРТ</Button>
                        <Button>СОХРАНИТЬ</Button>
                    </div>
                    <div className={`${styles.container__sidebar} ${isSidebarOpen ? styles.container__sidebar_open : styles.container__sidebar_close}`}>
                        {!isSidebarOpen ?
                            <div onClick={() => setIsSidebarOpen(true)} className={styles.sidebar__close}>
                                <Icon glyph='arrow-up' glyphColor='black' />
                            </div> :
                            <div className={styles.sidebar__open}>
                                <div className={styles.sidebar__line}>
                                    <div className={styles.sidebar__tabs}>
                                        {shedule.squards.map((squard, index)=>(
                                            <p key={squard.id} className={`${styles.sidebar__tab} ${index === activeSquardIndex && styles.sidebar__tab_active}`} onClick={() => setActiveSquardIndex(index)}>{squard.name}</p>
                                        ))}
                                    </div>
                                    <img style={{ cursor: 'pointer' }} onClick={() => setIsSidebarOpen(false)} src="/icons/close.svg" />
                                </div>
                                <DropZone activeSquardIndex={activeSquardIndex}>
                                    <div className={styles.sidebar__items}>
                                        {freeLessons.filter((lesson)=>lesson.squardIndex === activeSquardIndex).map((lesson)=>(
                                            <div key={lesson.lesson_id} className={styles.sidebar__item}>
                                            <p>{activeSquardIndex}ТСП</p>
                                            <p>т 8/2 лек</p>
                                            <p>ВО-404</p>
                                            <p>п-к Кизюн Н.Н.</p>
                                        </div>
                                        ))}
                                    </div>
                                </DropZone>
                            </div>
                        }
                    </div>
                </DndProvider>
            </div>
        </>
    )
}