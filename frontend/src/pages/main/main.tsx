import { Helmet } from 'react-helmet-async'
import styles from './main.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { CreateSchedule, CreateScheduleYear, ScheduleSquad, SmallShedule, UpdateSchedule } from '../../types/schedule'
import { Icon } from '../../components/icon'
import { Button } from '../../components/button/button'
import PopupContainer from '../../components/popupContainer/popupContainer'
import { AddInput, Input } from '../../components/input/Input'
import { useNavigate } from 'react-router-dom'
import { COURSES_YEAR, SEMESTR_YEAR } from '../../consts'
import { cloneObject, getUniqueElements, removeElementAtIndex } from '../../utils'
import { EditSquad, NewSquad, Squad } from '../../types/squad'
import { SettingsList } from '../../components/settingsList/settingsList'
import { Audience } from '../../types/audience'
import { NewTeacher, Teacher, Vacation } from '../../types/teacher'
import { HiddenInputBlock } from '../../components/hiddenInputBlock/hiddenInputBlock'
import { Direction } from '../../types/directions'
import { AddInputList } from '../../types/input'
import { VacationBlock } from '../../components/vacationBlock/vacationBlock'
import { isValidCreateSchedule } from '../../utils/validate'
import { DeletePopup } from '../../components/deletePopup/deletePopup'
import { getSemesterStartDate } from '../../utils/date'

const DEFAULT_AUDIENCE_NAME = 'Новая Аудитория'


export default function Main() {

    const navigate = useNavigate()

    const [isEnableValidationCreateSchedule, setIsEnableValidationCreateSchedule] = useState(false)
    const [confirmDeleteScheduleId, setConfirmDeleteScheduleId] = useState<string>()
    const [confirmDeleteScheduleYearIndex, setConfirmDeleteScheduleYearIndex] = useState<number>()
    const [confirmDeleteAudienceId, setConfirmDeleteAudienceId] = useState<string>()
    const [confirmDeleteTeacherId, setConfirmDeleteTeacherId] = useState<string>()
    const [confirmDeleteSquadId, setConfirmDeleteSquadId] = useState<string>()

    const [shedules, setShedules] = useState<SmallShedule[]>()
    const [newSchedule, setNewSchedule] = useState<CreateSchedule & {isNew: boolean}>()
    const [freeCoursesYear, setFreeCoursesYear] = useState(COURSES_YEAR)

    const [squads, setSquads] = useState<Squad[]>()
    const [editSquad, setEditSquad] = useState<EditSquad>()
    const [newSquad, setNewSquad] = useState<NewSquad>()


    const [allAudience, setAllAudience] = useState<(Audience & { isEdit: boolean, isWarning: boolean })[]>()
    const [allTeachers, setAllTeachers] = useState<Teacher[]>()
    const [allDirections, setAllDirections] = useState<Direction[]>()
    const [editTeacher, setEditTeacher] = useState<NewTeacher & {id: string}>()
    const [checkEditTeacher, setCheckEditTeacher] = useState(false)
    const [newTeacher, setNewTeacher] = useState<NewTeacher>()

    const [subjects, setSubjects] = useState<AddInputList[]>()

    const handleGetAllDirections = async () => {
        const { data } = await axios.get<Direction[]>(PagesURl.DIRECTION + '/find')
        setAllDirections(data)
    }
    const handleGetAllAudience = async () => {
        const { data } = await axios.get<Audience[]>(PagesURl.AUDIENCE)
        const sortedAudience = data.sort((a, b) => {
            if (a.name === DEFAULT_AUDIENCE_NAME && b.name !== DEFAULT_AUDIENCE_NAME) {
                return 1
            } else {
                return -1
            }
        })

        setAllAudience(sortedAudience.map((el) => ({ ...el, isEdit: false, isWarning: false })))
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
        setConfirmDeleteAudienceId(undefined)
        handleGetAllAudience()
    }
    const handleGetSubjects = async () => {
        const {data} = await axios.get(PagesURl.SUBJECT + '/find')
        setSubjects(data)
    }

    const handleGetAllTeachers = async () => {
        const { data } = await axios.get<Teacher[]>(PagesURl.TEACHER)
        setAllTeachers(data)
    }
    const handleAddTeacher = async () => {
        if (!newTeacher) return
        if (newTeacher.name.length === 0 || newTeacher.rank.length === 0) {
            setCheckEditTeacher(true)
            return
        }
        for (const vacation of newTeacher.vacations) {
            if (!vacation.endDate || !vacation.startDate) {
                setCheckEditTeacher(true)
                return
            }
        }
        setCheckEditTeacher(false)
        await axios.post(PagesURl.TEACHER, {
            ...newTeacher,
            subjectIds: newTeacher.subjects.map(el => el.id)
        })
        setNewTeacher(undefined)
        handleGetAllTeachers()
    }
    const handleEditTeacher = async () => {
        if (!editTeacher) return
        if (editTeacher.name.length === 0 || editTeacher.rank.length === 0) {
            setCheckEditTeacher(true)
            return
        }
        for (const vacation of editTeacher.vacations) {
            if (!vacation.endDate || !vacation.startDate) {
                setCheckEditTeacher(true)
                return
            }
        }
        setCheckEditTeacher(false)
        await axios.put(PagesURl.TEACHER, {
            id: editTeacher.id,
            name: editTeacher.name,
            rank: editTeacher.rank,
            vacations: editTeacher.vacations,
            subjectIds: editTeacher.subjects.map((el)=>el.id)
        })
        setEditTeacher(undefined)
        handleGetAllTeachers()
    }
    const handleDeleteTeacher = async (id: string) => {
        await axios.delete(PagesURl.TEACHER + `/${id}`)
        setConfirmDeleteTeacherId(undefined)
        handleGetAllTeachers()
    }

    const editDateVacationNewTeacher = (newVacation: Vacation, index: number) => {
        if (!newTeacher) return
        const result = { ...newTeacher }
        result.vacations[index] = newVacation
        setNewTeacher(result)
    }
    const deleteVacationNewTeacher = (index: number) => {
        if (!newTeacher) return
        const result = { ...newTeacher }
        result.vacations = removeElementAtIndex(result.vacations, index)
        setNewTeacher(result)
    }
    const addVacationNewTeacher = () => {
        if (!newTeacher) return
        setCheckEditTeacher(false)
        const result = { ...newTeacher }
        result.vacations.push({ startDate: '', endDate: '' })
        setNewTeacher(result)
    }
    const editDateVacationEditTeacher = (newVacation: Vacation, index: number) => {
        if (!editTeacher) return
        const result = { ...editTeacher }
        result.vacations[index] = newVacation
        setEditTeacher(result)
    }
    const deleteVacationEditTeacher = (index: number) => {
        if (!editTeacher) return
        const result = { ...editTeacher }
        result.vacations = removeElementAtIndex(result.vacations, index)
        setEditTeacher(result)
    }

    const getEditTeacher = (teacher: Teacher) => {
        if (!subjects) return
        const selectedSubjects = subjects.filter((el)=>teacher.subjectIds.includes(el.id.toString()))
        setEditTeacher({...teacher, subjects: selectedSubjects})
    }
    const addVacationEditTeacher = () => {
        if (!editTeacher) return
        setCheckEditTeacher(false)
        const result = { ...editTeacher }
        result.vacations.push({ startDate: '', endDate: '' })
        setEditTeacher(result)
    }

    const handleGetShedules = async () => {
        const { data } = await axios.get<SmallShedule[]>(PagesURl.SCHEDULE + '/find')
        setShedules(data)
    }
    const handleDeleteSchedule = async (id: string) => {
        await axios.delete(PagesURl.SCHEDULE + `/${id}`)
        setConfirmDeleteScheduleId(undefined)
        handleGetShedules()
    }
    const handleCreateShedule = async (schedule: CreateSchedule & {isNew: boolean}) => {
        setIsEnableValidationCreateSchedule(true)
        if (!isValidCreateSchedule(schedule)) return
        setIsEnableValidationCreateSchedule(false)
        const transformedSchedule = {
            ...schedule,
            pages: schedule.pages.map(page => ({
                ...page,
                squads: page.squads.map(squad => squad.id),
                semester: page.semester ? page.semester.id : undefined
            }))
        };
        const { data } = await axios[schedule.isNew ? 'post' : 'put']<{ data: string }>(PagesURl.SCHEDULE + (!schedule.isNew ? '/full' : ''), transformedSchedule)
        setNewSchedule(undefined)
        if (transformedSchedule.isNew) {
            navigate(`/${data.data}`)
        }
    }
    const handleGetEditSchedule = async (scheduleId: string) => {
        if (!squads) return
        const {data} = await axios.get<UpdateSchedule>(PagesURl.SCHEDULE + `/${scheduleId}/update-info`)
        setNewSchedule({...data, isNew: false, id: scheduleId, pages: data.pages.map((page)=>{
            const activeSquads = squads.filter((el)=>page.squads.includes(el.id))
            return {...page, squads: activeSquads, semester: SEMESTR_YEAR.filter((el)=>el.id === page.semester)[0]}
        })})
    }


    const handleGetAllSquads = async () => {
        const { data } = await axios.get<Squad[]>(PagesURl.SQUAD)
        setSquads(data)
    }
    const handleAddSquad = async () => {
        if (!newSquad) return
        const { data } = await axios.post<{ data: string }>(PagesURl.SQUAD, {
            name: newSquad.name,
        })
        handleEditSquad({ ...newSquad, id: data.data })
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
        setConfirmDeleteSquadId(undefined)
        handleGetAllSquads()
    }

    const getFreeYears = (scheduleYears: CreateScheduleYear[]) => {
        const newFreeYears = [...COURSES_YEAR, ...scheduleYears.map((year) => year.studyYear)]
        const unique = getUniqueElements(newFreeYears)
        return unique
    }

    const addNewYear = () => {
        if (!newSchedule) return
        setNewSchedule((prev) => {
            if (!prev) return
            const freeYears = getFreeYears(prev.pages)
            return {
                ...prev, pages: [...prev.pages, {
                    studyYear: freeYears[0],
                    squads: [],
                    start: new Date().toISOString(),
                    end: new Date().toISOString(),
                    semester: undefined
                }
                ]
            }
        })
    }
    const deleteYear = (index: number) => {
        if (!newSchedule) return
        setNewSchedule((prev) => {
            if (!prev) return
            return {
                ...prev, pages: removeElementAtIndex(prev.pages, index)
            }
        })
        setConfirmDeleteScheduleYearIndex(undefined)
    }
    const addNewYearToYear = (year: number, index: number) => {
        setNewSchedule((prev) => {
            if (!prev) return undefined
            const result = cloneObject(prev)
            if (result.pages[index].studyYear !== year) {
                result.pages[index] = { ...result.pages[index], squads: [], semester: undefined }
            }
            result.pages[index].studyYear = year
            return result
        })
    }
    const addNewSemesterToYear = (newSemestr: AddInputList, index: number) => {
        setNewSchedule((prev) => {
            if (!prev) return undefined
            const result = cloneObject(prev)
            if (result.pages[index].semester !== newSemestr) {
                result.pages[index].semester = newSemestr
                result.pages[index].end = ''
                result.pages[index].start = ''
            }
            return result
        })
    }
    const changeIsEditAudience = (index: number) => {
        if (!allAudience) return
        const newAudience = [...allAudience]
        const targetAudience = newAudience[index]
        let editFlag = false
        const result = newAudience.map((el) => {
            if (el.isEdit && el.id !== targetAudience.id) {
                editFlag = true
                return { ...el, isWarning: true }
            }
            return { ...el, isWarning: false }
        })
        if (!editFlag) {
            result[index].isEdit = true
        }
        setAllAudience(result)
    }

    const updateSquards = (newList: ScheduleSquad[], yearIndex: number) => {
        setNewSchedule((prev) => {
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.pages[yearIndex].squads = newList
            return newSchedule
        })
    }
    const updateDateYear = (newDate: string, isStart: boolean, yearIndex: number) => {
        setNewSchedule((prev) => {
            if (!prev) return
            const newSchedule = cloneObject(prev)
            newSchedule.pages[yearIndex][isStart ? 'start' : 'end'] = newDate
            return newSchedule
        })
    }

    const getEditSquad = (squad: Squad) => {
        if (!allTeachers || !allAudience || !allDirections) return
        const teacher = allTeachers.find((teacher) => teacher.id === squad.daddyId)
        const fixedAudience = allAudience.find((audience) => audience.id === squad.fixedAudienceId)
        const direction = allDirections.find((direction) => direction.id === squad.directionId)
        setEditSquad({
            ...squad,
            studyYear: { id: squad.studyYear, name: squad.studyYear },
            daddy: teacher ? { id: squad.daddyId, name: teacher.name } : undefined,
            fixedAudience: fixedAudience ? { id: squad.fixedAudienceId, name: fixedAudience.name } : undefined,
            direction: direction ? { id: squad.directionId, name: direction.name } : undefined
        })
    }

    useEffect(() => {
        if (newSchedule) {
            setFreeCoursesYear(getFreeYears(newSchedule.pages))
        }
    }, [newSchedule])

    useEffect(() => {
        handleGetAllDirections()
        handleGetAllSquads()
        handleGetShedules()
        handleGetAllAudience()
        handleGetAllTeachers()
        handleGetSubjects()
    }, [])

    if (!shedules || !allAudience || !allTeachers || !squads || !allDirections || !subjects) {
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
                        {shedules.length!==0 && <div className={styles.container__shedules}>
                            {shedules.map((shedule) => (
                                <div onClick={() => navigate(`/${shedule.id}`)} className={styles.container__shedule} key={shedule.id}>
                                    <p>{shedule.name}</p>
                                    <div className={styles.container__icons}>
                                        <div onClick={(e) => { e.stopPropagation(); handleGetEditSchedule(shedule.id) }}>
                                            <Icon glyph='edit' glyphColor='black' />
                                        </div>
                                        <div onClick={(e) => { e.stopPropagation(); setConfirmDeleteScheduleId(shedule.id) }}>
                                            <Icon glyph='close' glyphColor='black' />
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>}
                        <div 
                            onClick={() => {setNewSchedule({ name: '', pages: [], isNew: true });setIsEnableValidationCreateSchedule(false)}} 
                            className={styles.container__button}
                        >
                            <Button>Создать новое</Button>
                        </div>
                    </div>
                    <div className={styles.container__right}>
                        <h3 className={styles.container__subtitle}>Глобальные настройки</h3>
                        <SettingsList title='Настройка аудиторий'>
                            <>
                                {allAudience.map((audience, index) => (
                                    <HiddenInputBlock
                                        isEdit={audience.isEdit}
                                        isWarning={audience.isWarning}
                                        key={audience.id}
                                        value={audience.name}
                                        onEdit={() => changeIsEditAudience(index)}
                                        onDelete={() => { setConfirmDeleteAudienceId(audience.id) }}
                                        onEnter={(val) => { handleEditAudience(audience.id, val) }}
                                    />
                                ))}
                                <Button onClick={handleAddAudience} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
                            </>
                        </SettingsList>
                        <SettingsList title='Настройка преподавателей'>
                            <>
                                {allTeachers.map((teacher) => (
                                    <HiddenInputBlock
                                        key={teacher.id}
                                        value={`${teacher.rank} ${teacher.name}`}
                                        onDelete={() => { setConfirmDeleteTeacherId(teacher.id) }}
                                        onEdit={() => getEditTeacher(teacher)}
                                    />
                                ))}
                                <Button
                                    onClick={() => setNewTeacher({ name: '', rank: '', vacations: [], subjects: [] })}
                                    size={'max'}
                                    variant={'whiteMain'}
                                >
                                    <Icon glyph='add' glyphColor='grey' />
                                </Button>
                            </>
                        </SettingsList>
                        <SettingsList title="Настройка взводов">
                            <>
                                {squads.map((squad) => (
                                    <HiddenInputBlock
                                        key={squad.id}
                                        value={`${squad.name}`}
                                        onDelete={() => { setConfirmDeleteSquadId(squad.id) }}
                                        onEdit={() => getEditSquad(squad)}
                                    />
                                ))}
                                <Button onClick={() => setNewSquad({ name: '' })} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
                            </>
                        </SettingsList>
                    </div>
                </div>
            </div>
            {newSchedule !== undefined &&
                <PopupContainer displayClose onClose={() => {setNewSchedule(undefined);setIsEnableValidationCreateSchedule(false)}} isActive={confirmDeleteScheduleYearIndex === undefined}>
                    <div className={styles.popup}>
                        <h2>{newSchedule.isNew ? 'Создание' : 'Редактирование'} расписания</h2>
                        <div style={{ width: '95%' }}><Input value={newSchedule.name} onChange={(value) => setNewSchedule({ ...newSchedule, name: value })} placeholder='Введите название' /></div>
                        {newSchedule.pages.map((year, index) => (
                            <div className={styles.popup__addList} key={year.studyYear}>
                                <div onClick={(e)=>{e.stopPropagation();setConfirmDeleteScheduleYearIndex(index)}} className={styles.popup__delete}>
                                    <Icon glyph='trash' glyphColor='error'/>
                                </div>
                                <AddInput
                                    isError={isEnableValidationCreateSchedule}
                                    title='Год обучения'
                                    singleMode
                                    selectedList={[{ name: year.studyYear, id: year.studyYear }]}
                                    allList={freeCoursesYear.map((year) => ({ name: year, id: year }))}
                                    changeInputList={(newList) => addNewYearToYear(Number(newList[0].id), index)}
                                />
                                <AddInput
                                    title='Семестр'
                                    singleMode
                                    isError={isEnableValidationCreateSchedule}
                                    selectedList={year.semester ? [year.semester] : []}
                                    allList={year.studyYear === 1 ? SEMESTR_YEAR.filter((el) => el.id === 0) : SEMESTR_YEAR}
                                    changeInputList={(newList) => addNewSemesterToYear(newList[0], index)}
                                />
                                {squads &&
                                    <AddInput
                                        title='Взвода'
                                        isError={isEnableValidationCreateSchedule}
                                        selectedList={year.squads}
                                        allList={squads.filter((squad) => squad.studyYear === year.studyYear)}
                                        changeInputList={(newList) => updateSquards(newList.map((item) => ({ name: item.name.toString(), id: item.id.toString() })), index)}
                                    />
                                }
                                {year.semester && <div className={styles.popup__line}>
                                    <p>Дата первого занятия</p>
                                    <Input startDate={getSemesterStartDate(Number(year.semester.id))} isError={isEnableValidationCreateSchedule} value={year.start} type='date' onChange={(val) => updateDateYear(val, true, index)} />
                                </div>}
                                {year.semester && <div className={styles.popup__line}>
                                    <p>Дата последнего занятия</p>
                                    <Input startDate={getSemesterStartDate(Number(year.semester.id))} isError={isEnableValidationCreateSchedule} value={year.end} type='date' onChange={(val) => updateDateYear(val, false, index)} />
                                </div>}
                            </div>
                        ))}
                        {COURSES_YEAR.length > newSchedule.pages.length && <Button onClick={addNewYear}>Добавить год обучения</Button>}
                        <Button onClick={() => handleCreateShedule(newSchedule)}>{newSchedule.isNew ? 'Создать' : 'Редактировать'} расписание</Button>
                    </div>
                </PopupContainer>
            }
            {editTeacher &&
                <PopupContainer displayClose onClose={() => setEditTeacher(undefined)}>
                    <div className={styles.edit}>
                        <h2>Редактирование преподавателя</h2>
                        <Input isError={checkEditTeacher} value={editTeacher.name} placeholder='Фамилия' onChange={(val) => setEditTeacher({ ...editTeacher, name: val })} />
                        <Input isError={checkEditTeacher} value={editTeacher.rank} placeholder='Звание' onChange={(val) => setEditTeacher({ ...editTeacher, rank: val })} />
                        <AddInput
                            enableSearch
                            minWidth={367}
                            allList={subjects}
                            selectedList={editTeacher.subjects}
                            changeInputList={(newList)=>setEditTeacher({...editTeacher, subjects: newList})}
                            title='Приоритетные дисциплины'
                        />
                        {editTeacher.vacations.map((el, index) => (
                            <VacationBlock
                                isCheckError={checkEditTeacher}
                                key={`${el.startDate}--${el.endDate}`}
                                title={`Отпуск ${index + 1}`}
                                start={el.startDate}
                                end={el.endDate}
                                onChangeDate={(vacation) => { editDateVacationEditTeacher(vacation, index) }}
                                onDelete={() => { deleteVacationEditTeacher(index) }}
                            />
                        ))}
                        <Button onClick={addVacationEditTeacher} variant={'whiteMain'} textColor={'grey'} size={'max'}>
                            <Icon glyph='add' glyphColor='grey' />
                            <p>Добавить отпуск</p>
                        </Button>
                        <Button onClick={handleEditTeacher}>Сохранить</Button>
                    </div>
                </PopupContainer>
            }
            {newTeacher &&
                <PopupContainer displayClose onClose={() => setNewTeacher(undefined)}>
                    <div className={styles.edit}>
                        <h2>Создание преподавателя</h2>
                        <Input isError={checkEditTeacher} value={newTeacher.name} placeholder='Фамилия' onChange={(val) => setNewTeacher({ ...newTeacher, name: val })} />
                        <Input isError={checkEditTeacher} value={newTeacher.rank} placeholder='Звание' onChange={(val) => setNewTeacher({ ...newTeacher, rank: val })} />
                        <AddInput
                            enableSearch
                            minWidth={367}
                            allList={subjects}
                            selectedList={newTeacher.subjects}
                            changeInputList={(newList)=>setNewTeacher({...newTeacher, subjects: newList})}
                            title='Приоритетные дисциплины'
                        />
                        {newTeacher.vacations.map((el, index) => (
                            <VacationBlock
                                isCheckError={checkEditTeacher}
                                key={`${el.startDate}--${el.endDate}`}
                                title={`Отпуск ${index + 1}`}
                                start={el.startDate}
                                end={el.endDate}
                                onChangeDate={(vacation) => editDateVacationNewTeacher(vacation, index)}
                                onDelete={() => { deleteVacationNewTeacher(index) }}
                            />
                        ))}
                        <Button onClick={addVacationNewTeacher} variant={'whiteMain'} textColor={'grey'} size={'max'}>
                            <Icon glyph='add' glyphColor='grey' />
                            <p>Добавить отпуск</p>
                        </Button>
                        <Button onClick={handleAddTeacher}>Создать преподавателя</Button>
                    </div>
                </PopupContainer>
            }

            {editSquad &&
                <PopupContainer onClose={() => setEditSquad(undefined)} displayClose>
                    <div className={styles.edit}>
                        <h2>Редактирование Взвода</h2>
                        <Input value={editSquad.name} placeholder='Название' onChange={(val) => setEditSquad({ ...editSquad, name: val })} />
                        <div className={styles.edit__line}>
                            <p>Год обучения</p>
                            <AddInput
                                selectedList={editSquad.studyYear ? [editSquad.studyYear] : []}
                                allList={COURSES_YEAR.map((item) => ({ id: item, name: item }))}
                                title='Выберите год обучения'
                                singleMode
                                changeInputList={(list) => setEditSquad({ ...editSquad, studyYear: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Ответственный преподаватель</p>
                            <AddInput
                                selectedList={editSquad.daddy ? [editSquad.daddy] : []}
                                allList={allTeachers}
                                title='Выберите ответственного преподавателя'
                                singleMode
                                changeInputList={(list) => setEditSquad({ ...editSquad, daddy: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Аудитория</p>
                            <AddInput
                                selectedList={editSquad.fixedAudience ? [editSquad.fixedAudience] : []}
                                allList={allAudience}
                                title='Выберите аудиторию взвода'
                                singleMode
                                changeInputList={(list) => setEditSquad({ ...editSquad, fixedAudience: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Направление</p>
                            <AddInput
                                selectedList={editSquad.direction ? [editSquad.direction] : []}
                                allList={allDirections}
                                title='Выберите направление взвода'
                                singleMode
                                changeInputList={(list) => setEditSquad({ ...editSquad, direction: list[0] })}
                            />
                        </div>
                        <Button onClick={() => handleEditSquad()}>Сохранить</Button>
                    </div>
                </PopupContainer>
            }
            {newSquad &&
                <PopupContainer displayClose onClose={() => setNewSquad(undefined)}>
                    <div className={styles.edit}>
                        <h2>Создание Взвода</h2>
                        <Input value={newSquad.name} placeholder='Название' onChange={(val) => setNewSquad({ ...newSquad, name: val })} />
                        <div className={styles.edit__line}>
                            <p>Год обучения</p>
                            <AddInput
                                selectedList={newSquad.studyYear ? [newSquad.studyYear] : []}
                                allList={COURSES_YEAR.map((item) => ({ id: item, name: item }))}
                                title='Выберите год обучения'
                                singleMode
                                changeInputList={(list) => setNewSquad({ ...newSquad, studyYear: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Ответственный преподаватель</p>
                            <AddInput
                                selectedList={newSquad.daddy ? [newSquad.daddy] : []}
                                allList={allTeachers}
                                title='Выберите ответственного преподавателя'
                                singleMode
                                changeInputList={(list) => setNewSquad({ ...newSquad, daddy: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Аудитория</p>
                            <AddInput
                                selectedList={newSquad.fixedAudience ? [newSquad.fixedAudience] : []}
                                allList={allAudience}
                                title='Выберите аудиторию взвода'
                                singleMode
                                changeInputList={(list) => setNewSquad({ ...newSquad, fixedAudience: list[0] })}
                            />
                        </div>
                        <div className={styles.edit__line}>
                            <p>Направление</p>
                            <AddInput
                                selectedList={newSquad.direction ? [newSquad.direction] : []}
                                allList={allDirections}
                                title='Выберите направление взвода'
                                singleMode
                                changeInputList={(list) => setNewSquad({ ...newSquad, direction: list[0] })}
                            />
                        </div>
                        <Button onClick={handleAddSquad}>Создать взвод</Button>
                    </div>
                </PopupContainer>
            }
            {confirmDeleteScheduleId && 
                <DeletePopup
                    title='Удаление расписания'
                    text='Вы уверены, что хотите удалить расписание?'
                    onCancel={()=>setConfirmDeleteScheduleId(undefined)}
                    onDelete={()=>handleDeleteSchedule(confirmDeleteScheduleId)}
                />
            }
            {confirmDeleteScheduleYearIndex!==undefined && 
                <DeletePopup
                    title='Удаление года обучения'
                    text='Вы уверены, что хотите удалить год обучения?'
                    onCancel={()=>setConfirmDeleteScheduleYearIndex(undefined)}
                    onDelete={()=>deleteYear(confirmDeleteScheduleYearIndex)}
                />
            }
            {confirmDeleteAudienceId && 
                <DeletePopup
                    title='Удаление аудитории'
                    text='Вы уверены, что хотите удалить аудиторию?'
                    onCancel={()=>setConfirmDeleteAudienceId(undefined)}
                    onDelete={()=>handleDeleteAudience(confirmDeleteAudienceId)}
                />
            }
            {confirmDeleteTeacherId && 
                <DeletePopup
                    title='Удаление преподавателя'
                    text='Вы уверены, что хотите удалить преподавателя?'
                    onCancel={()=>setConfirmDeleteTeacherId(undefined)}
                    onDelete={()=>handleDeleteTeacher(confirmDeleteTeacherId)}
                />
            }
            {confirmDeleteSquadId && 
                <DeletePopup
                    title='Удаление взвода'
                    text='Вы уверены, что хотите удалить взвод?'
                    onCancel={()=>setConfirmDeleteSquadId(undefined)}
                    onDelete={()=>handleDeleteSquad(confirmDeleteSquadId)}
                />
            }
        </>
    )
}