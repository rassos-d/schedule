import { Helmet } from 'react-helmet-async'
import styles from './schedule.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { Button } from '../../components/button/button'
import { ChangeLessonReponse, Schedule } from '../../types/schedule'
import { useEffect, useState } from 'react'
import { getFullSchedule, getSchedule, sortedDates } from '../../utils/schedule'
import { Icon } from '../../components/icon'
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'
import { DragLesson } from '../../components/dragNDrop/dragLesson'
import { DropLesson } from '../../components/dragNDrop/dropLesson'
import { DropZone } from '../../components/dragNDrop/dropZone'
import { DragFreeLesson } from '../../components/dragNDrop/dragFreeLesson'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { AddInput, Input } from '../../components/input/Input'
import { Tabs } from '../../components/tabs/tabs'
import { COURSES_YEAR } from '../../consts'
import { useNavigate, useParams } from 'react-router-dom'
import { Teacher } from '../../types/teacher'
import { Audience } from '../../types/audience'
import { Direction } from '../../types/directions'
import { FreeLesson, NewLesson, SheduleLesson } from '../../types/lesson'
import { Squad } from '../../types/squad'
import { getWeekDayAndDate } from '../../utils/date'
import { Theme, toast } from 'react-toastify'
import { Subject } from '../../types/subject'
import { AddInputList } from '../../types/input'

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


export default function ShedulePage() {

    const navigate = useNavigate()

    const {id} = useParams()

    const [allTabs, setAllTabs] = useState<number[]>()
    const [activeTab, setActiveTab] = useState<number>()

    const [schedule, setShedule] = useState<Schedule>()

    const [subjects, setSubjects] = useState<Subject[]>()

    const [allDirections, setAllDirections] = useState<Direction[]>()

    const [allTeachers, setAllTeachers] = useState<Teacher[]>()
    const [allAudience, setAllAudience] = useState<Audience[]>()
    const [allSquads, setAllSquads] = useState<Squad[]>()
    const [allThemes, setAllThemes] = useState<Theme[]>()

    const [isSidebarOpen, setIsSidebarOpen] = useState(false)
    const [activeSquardIndex, setActiveSquardIndex] = useState(0)
    const [freeLessons, setFreeLessons] = useState<FreeLesson[]>()

    const [newLesson, setNewLesson] = useState<NewLesson>()

    const handleGetAllTeachers = async () => {
        const {data } = await axios.get<Teacher[]>(PagesURl.TEACHER)
        //console.log(data)
        setAllTeachers(data)
    }
    const handleGetAllAudience = async () => {
        const {data} = await axios.get<Audience[]>(PagesURl.AUDIENCE)
        //console.log(data)
        setAllAudience(data)
    }
    const handleGetAllDirections = async () => {
        const {data} = await axios.get<Direction[]>(PagesURl.DIRECTION + '/find')
        //console.log(data)
        setAllDirections(data)
    }
    const handleGetAllSquads = async () => {
        const {data} = await axios.get<Squad[]>(PagesURl.SQUAD)
        setAllSquads(data)
    }
    const handleGetAllThemes = async () => {
        const {data} = await axios.get<Theme[]>(PagesURl.THEME + '/find')
        setAllThemes(data)
    }

    const handleGetSubjects = async (directionId: string) => {
        const {data} = await axios.get<Subject[]>(PagesURl.SUBJECT + '/find', {
            params: {
                directionId
            }
        })
        console.log(data)
        setSubjects(data)
    }

    const handleGetStudyYears = () => {
        setAllTabs([1])
        setActiveTab(1)
    }

    const handleGetSchedule = async (studyYear: number) => {
        const {data} = await axios.get<Schedule>(PagesURl.SCHEDULE + `/${id}/pages/${studyYear}`)
        //console.log(data, 'schedule')
        const freeLessons:FreeLesson[] = []
        for (const lesson of data.noName) {
            freeLessons.push(({...lesson, squardIndex: data.squads.findIndex((squad)=>squad.name === lesson.squadName)}))
        }
        setFreeLessons(freeLessons)
        setShedule(getFullSchedule(data))
    }
    const handleUpdateScheduleTime = async (lesson: Partial<SheduleLesson>) => {
        const {data} = await axios.put<ChangeLessonReponse>(PagesURl.EVENT + `/${lesson.id}/schedules/${id}/${1}`, {
            number: lesson.number,
            date: lesson.date
        })
        if (data.message) {
            toast(data.message)
        }
    }


    const onMoveLessonFromTableToTable = (squardIndex: number, target: {date: string;number: number;lesson?: SheduleLesson}, oldDate: string, oldNumber: number, oldLesson:SheduleLesson) => {
        if (oldDate === target.date && oldNumber === target.number) return
        setShedule((prev) => {
            const newShedule:Schedule = JSON.parse(JSON.stringify(prev))
            if (target.lesson !== undefined) {
                const oldLesson = newShedule.squads[squardIndex].events[oldDate][oldNumber - 1]
                newShedule.squads[squardIndex].events[target.date][target.number - 1] = {...oldLesson, number: target.lesson.number, date: target.lesson.date}
                newShedule.squads[squardIndex].events[oldDate][oldNumber - 1] = {...target.lesson, number: oldLesson.number}
                handleUpdateScheduleTime({...oldLesson, number: target.lesson.number, date: target.lesson.date})
                handleUpdateScheduleTime({...target.lesson, number: oldLesson.number, date: oldDate})
                return newShedule
            }
            newShedule.squads[squardIndex].events[target.date][target.number - 1] = {...oldLesson, number: target.number, date: target.date}
            newShedule.squads[squardIndex].events[oldDate][oldNumber - 1] = {number: oldNumber}
            handleUpdateScheduleTime({...oldLesson, number: target.number, date: target.date})
            return newShedule
        })
    }
    const onMoveLessonToFree = (activeSquardIndex: number, date: string, lesson: SheduleLesson) => {
        if (activeSquardIndex === -1) return
        setFreeLessons((prev)=>{
            if (!prev) return
            const newLessons = [...prev]
            newLessons.push({...lesson, squardIndex: activeSquardIndex})
            return newLessons
        })
        setShedule((prev)=>{
            const newShedule:Schedule = JSON.parse(JSON.stringify(prev))
            newShedule.squads[activeSquardIndex].events[date][lesson.number - 1] = {number: lesson.number}
            handleUpdateScheduleTime({...lesson, number: undefined, date: undefined})
            return newShedule
        })
    }
    const onMoveFreeToLesson = (target: {date: string, number: number}, lesson: FreeLesson) => {
        if (!target.date) return
        setShedule((prev)=>{
            const newShedule:Schedule = JSON.parse(JSON.stringify(prev))
            newShedule.squads[activeSquardIndex].events[target.date][target.number - 1] = {...lesson, number: target.number, date: target.date}
            handleUpdateScheduleTime({...lesson, number: target.number, date: target.date})
            return newShedule
        })
        setFreeLessons((prev)=>{
            if (!prev) return
            let newLessons = [...prev]
            newLessons = newLessons.filter((item)=>item.id !== lesson.id)
            return newLessons
        })
    }

    const startDragging = (squardIndex: number) => {
        setActiveSquardIndex(squardIndex)
        setIsSidebarOpen(true)
    }

    const createLesson = (day: string, number: number, squardIndex: number) => {
        if (!allDirections || !schedule) return
        const direction = allDirections.find((direction)=>schedule.squads[squardIndex].directionName === direction.name)
        console.log(direction)
        handleGetSubjects(direction ? direction.id : '')
        setNewLesson({
            date: day, 
            number, squardIndex, 
            teacher: {name: schedule.squads[squardIndex].teacherName || '', id: schedule.squads[squardIndex].teacherName || ''},
            audience: {name: schedule.squads[squardIndex].audienceName || '', id: schedule.squads[squardIndex].audienceName || ''}
        })
    }
    const getLessonsByTheme = () => {
        if (!subjects || !newLesson || !newLesson.subject) return [] as AddInputList[]
        const selectedSubject = subjects.filter((subject)=>subject.id === newLesson.subject?.id)
        if (!selectedSubject) return [] as AddInputList[]
        return selectedSubject[0].themes.filter((theme)=>theme.id === newLesson.theme?.id)[0].lessons
    }

    useEffect(()=>{
        handleGetStudyYears()
    }, [])

    useEffect(()=>{
        if (activeTab) {
            handleGetAllSquads()
            handleGetAllTeachers()
            handleGetAllAudience()
            handleGetAllDirections()
            handleGetAllThemes()
            handleGetSchedule(activeTab)
        }
    },[activeTab])

    if (!schedule || !freeLessons || !allTabs || !activeTab) return <></>

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
                    {schedule.squads.map((item, squardIndex) => (
                        <div key={item.id} className={styles.container__tableContainer}>
                            <div className={styles.container__table}>
                                <div className={styles.table__firstLine}>
                                    <p className={styles.table__header}>Учебный взвод</p>
                                    <p className={styles.table__header}>Учебный час, время</p>
                                    {sortedDates(item.events).map(([dayKey]) => (
                                        <p className={styles.table__date} key={dayKey}>{getWeekDayAndDate(dayKey)}</p>
                                    ))}
                                </div>
                                <div className={`${styles.table__content}`}>
                                    <div className={styles.table__content_first}>
                                        <h3>{item.name}</h3>
                                        <p>{item.directionName}</p>
                                        <p>{item.teacherName ? `Ответственный преподаватель ${item.teacherName}` : ''}</p>
                                    </div>
                                    <div className={styles.table__time}>
                                        {TIMES.map((el, index) => (
                                            <div key={el.time} className={`${styles.table__time_row} ${index === 2 && styles.table__time_row_grey}`}>
                                                <p className={`${styles.table__time_number} ${index === 2 && styles.table__time_number_bold}`}>{el.number}</p>
                                                <p className={styles.table__time_time}>{el.time}</p>
                                            </div>
                                        ))}
                                    </div>
                                    {sortedDates(getSchedule(item.events)).map(([dayKey, dayLessons], index) => (
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
                                        {schedule.squads.map((squard, index)=>(
                                            <p key={squard.id} className={`${styles.sidebar__tab} ${index === activeSquardIndex && styles.sidebar__tab_active}`} onClick={(e) => {e.stopPropagation();setActiveSquardIndex(index)}}>{squard.name}</p>
                                        ))}
                                    </div>
                                    <div style={{ cursor: 'pointer' }} className={styles.sidebar_close} onClick={() => {setIsSidebarOpen(false)}}>
                                        <Icon glyph='arrow-down' glyphColor='black' />
                                    </div>
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
            {newLesson && subjects &&
                <PopupContainer onClose={()=>setNewLesson(undefined)}>
                    <div className={styles.popup}>
                        <h2>Создание занятия</h2>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Взвод:</p>
                            <p className={styles.popup__text}>{schedule.squads[newLesson.squardIndex].name}</p>
                        </div>
                        <div className={styles.popup__line}>
                            <p className={styles.popup__title}>Дисциплина:</p>
                            <AddInput
                                selectedList={newLesson.subject ? [newLesson.subject] : []}
                                singleMode
                                allList={subjects}
                                title='Выберите дисциплину'
                                changeInputList={(newList)=>setNewLesson({...newLesson, subject: {...newList[0]}})}
                            />
                        </div>
                        {newLesson.subject &&   
                            <div className={styles.popup__line}>
                                <p className={styles.popup__title}>Тема:</p>
                                <AddInput
                                    selectedList={newLesson.theme ? [newLesson.theme] : []}
                                    singleMode
                                    allList={subjects.filter((subject)=>subject.id === newLesson.subject?.id)[0].themes}
                                    title='Выберите тему'
                                    changeInputList={(newList)=>setNewLesson({...newLesson, theme: {...newList[0]}})}
                                />
                            </div>
                        }
                        {newLesson.theme &&   
                            <div className={styles.popup__line}>
                                <p className={styles.popup__title}>Занятие:</p>
                                <AddInput
                                    selectedList={newLesson.lesson ? [newLesson.lesson] : []}
                                    singleMode
                                    allList={getLessonsByTheme()}
                                    title='Выберите занятие'
                                    changeInputList={(newList)=>setNewLesson({...newLesson, subject: {...newList[0]}})}
                                />
                            </div>
                        }
                            <div className={styles.popup__line}>
                                <p className={styles.popup__title}>Занятие:</p>
                                <AddInput
                                    selectedList={newLesson.lesson ? [newLesson.lesson] : []}
                                    singleMode
                                    allList={getLessonsByTheme()}
                                    title='Выберите занятие'
                                    changeInputList={(newList)=>setNewLesson({...newLesson, subject: {...newList[0]}})}
                                />
                            </div>
                       {/*  <div className={styles.popup__block}>
                            <p className={styles.popup__title}>Преподаватель:</p>
                            <div className={styles.popup__text}>
                                <Input value={newLesson.name ?? ''} onChange={(value)=>setNewLesson({...newLesson, name: value})}/>
                            </div>
                        </div> */}
                       {/*  <div className={styles.popup__line}>
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
                        </div> */}
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