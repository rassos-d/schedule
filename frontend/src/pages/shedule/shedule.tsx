import { Helmet } from 'react-helmet-async'
import styles from './shedule.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { Button } from '../../components/button/button'
import { Shedule } from '../../types/shedule'
import { useEffect, useState } from 'react'
import { getFullShedule, getShedule } from '../../utils/shedule'
import { Icon } from '../../components/icon'
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'
import { DragLesson } from '../../components/dragNDrop/dragLesson'
import { DropLesson } from '../../components/dragNDrop/dropLesson'
import { DropZone } from '../../components/dragNDrop/dropZone'
import { DragFreeLesson } from '../../components/dragNDrop/dragFreeLesson'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { Input } from '../../components/input/Input'
import { Tabs } from '../../components/tabs/tabs'
import { COURSES_YEAR } from '../../consts/tabs'
import { useNavigate, useParams } from 'react-router-dom'
import { Teacher } from '../../types/teacher'
import { Audience } from '../../types/audience'
import { Direction } from '../../types/directions'
import { FreeLesson, Lesson, NewLesson } from '../../types/lesson'
import { Squad } from '../../types/squad'

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
    squads: [
        {
            id: '2',
            name: 'А-323',
            events: {
                "05.09": [{ id: '1', name: "т1 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "12.09": [{ id: '12', name: "т2 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "19.09": [{ id: '13', name: "т3 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "26.09": [{ id: '14', name: "т4 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "03.10": [{ id: '15', name: "т5 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "10.10": [{ id: '16', name: "т6 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "17.10": [{ id: '17', name: "т7 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
                "24.10": [{ id: '18', name: "т8 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "31.10": [{ id: '19', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "07.10": [{ id: '20', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "14.11": [{ id: '21', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "21.11": [{ id: '22', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "28.11": [{ id: '23', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "05.12": [{ id: '24', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "12.12": [{ id: '25', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "19.12": [{ id: '26', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "26.12": [{ id: '27', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
            }
        },
        {
            id: '3',
            name: 'Д-323',
            events: {
                "05.09": [{ id: '21', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "12.09": [{ id: '22', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "19.09": [{ id: '23', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "26.09": [{ id: '24', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "03.10": [{ id: '25', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "10.10": [{ id: '26', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 4 }],
                "17.10": [{ id: '27', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
                "24.10": [{ id: '28', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "31.10": [{ id: '29', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "07.10": [{ id: '30', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "14.11": [{ id: '31', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 3 }],
                "21.11": [{ id: '32', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "28.11": [{ id: '33', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "05.12": [{ id: '34', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "12.12": [{ id: '35', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 1 }],
                "19.12": [{ id: '36', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 2 }],
                "26.12": [{ id: '37', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', number: 5 }],
            }
        }
    ]
}

const FREE_LESSONS = [
    { id: '51', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 0 },
    { id: '54', name: "т 8/2 лек", teacherId: '1', teacher_name: 'п-к Кизюн Н.Н.', audience_name: 'ВО-404', squardIndex: 1 },
]

export default function ShedulePage() {

    const navigate = useNavigate()

    const {id} = useParams()

    const [activeTab, setActiveTab] = useState(1)

    const [shedule, setShedule] = useState(getFullShedule(SHEDULE))
    const [allTeachers, setAllTeachers] = useState<Teacher[]>()
    const [allAudience, setAllAudience] = useState<Audience[]>()
    const [allDirections, setAllDirections] = useState<Direction[]>()
    const [allSquads, setAllSquads] = useState<Squad[]>()
 
    const [isSidebarOpen, setIsSidebarOpen] = useState(false)
    const [activeSquardIndex, setActiveSquardIndex] = useState(0)
    const [freeLessons, setFreeLessons] = useState<FreeLesson[]>(FREE_LESSONS)

    const [newLesson, setNewLesson] = useState<NewLesson>()

    const handleGetAllTeachers = async () => {
        const {data, config} = await axios.get<Teacher[]>(PagesURl.TEACHER)
        console.log(data)
        setAllTeachers(data)
    }
    const handleGetAllAudience = async () => {
        const {data} = await axios.get<Audience[]>(PagesURl.AUDIENCE)
        console.log(data)
        setAllAudience(data)
    }
    const handleGetAllDirections = async () => {
        const {data} = await axios.get<Direction[]>(PagesURl.DIRECTION + '/find')
        console.log(data)
        setAllDirections(data)
    }
    const handleGetAllSquads = async () => {
        const {data} = await axios.get<Squad[]>(PagesURl.SQUAD)
        setAllSquads(data)
    }

    const handleGetStudyYears = () => {
        return [3]
    }

    const handleGetSchedule = async (studyYear: number) => {
        const {data} = await axios.get<Shedule>(PagesURl.SHEDULE + `/${id}/pages/${studyYear}`)
        console.log(data, 'cshedule')
        //setShedule(data)
    }


    const onMoveLessonFromTableToTable = (squardIndex: number, target: {date: string;number: number;lesson?: Lesson}, oldDate: string, oldNumber: number, oldLesson:Lesson) => {
        if (oldDate === target.date && oldNumber === target.number) return
        setShedule((prev) => {
            const newShedule:Shedule = JSON.parse(JSON.stringify(prev))
            if (target.lesson !== undefined) {
                const oldLesson = newShedule.squads[squardIndex].events[oldDate][oldNumber - 1]
                newShedule.squads[squardIndex].events[target.date][target.number - 1] = {...oldLesson, number: target.lesson.number}
                newShedule.squads[squardIndex].events[oldDate][oldNumber - 1] = {...target.lesson, number: oldLesson.number}
                return newShedule
            }
            newShedule.squads[squardIndex].events[target.date][target.number - 1] = {...oldLesson, number: target.number}
            newShedule.squads[squardIndex].events[oldDate][oldNumber - 1] = {number: oldNumber}
            return newShedule
        })
    }
    const onMoveLessonToFree = (activeSquardIndex: number, date: string, lesson: Lesson) => {
        if (activeSquardIndex === -1) return
        setFreeLessons((prev)=>{
            const newLessons = [...prev]
            newLessons.push({...lesson, squardIndex: activeSquardIndex})
            return newLessons
        })
        setShedule((prev)=>{
            const newShedule:Shedule = JSON.parse(JSON.stringify(prev))
            newShedule.squads[activeSquardIndex].events[date][lesson.number - 1] = {number: lesson.number}
            return newShedule
        })
    }
    const onMoveFreeToLesson = (target: {date: string, number: number}, lesson: FreeLesson) => {
        setShedule((prev)=>{
            const newShedule:Shedule = JSON.parse(JSON.stringify(prev))
            newShedule.squads[activeSquardIndex].events[target.date][target.number - 1] = {...lesson, number: target.number}
            return newShedule
        })
        setFreeLessons((prev)=>{
            let newLessons = [...prev]
            newLessons = newLessons.filter((item)=>item.id !== lesson.id)
            return newLessons
        })
    }

    const startDragging = (squardIndex: number) => {
        setActiveSquardIndex(squardIndex)
    }

    const createLesson = (day: string, number: number, squardIndex: number) => {
        setNewLesson({date: day, number, squardIndex})
    }

    useEffect(()=>{
        handleGetAllSquads()
        handleGetAllTeachers()
        handleGetAllAudience()
        handleGetAllDirections()
        handleGetSchedule(handleGetStudyYears()[0])
    },[])

    return (
        <>
            <Helmet>
                <title>{`Расписание ${activeTab} курс`}</title>
            </Helmet>
            <div className={styles.container}>
                <div className={styles.container__tabs}>
                    <Tabs onClick={setActiveTab} tabs={COURSES_YEAR} activeTab={activeTab}/>
                </div>
                <Button onClick={()=>navigate('/')} className={styles.container__back}>На главную</Button>
                <h1 className={styles.container__title}>{`Расписание ${activeTab} курс`}</h1>
                <DndProvider backend={HTML5Backend}>
                    {shedule.squads.map((item, squardIndex) => (
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
                                                    {"id" in lesson ?
                                                        <DragLesson
                                                            squardIndex={squardIndex} 
                                                            lesson={lesson} 
                                                            date={dayKey}
                                                            number={lesson.number}
                                                            onStartDragging={()=>startDragging(squardIndex)}
                                                            onMove={(target, oldDate, oldNumber) => {
                                                                 "activeSquardIndex" in target ? onMoveLessonToFree(target.activeSquardIndex, dayKey, lesson) :
                                                                onMoveLessonFromTableToTable(squardIndex, target, oldDate, oldNumber,  lesson)
                                                            }} 
                                                        /> :
                                                        <DropLesson 
                                                            squardIndex={squardIndex} 
                                                            date={dayKey} 
                                                            number={lesson.number}
                                                            onCreateLesson={createLesson}
                                                        />
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
                                        {shedule.squads.map((squard, index)=>(
                                            <p key={squard.id} className={`${styles.sidebar__tab} ${index === activeSquardIndex && styles.sidebar__tab_active}`} onClick={() => setActiveSquardIndex(index)}>{squard.name}</p>
                                        ))}
                                    </div>
                                    <img style={{ cursor: 'pointer' }} onClick={() => setIsSidebarOpen(false)} src="/icons/close.svg" />
                                </div>
                                <DropZone activeSquardIndex={activeSquardIndex}>
                                    <div className={styles.sidebar__items}>
                                        {freeLessons.filter((lesson)=>lesson.squardIndex === activeSquardIndex).map((lesson)=>(
                                            <DragFreeLesson key={lesson.id} lesson={lesson} squardIndex={activeSquardIndex} onMove={onMoveFreeToLesson}/>
                                        ))}
                                    </div>
                                </DropZone>
                            </div>
                        }
                    </div>
                </DndProvider>
            </div>
            {newLesson && 
                <PopupContainer onClose={()=>setNewLesson(undefined)}>
                    <div className={styles.popup}>
                        <h2>Создание занятия</h2>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Взвод:</p>
                            <p className={styles.popup__text}>{shedule.squads[newLesson.squardIndex].name}</p>
                        </div>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Тема:</p>
                            <div className={styles.popup__text}>
                                <Input value={newLesson.name ?? ''} onChange={(value)=>setNewLesson({...newLesson, name: value})}/>
                            </div>
                        </div>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Занятие:</p>
                            <div className={styles.popup__text}>
                                <Input value={newLesson.name ?? ''} onChange={(value)=>setNewLesson({...newLesson, name: value})}/>
                            </div>
                        </div>
                        <div className={styles.popup__block}>
                            <p className={styles.popup__title}>Преподаватель:</p>
                            <div className={styles.popup__text}>
                                <Input value={newLesson.name ?? ''} onChange={(value)=>setNewLesson({...newLesson, name: value})}/>
                            </div>
                        </div>
                        <div className={styles.popup__block}>
                            <p className={styles.popup__title}>Аудитория:</p>
                            <div className={styles.popup__text}>
                                <Input value={newLesson.name ?? ''} onChange={(value)=>setNewLesson({...newLesson, name: value})}/>
                            </div>
                        </div>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Дата:</p>
                            <div className={styles.popup__text}>
                                <Input type='date' value={newLesson.date ?? ''} onChange={(value)=>setNewLesson({...newLesson, date: value})}/>
                            </div>
                        </div>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Дата:</p>
                            <p className={styles.popup__text}>{TIMES[newLesson.number - 1].time}</p>
                        </div>
                        <Button size={'max'}>Сохранить</Button>
                    </div>
                </PopupContainer>
            }
        </>
    )
}