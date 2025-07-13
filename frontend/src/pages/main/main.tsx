import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { CreateSchedule, CreateScheduleYear, ScheduleSquad, SmallShedule } from '../../types/schedule'
import { Icon } from '../../components/icon'
import { Button } from '../../components/button/button'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { AddInput, Input } from '../../components/input/Input'
import { useNavigate } from 'react-router-dom'
import { COURSES_YEAR } from '../../consts'
import { cloneObject, getUniqueElements } from '../../utils'
import { EditSquad, NewSquad, Squad } from '../../types/squad'
import { SettingsList } from '../../components/settingsList/settingsList'
import { Audience } from '../../types/audience'
import { NewTeacher, Teacher } from '../../types/teacher'
import { HiddenInputBlock } from '../../components/hiddenInputBlock/hiddenInputBlock'
import { Direction } from '../../types/directions'

const DEFAULT_AUDIENCE_NAME = 'Новая Аудитория'


export default function Main () {

    const navigate = useNavigate()

    const [shedules, setShedules] = useState<SmallShedule[]>()
    const [newSchedule, setNewSchedule] = useState<CreateSchedule>()
    const [freeCoursesYear, setFreeCoursesYear] = useState(COURSES_YEAR)

    const [squads, setSquads] = useState<Squad[]>()
    const [editSquad, setEditSquad] = useState<EditSquad>()
    const [newSquad, setNewSquad] = useState<NewSquad>()


    const [allAudience, setAllAudience] = useState<Audience[]>()
    const [allTeachers, setAllTeachers] = useState<Teacher[]>()
    const [allDirections, setAllDirections] = useState<Direction[]>()
    const [editTeacher, setEditTeacher] = useState<Teacher>()
    const [newTeacher, setNewTeacher] = useState<NewTeacher>()

    const handleGetAllDirections = async () => {
        const {data} = await axios.get<Direction[]>(PagesURl.DIRECTION + '/find')
        //console.log(data)
        setAllDirections(data)
    }

    const handleGetAllAudience = async () => {
        const {data} = await axios.get<Audience[]>(PagesURl.AUDIENCE)
        setAllAudience(data.sort((a, b)=>{
            if (a.name === DEFAULT_AUDIENCE_NAME && b.name !== DEFAULT_AUDIENCE_NAME) {
                return 1
            } else {
                return -1
            }
        }))
    }
    const handleAddAudience = async () => {
        await axios.post(PagesURl.AUDIENCE, {
            name: 'Новая Аудитория'
        })
        handleGetAllAudience()
    }
    const handleEditAudience = async (id: string, name: string) => {
        await axios.put(PagesURl.AUDIENCE, {
            id,
            name
        })
        handleGetAllAudience()
    }
    const handleDeleteAudience = async (id: string) => {
        await axios.delete(PagesURl.AUDIENCE + `/${id}`)
        handleGetAllAudience()
    }

    const handleGetAllTeachers = async () => {
        const {data } = await axios.get<Teacher[]>(PagesURl.TEACHER)
        //console.log(data)
        setAllTeachers(data)
    }
    const handleAddTeacher = async () => {
        await axios.post(PagesURl.TEACHER, {
            ...newTeacher
        })
        setNewTeacher(undefined)
        handleGetAllTeachers()
    }
    const handleEditTeacher = async () => {
        if (!editTeacher) return
        await axios.put(PagesURl.TEACHER, {
            id: editTeacher.id,
            name: editTeacher.name,
            rank: editTeacher.rank
        })
        setEditTeacher(undefined)
        handleGetAllTeachers()
    }
    const handleDeleteTeacher = async (id: string) => {
        await axios.delete(PagesURl.TEACHER + `/${id}`)
        handleGetAllAudience()
    }

    const handleGetShedules = async () => {
        const {data} = await axios.get<SmallShedule[]>(PagesURl.SCHEDULE + '/find')
        setShedules(data)
    }
    const handleDeleteSchedule = async (id: string) => {
        await axios.delete(PagesURl.SCHEDULE + `/${id}`)
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
        const {data} = await axios.post<{data: string}>(PagesURl.SCHEDULE, transformedSchedule)
        navigate(`/${data.data}`)
    }


    const handleGetAllSquads = async () => {
        const {data} = await axios.get<Squad[]>(PagesURl.SQUAD)
        setSquads(data)
    }
    const handleAddSquad = async () => {
        if (!newSquad) return
        const {data} = await axios.post<{data: string}>(PagesURl.SQUAD, {
            name: newSquad.name,
        })
        handleEditSquad({...newSquad, id:data.data})
        setNewSquad(undefined)
        handleGetAllSquads()
    }
    const handleEditSquad = async (newSquad?: EditSquad) => {
        const targetSquad = newSquad ? newSquad : editSquad
        if (!targetSquad) return
        await axios.put(PagesURl.SQUAD, {
            id: targetSquad.id,
            name: targetSquad.name,
            studyYear: targetSquad.studyYear ? {
                data: targetSquad.studyYear.id
            } : undefined,
            daddyId: targetSquad.daddy ? {
                data: targetSquad.daddy.id
            } : undefined,
            fixedAudienceId: targetSquad.fixedAudience ? {
                data: targetSquad.fixedAudience.id
            } : undefined,
            directionId: targetSquad.direction ? {
                data: targetSquad.direction.id
            } : undefined
        })
        setEditSquad(undefined)
        handleGetAllSquads()
    }
    const handleDeleteSquad = async (id: string) => {
        await axios.delete(PagesURl.SQUAD + `/${id}`)
        handleGetAllSquads()
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

    const getEditSquad = (squad: Squad) => {
        if (!allTeachers || !allAudience || !allDirections) return
        const teacher = allTeachers.find((teacher)=>teacher.id === squad.daddyId)
        const fixedAudience = allAudience.find((audience)=>audience.id === squad.fixedAudienceId)
        const direction = allDirections.find((direction)=>direction.id === squad.directionId)
        setEditSquad({...squad,
            studyYear: {id: squad.studyYear, name: squad.studyYear},
            daddy: teacher ? {id: squad.daddyId, name: teacher.name} : undefined,
            fixedAudience: fixedAudience ? {id: squad.fixedAudienceId, name: fixedAudience.name} : undefined,
            direction: direction ? {id: squad.directionId, name: direction.name} : undefined
        })
    }

    useEffect(()=>{
        if (newSchedule) {
            setFreeCoursesYear(getFreeYears(newSchedule.pages))
        }
    },[newSchedule])

    useEffect(()=>{
        handleGetAllDirections()
        handleGetAllSquads()
        handleGetShedules()
        handleGetAllAudience()
        handleGetAllTeachers()
    },[])

    useEffect(()=>{
        if (allAudience && allTeachers) {
            handleGetAllAudience()
            handleGetAllTeachers()
        }
    },[allAudience, allTeachers])

    if (!shedules || !allAudience || !allTeachers || !squads || !allDirections) {
        return <></>
    }

    return (
        <>
            <Helmet>
                <title>Главная</title>
            </Helmet>
            <div className={styles.container}>
                <h1 className={styles.container__title}>Расписание кафедры СП</h1>
                <div className={styles.container__content}>
                    <div className={styles.container__left}>
                        <h3 className={styles.container__subtitle}>Сохранённые расписания</h3>
                        <div className={styles.container__shedules}>
                            {shedules.map((shedule) => (
                                <div onClick={() => navigate(`/${shedule.id}`)} className={styles.container__shedule} key={shedule.id}>
                                    <p>{shedule.name}</p>
                                    <div onClick={(e) => {e.stopPropagation(); handleDeleteSchedule(shedule.id) }}><Icon glyph='close' glyphColor='black' /></div>
                                </div>
                            ))}
                        </div>
                        <div onClick={() => setNewSchedule({ name: '', pages: [] })} className={styles.container__button}><Button>Создать новое</Button></div>
                    </div>
                    <div className={styles.container__right}>
                        <h3 className={styles.container__subtitle}>Настройки</h3>
                        <SettingsList title='Настройка аудиторий'>
                            <>
                                {allAudience.map((audience) => (
                                    <HiddenInputBlock 
                                        key={audience.id} 
                                        value={audience.name} 
                                        onDelete={()=>{handleDeleteAudience(audience.id)}} 
                                        onEnter={(val) => {handleEditAudience(audience.id, val)}} 
                                    />
                                ))}
                                <Button onClick={handleAddAudience} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey'/></Button>
                            </>
                        </SettingsList>
                        <SettingsList title='Настройка преподавателей'>
                            <>
                                {allTeachers.map((teacher) => (
                                    <HiddenInputBlock
                                        key={teacher.id}
                                        value={`${teacher.rank} ${teacher.name}`}
                                        onDelete={() => { handleDeleteTeacher(teacher.id) }}
                                        onEdit={()=>setEditTeacher(teacher)}
                                    />
                                ))}
                                <Button onClick={()=>setNewTeacher({name: '', rank: ''})} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
                            </>
                        </SettingsList>
                        <SettingsList title="Настройка взводов">
                            <>
                                {squads.map((squad)=>(
                                    <HiddenInputBlock
                                        key={squad.id}
                                        value={`${squad.name}`}
                                        onDelete={() => { handleDeleteSquad(squad.id) }}
                                        onEdit={()=>getEditSquad(squad)}
                                    />
                                ))}
                                <Button onClick={()=>setNewSquad({name: ''})} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
                            </>
                        </SettingsList>
                    </div>
                </div>
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
                                <div className={styles.popup__line}>
                                    <p>Дата первого занятия</p>
                                    <Input value={year.start} type='date' onChange={(val)=>updateDateYear(val, true, index)}/>
                                </div>
                                <div className={styles.popup__line}>
                                    <p>Дата последнего занятия</p>
                                    <Input value={year.end} type='date' onChange={(val)=>updateDateYear(val, false, index)}/>
                                </div>
                            </div>
                        ))}
                        {COURSES_YEAR.length > newSchedule.pages.length && <Button onClick={addNewYear}>Добавить год обучения</Button>}
                        <Button onClick={()=>handleCreateShedule(newSchedule)}>Создать расписание</Button>
                    </div>
                </PopupContainer>
            }
            {editTeacher && 
                <PopupContainer onClose={()=>setEditTeacher(undefined)}>
                    <div className={styles.edit}>
                        <h2>Редактирование преподавателя</h2>
                        <Input value={editTeacher.name} placeholder='Фамилия' onChange={(val)=>setEditTeacher({...editTeacher, name: val})}/>
                        <Input value={editTeacher.rank} placeholder='Звание'  onChange={(val)=>setEditTeacher({...editTeacher, rank: val})}/>
                        <Button onClick={handleEditTeacher}>Изменить преподавателя</Button>
                    </div>
                </PopupContainer>
            }
            {newTeacher && 
                <PopupContainer onClose={()=>setNewTeacher(undefined)}>
                    <div className={styles.edit}>
                        <h2>Создание преподавателя</h2>
                        <Input value={newTeacher.name} placeholder='Фамилия' onChange={(val)=>setNewTeacher({...newTeacher, name: val})}/>
                        <Input value={newTeacher.rank} placeholder='Звание'  onChange={(val)=>setNewTeacher({...newTeacher, rank: val})}/>
                        <Button onClick={handleAddTeacher}>Создать преподавателя</Button>
                    </div>
                </PopupContainer>
            }

            {editSquad && 
                <PopupContainer onClose={()=>setEditSquad(undefined)}>
                    <div className={styles.edit}>
                        <h2>Редактирование Взвода</h2>
                        <Input value={editSquad.name} placeholder='Название' onChange={(val)=>setEditSquad({...editSquad, name: val})}/>
                        <div className={styles.edit__line}>
                            <p>Год обучения</p>
                            <AddInput 
                                selectedList={editSquad.studyYear ? [editSquad.studyYear] : []}
                                allList={COURSES_YEAR.map((item)=>({id: item, name: item}))}
                                title='Выберите год обучения'
                                singleMode
                                changeInputList={(list)=>setEditSquad({...editSquad, studyYear: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Ответственный преподаватель</p>
                            <AddInput 
                                selectedList={editSquad.daddy ? [editSquad.daddy] : []}
                                allList={allTeachers}
                                title='Выберите ответственного преподавателя'
                                singleMode
                                changeInputList={(list)=>setEditSquad({...editSquad, daddy: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Аудитория</p>
                            <AddInput 
                                selectedList={editSquad.fixedAudience ? [editSquad.fixedAudience] : []}
                                allList={allAudience}
                                title='Выберите аудиторию взвода'
                                singleMode
                                changeInputList={(list)=>setEditSquad({...editSquad, fixedAudience: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Направление</p>
                            <AddInput 
                                selectedList={editSquad.direction ? [editSquad.direction] : []}
                                allList={allDirections}
                                title='Выберите направление взвода'
                                singleMode
                                changeInputList={(list)=>setEditSquad({...editSquad, direction: list[0]})}
                            />
                        </div>
                        <Button onClick={()=>handleEditSquad()}>Изменить взвод</Button>
                    </div>
                </PopupContainer>
            }
            {newSquad && 
                <PopupContainer onClose={()=>setNewSquad(undefined)}>
                    <div className={styles.edit}>
                        <h2>Создание Взвода</h2>
                        <Input value={newSquad.name} placeholder='Название' onChange={(val)=>setNewSquad({...newSquad, name: val})}/>
                        <div className={styles.edit__line}>
                            <p>Год обучения</p>
                            <AddInput 
                                selectedList={newSquad.studyYear ? [newSquad.studyYear] : []}
                                allList={COURSES_YEAR.map((item)=>({id: item, name: item}))}
                                title='Выберите год обучения'
                                singleMode
                                changeInputList={(list)=>setNewSquad({...newSquad, studyYear: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Ответственный преподаватель</p>
                            <AddInput 
                                selectedList={newSquad.daddy ? [newSquad.daddy] : []}
                                allList={allTeachers}
                                title='Выберите ответственного преподавателя'
                                singleMode
                                changeInputList={(list)=>setNewSquad({...newSquad, daddy: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Аудитория</p>
                            <AddInput 
                                selectedList={newSquad.fixedAudience ? [newSquad.fixedAudience] : []}
                                allList={allAudience}
                                title='Выберите аудиторию взвода'
                                singleMode
                                changeInputList={(list)=>setNewSquad({...newSquad, fixedAudience: list[0]})}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Направление</p>
                            <AddInput 
                                selectedList={newSquad.direction ? [newSquad.direction] : []}
                                allList={allDirections}
                                title='Выберите направление взвода'
                                singleMode
                                changeInputList={(list)=>setNewSquad({...newSquad, direction: list[0]})}
                            />
                        </div>
                        <Button onClick={handleAddSquad}>Создать взвод</Button>
                    </div>
                </PopupContainer>
            }
        </>
    )
}