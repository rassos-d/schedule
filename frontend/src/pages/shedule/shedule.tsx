import { Helmet } from 'react-helmet-async'
import styles from './shedule.module.scss'
import { Button } from '../../components/button/button'
import { Shedule } from '../../types/shedule'
import { useState } from 'react'
import { getShedule } from '../../utils/shedule'
import { Icon } from '../../components/icon'

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

const SHEDULE:Shedule = {
    id: '1',
    name: 'name',
    squards: [
        {
            id: '2',
            name: 'squard',
            events: {
                "05.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "12.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "19.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "26.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4}],
                "03.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "10.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4}],
                "17.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5}],
                "24.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "31.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "07.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "14.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "21.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "28.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "05.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "12.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "19.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "26.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5}],
            }
        },
        {
            id: '2',
            name: 'squard',
            events: {
                "05.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "12.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "19.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "26.09": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4}],
                "03.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "10.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4}],
                "17.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5}],
                "24.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "31.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "07.10": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "14.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3}],
                "21.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "28.11": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "05.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "12.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1}],
                "19.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2}],
                "26.12": [{lesson_id: '1', lesson_name: "т 8/2 лек", teacher_id: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5}],
            }
        }
    ]
}

export default function ShedulePage () {

    //const [shedule, setShedule] = useState(SHEDULE)
    const [isSidebarOpen, setIsSidebarOpen] = useState(false)
    const [activeTab, setActiveTab] = useState('Д')

    return (
        <>
            <Helmet>
                <title>Расписание</title>
            </Helmet>
            <div className={styles.container}>
                <h1 className={styles.container__title}>Расписание</h1>
                {SHEDULE.squards.map((shedule)=>(
                    <div key={shedule.id} className={styles.container__tableContainer}>
                        <div className={styles.container__table}>
                            <div className={styles.table__firstLine}>
                                <p className={styles.table__header}>Учебный взвод</p>
                                <p className={styles.table__header}>Учебный час, время</p>
                                {Object.keys(shedule.events).map((date) => (
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
                                {Object.values(getShedule(shedule.events)).map((day, index) => (
                                    <div key={`day-${index}`} className={styles.table__day}>
                                        {day.map((lesson) => (
                                            <div key={lesson.number} className={`${styles.table__lesson} ${lesson.number === 3 && styles.table__time_row_grey}`}>
                                                {"lesson_id" in lesson ?
                                                    <>
                                                        <p>ТСП</p>
                                                        <p>т 8/2 лек</p>
                                                        <p>ВО-404</p>
                                                        <p>п-к Кизюн Н.Н.</p>
                                                    </> :
                                                    <></>
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
                        <div onClick={()=>setIsSidebarOpen(true)} className={styles.sidebar__close}>
                            <Icon glyph='arrow-up' glyphColor='black'/>
                        </div> : 
                        <div className={styles.sidebar__open}>
                            <div className={styles.sidebar__line}>
                                <div className={styles.sidebar__tabs}>
                                    <p className={`${styles.sidebar__tab} ${activeTab === "Д" && styles.sidebar__tab_active}`} onClick={()=>setActiveTab("Д")}>Д</p>
                                    <p className={`${styles.sidebar__tab} ${activeTab === "А" && styles.sidebar__tab_active}`} onClick={()=>setActiveTab("А")}>А</p>
                                    <p className={`${styles.sidebar__tab} ${activeTab === "Р" && styles.sidebar__tab_active}`} onClick={()=>setActiveTab("Р")}>Р</p>
                                </div>
                                <img style={{cursor: 'pointer'}} onClick={()=>setIsSidebarOpen(false)} src="/icons/close.svg"/>
                            </div>
                            <div className={styles.sidebar__items}>
                                <div className={styles.sidebar__item}>
                                    <p>ТСП</p>
                                    <p>т 8/2 лек</p>
                                    <p>ВО-404</p>
                                    <p>п-к Кизюн Н.Н.</p>
                                </div>
                                <div className={styles.sidebar__item}>
                                    <p>ТСП</p>
                                    <p>т 8/2 лек</p>
                                    <p>ВО-404</p>
                                    <p>п-к Кизюн Н.Н.</p>
                                </div>
                                <div className={styles.sidebar__item}>
                                    <p>ТСП</p>
                                    <p>т 8/2 лек</p>
                                    <p>ВО-404</p>
                                    <p>п-к Кизюн Н.Н.</p>
                                </div>
                            </div>
                        </div>
                    }
                </div>
            </div>
        </> 
    )
}