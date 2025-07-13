import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { CreateSchedule, CreateScheduleYear, ScheduleSquad, SmallShedule } from '../../types/shedule'
import { Icon } from '../../components/icon'
import { Button } from '../../components/button/button'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { AddInput, Input } from '../../components/input/Input'
import { useNavigate } from 'react-router-dom'
import { COURSES_YEAR } from '../../consts/tabs'
import { cloneObject, getUniqueElements } from '../../utils'
import { Squad } from '../../types/squad'


export default function Main () {

    const [shedules, setShedules] = useState<SmallShedule[]>()
    const [newSchedule, setNewSchedule] = useState<CreateSchedule>()
    const [freeCoursesYear, setFreeCoursesYear] = useState(COURSES_YEAR)
    const [squads, setSquads] = useState<Squad[]>()
    const navigate = useNavigate()


    const handleGetShedules = async () => {
        const {data} = await axios.get<SmallShedule[]>(PagesURl.SHEDULE + '/find')
        setShedules(data)
    }
    const handleDeleteSchedule = async (id: string) => {
        await axios.delete(PagesURl.SHEDULE + `/${id}`)
        handleGetShedules()
    }
    const handleCreateShedule = async (schedule: CreateSchedule) => {
        const transformedSchedule = {
            ...schedule,
            pages: schedule.pages.map(page => ({
                ...page,
                squads: page.squads.map(squad => squad.id)
            }))
        };
        const {data} = await axios.post<{data: string}>(PagesURl.SHEDULE, transformedSchedule)
        navigate(`/${data.data}`)
    }
    const handleGetAllSquads = async () => {
        const {data} = await axios.get<Squad[]>(PagesURl.SQUAD)
        setSquads(data)
    }

    const getFreeYears = (scheduleYears: CreateScheduleYear[]) => {
        const newFreeYears = [...COURSES_YEAR, ...scheduleYears.map((year)=>year.studyYear)]
        const unique = getUniqueElements(newFreeYears)
        return unique
    }

    const addNewYear = () => {
        if (!newSchedule) return
        setNewSchedule((prev)=>{
            if (!prev) return 
            const freeYears = getFreeYears(prev.pages)
            return {...prev, pages: [...prev.pages, {
                studyYear: freeYears[0], 
                squads: [], 
                start: new Date().toISOString(), 
                end: new Date().toISOString()
            }
        ]}})
    }
    const addNewYearToYear = (year: number, index: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return undefined
            const result = cloneObject(prev)
            if (result.pages[index].studyYear !== year) {
                result.pages[index] = {...result.pages[index], squads: []}
            }
            result.pages[index].studyYear = year
            return result
        })
    }

    const updateSquards = (newList: ScheduleSquad[], yearIndex: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.pages[yearIndex].squads = newList
            return newSchedule
        })
    }
    const updateDateYear = (newDate: string, isStart: boolean, yearIndex: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.pages[yearIndex][isStart ? 'start' : 'end'] = newDate
            return newSchedule
        })
    }

    useEffect(()=>{
        if (newSchedule) {
            setFreeCoursesYear(getFreeYears(newSchedule.pages))
        }
    },[newSchedule])

    useEffect(()=>{
        handleGetAllSquads()
        handleGetShedules()
    },[])

    if (!shedules) {
        return <></>
    }

    return (
        <>
            <Helmet>
                <title>Главная</title>
            </Helmet>
            <div className={styles.container}>
                <h1 className={styles.container__title}>Расписание кафедры СП</h1>
                <h3 className={styles.container__subtitle}>Сохранённые расписания</h3>
                <div className={styles.container__shedules}>
                    {shedules.map((shedule)=>(
                        <div onClick={()=>navigate(`/${shedule.id}`)} className={styles.container__shedule} key={shedule.id}>
                            <p>{shedule.name}</p>
                            <div onClick={(e)=>{e.stopPropagation();handleDeleteSchedule(shedule.id)}}><Icon glyph='close' glyphColor='black'/></div>
                        </div>
                    ))}
                </div>
                <div onClick={()=>setNewSchedule({name: '', pages: []})} className={styles.container__button}><Button>Создать новое</Button></div>
            </div>
            {newSchedule !== undefined && 
                <PopupContainer onClose={()=>setNewSchedule(undefined)}>
                    <div className={styles.popup}>
                        <h2>Создание расписания</h2>
                        <div onClick={()=>setNewSchedule(undefined)} className={styles.popup__close}><Icon glyph='close' glyphColor='black'/></div>
                        <div style={{width: '95%'}}><Input value={newSchedule.name} onChange={(value)=>setNewSchedule({...newSchedule, name: value})} placeholder='Введите название'/></div>
                        {newSchedule.pages.map((year, index)=>(
                            <div className={styles.popup__addList} key={year.studyYear}>
                                <AddInput 
                                    title='Год обучения' 
                                    singleMode 
                                    selectedList={[{name: year.studyYear, id: year.studyYear}]} 
                                    allList={freeCoursesYear.map((year)=>({name: year, id: year}))} 
                                    changeInputList={(newList)=>addNewYearToYear(Number(newList[0].id), index)}
                                />
                                {squads &&
                                <AddInput
                                    title='Взвода'
                                    selectedList={year.squads}
                                    allList={squads.filter((squad)=>squad.studyYear===year.studyYear)}
                                    changeInputList={(newList)=>updateSquards(newList.map((item)=>({name: item.name.toString(), id: item.id.toString()})), index)}
                                />}
                                <Input value={year.start} type='date' onChange={(val)=>updateDateYear(val, true, index)}/>
                                <Input value={year.end} type='date' onChange={(val)=>updateDateYear(val, false, index)}/>
                            </div>
                        ))}
                        {COURSES_YEAR.length > newSchedule.pages.length && <Button onClick={addNewYear}>Добавить год обучения</Button>}
                        <Button onClick={()=>handleCreateShedule(newSchedule)}>Создать расписание</Button>
                    </div>
                </PopupContainer>
            }
        </>
    )
}