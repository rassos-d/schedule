import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { CreateSchedule, CreateScheduleYear, ScheduleSquard, SmallShedule } from '../../types/shedule'
import { Icon } from '../../components/icon'
import { Button } from '../../components/button/button'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { AddInput, Input } from '../../components/input/Input'
import { useNavigate } from 'react-router-dom'
import { COURSES_YEAR } from '../../consts/tabs'
import { cloneObject, getUniqueElements } from '../../utils'

const SQUARDS = [{name: 'Д-323', id: 'Д-323'}, {name: 'Д-324', id: 'Д-324'}, {name: 'Д-325', id: 'Д-325'}]

export default function Main () {

    const [shedules, setShedules] = useState<SmallShedule[]>()
    const [newSchedule, setNewSchedule] = useState<CreateSchedule>()
    const [freeCoursesYear, setFreeCoursesYear] = useState(COURSES_YEAR)
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
        console.log(schedule)
        /* const {data} = await axios.post<{data: string}>(PagesURl.SHEDULE, {
            name: name
        }) */
        /* navigate(`/${data.data}`) */
    }

    const getFreeYears = (scheduleYears: CreateScheduleYear[]) => {
        const newFreeYears = [...COURSES_YEAR, ...scheduleYears.map((year)=>year.year)]
        const unique = getUniqueElements(newFreeYears)
        return unique
    }

    const addNewYear = () => {
        if (!newSchedule) return
        setNewSchedule((prev)=>{
            if (!prev) return 
            const freeYears = getFreeYears(prev.years)
            return {...prev, years: [...prev.years, {
                year: freeYears[0], 
                squards: [], 
                start_date: new Date().toISOString(), 
                end_date: new Date().toISOString()
            }
        ]}})
    }
    const addNewYearToYear = (year: number, index: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return undefined
            const result = cloneObject(prev)
            if (result.years[index].year !== year) {
                result.years[index] = {...result.years[index], squards: []}
            }
            result.years[index].year = year
            return result
        })
    }

    const updateSquards = (newList: ScheduleSquard[], yearIndex: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.years[yearIndex].squards = newList
            return newSchedule
        })
    }
    const updateDateYear = (newDate: string, isStart: boolean, yearIndex: number) => {
        setNewSchedule((prev)=>{
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.years[yearIndex][isStart ? 'start_date' : 'end_date'] = newDate
            return newSchedule
        })
    }

    useEffect(()=>{
        if (newSchedule) {
            setFreeCoursesYear(getFreeYears(newSchedule.years))
        }
    },[newSchedule])

    useEffect(()=>{
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
                <div onClick={()=>setNewSchedule({name: '', years: []})} className={styles.container__button}><Button>Создать новое</Button></div>
            </div>
            {newSchedule !== undefined && 
                <PopupContainer onClose={()=>setNewSchedule(undefined)}>
                    <div className={styles.popup}>
                        <h2>Создание расписания</h2>
                        <div onClick={()=>setNewSchedule(undefined)} className={styles.popup__close}><Icon glyph='close' glyphColor='black'/></div>
                        <div style={{width: '95%'}}><Input value={newSchedule.name} onChange={(value)=>setNewSchedule({...newSchedule, name: value})} placeholder='Введите название'/></div>
                        {newSchedule.years.map((year, index)=>(
                            <div className={styles.popup__addList} key={year.year}>
                                <AddInput 
                                    title='Год обучения' 
                                    singleMode 
                                    selectedList={[{name: year.year, id: year.year}]} 
                                    allList={freeCoursesYear.map((year)=>({name: year, id: year}))} 
                                    changeInputList={(newList)=>addNewYearToYear(Number(newList[0].id), index)}
                                />
                                <AddInput
                                    title='Взвода'
                                    selectedList={year.squards}
                                    allList={SQUARDS}
                                    changeInputList={(newList)=>updateSquards(newList.map((item)=>({name: item.name.toString(), id: item.id.toString()})), index)}
                                />
                                <Input value={year.start_date} type='date' onChange={(val)=>updateDateYear(val, true, index)}/>
                                <Input value={year.end_date} type='date' onChange={(val)=>updateDateYear(val, false, index)}/>
                            </div>
                        ))}
                        {COURSES_YEAR.length > newSchedule.years.length && <Button onClick={addNewYear}>Добавить год обучения</Button>}
                        <Button onClick={()=>handleCreateShedule(newSchedule)}>Создать расписание</Button>
                    </div>
                </PopupContainer>
            }
        </>
    )
}