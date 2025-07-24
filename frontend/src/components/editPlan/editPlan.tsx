import { Direction, NewDirection } from '../../types/directions'
import { HiddenInputBlock } from '../hiddenInputBlock/hiddenInputBlock'
import { SettingsList } from '../settingsList/settingsList'
import styles from './editPlan.module.scss'
import axios, { PagesURl } from '../../services/api/api'
import { useEffect, useState } from 'react'
import { DeletePopup } from '../deletePopup/deletePopup'
import { Subject } from '../../types/subject'
import { Button } from '../button/button'
import { Icon } from '../icon'
import { NewTheme, Theme } from '../../types/theme'
import PopupContainer from '../popupContainer/popupContainer'
import { AddInput, Input, SearchInput } from '../input/Input'
import { EditLesson, Lesson, NewSmallLesson } from '../../types/lesson'
import { DIRECTION_TYPE, LESSON_TYPE, SEMESTER_COUNT } from '../../consts'

const NEW_SUBJECT_NAME = 'Новый предмет'

export function EditPlan() {

  const [isOpenDirections, setIsOpensDirections] = useState<boolean>(true)
  const [allDirections, setAllDirections] = useState<(Direction & { isEdit: boolean, isWarning: boolean })[]>()
  const [confirmDeleteDirectionId, setConfirmDeleteDirectionId] = useState<string>()
  const [selectedDirection, setSelectedDirection] = useState<{name: string, id: string}>()
  const [editDirection, setEditDirection] = useState<NewDirection>()
  const [newDirection, setNewDirection] = useState<Partial<NewDirection>>()
  const [searchDirection, setSearchDirection] = useState('')
  const [checkDirection, setCheckDirection] = useState(false)

  const [isOpenSubjects, setIsOpenSubjects] = useState<boolean>(false)
  const [allSubjects, setAllSubjects] = useState<(Subject & { isEdit: boolean, isWarning: boolean})[]>()
  const [confirmDeleteSubjectId, setConfirmDeleteSubjectId] = useState<string>()
  const [selectedSubject, setSelectedSubject] = useState<{name: string, id: string}>()
  const [searchSubject, setSearchSubject] = useState('')

  const [isOpenThemes, setIsOpenThemes] = useState<boolean>(false)
  const [allThemes, setAllThemes] = useState<(Theme & { isEdit: boolean, isWarning: boolean})[]>()
  const [confirmDeleteThemeId, setConfirmDeleteThemeId] = useState<string>()
  const [selectedTheme, setSelectedTheme] = useState<{name: string, id: string}>()
  const [editTheme, setEditTheme] = useState<Theme>()
  const [newTheme, setNewTheme] = useState<NewTheme>()
  const [searchTheme, setSearchTheme] = useState('')
  const [checkTheme, setCheckTheme] = useState(false)

  const [isOpenLessons, setIsOpenLessons] = useState<boolean>(false)
  const [allLessons, setAllLessons] = useState<(Lesson & { isEdit: boolean, isWarning: boolean})[]>()
  const [confirmDeleteLessonId, setConfirmDeleteLessonId] = useState<string>()
  const [editLesson, setEditLesson] = useState<EditLesson>()
  const [newLesson, setNewLesson] = useState<NewSmallLesson>()
  const [searchLesson, setSearchLesson] = useState('')
  const [checkLesson, setCheckLesson] = useState(false)

  const handleGetAllDirections = async () => {
    const { data } = await axios.get<Direction[]>(PagesURl.DIRECTION + '/find')
    setAllDirections(data.map((el) => ({ ...el, isEdit: false, isWarning: false })))
    setAllSubjects(undefined)
  }
  const handleDeleteDirection = async (id: string) => {
    await axios.delete(PagesURl.DIRECTION + `/${id}`)
    setConfirmDeleteDirectionId(undefined)
    handleGetAllDirections()
  }
  const handleCreateDirection = async (isNew: boolean, name: string | undefined, type: number | undefined, id: string | undefined) => {
    if (type === undefined || !name) {
      setCheckDirection(true)
    }
    setCheckDirection(false)
    await axios[isNew ? 'post' : 'put'](PagesURl.DIRECTION, {
      name,
      type,
      id
    })
    setNewDirection(undefined)
    setEditDirection(undefined)
    handleGetAllDirections()
  }

  const handleGetAllSubjects = async (directionId: string | undefined) => {
    const {data} = await axios.get<Subject[]>(PagesURl.SUBJECT + '/find', {
      params: {
        directionId: directionId
      }
    })
    setAllSubjects(data.map((el) => ({ ...el, isEdit: false, isWarning: false })))
    setIsOpenSubjects(true)
  }
  const handleEditSubject = async (id: string, name: string) => {
    await axios.put(PagesURl.SUBJECT, {
      id,
      name
    })
    handleGetAllSubjects(selectedDirection?.id)
  }
  const handleDeleteSubject = async (id: string) => {
    await axios.delete(PagesURl.SUBJECT + `/${id}`)
    setConfirmDeleteSubjectId(undefined)
    handleGetAllSubjects(selectedDirection?.id)
  }
  const handleCreateSubject = async () => {
    await axios.post(PagesURl.SUBJECT, {
      name: NEW_SUBJECT_NAME,
      directionId: selectedDirection?.id
    })
    handleGetAllSubjects(selectedDirection?.id)
  }

  const handleGetAllThemes = async (subjectId: string | undefined) => {
    const {data} = await axios.get<Theme[]>(PagesURl.THEME + '/find', {
      params: {
        directionId: selectedDirection?.id,
        subjectId: subjectId
      }
    })
    setAllThemes(data.map((el) => ({ ...el, isEdit: false, isWarning: false })))
    setIsOpenThemes(true)
  }
  const handleDeleteTheme = async (id: string) => {
    await axios.delete(PagesURl.THEME + `/${id}`)
    setConfirmDeleteThemeId(undefined)
    handleGetAllThemes(selectedSubject?.id)
  }
  const handleCreateTheme = async (isNew:boolean, number: number | undefined, id: string | undefined) => {
    if (number === undefined || !number) {
      setCheckTheme(true)
      return
    }
    setCheckTheme(false)
    await axios[isNew ? 'post' : 'put'](PagesURl.THEME, {
      id,
      number,
      name,
      subjectId: selectedSubject?.id
    })
    setNewTheme(undefined)
    setEditTheme(undefined)
    handleGetAllThemes(selectedSubject?.id)
  }

  const handleGetAllLessons = async (themeId: string | undefined) => {
    const {data} = await axios.get(PagesURl.LESSON + '/find', {
      params: {
        themeId
      }
    })
    setAllLessons(data)
    setIsOpenLessons(true)
  }
  const handleDeleteLesson = async (lessonId: string) => {
    await axios.delete(PagesURl.LESSON + `/${lessonId}`)
    setConfirmDeleteLessonId(undefined)
    handleGetAllLessons(selectedTheme?.id)
  }
  const handleCreateLesson = async (
      isNew: boolean, 
      number: number | undefined, 
      type: number | undefined, 
      semester: number | undefined, 
      hoursCount: number | undefined = 2, id: string | undefined
    ) => {
    if (number === undefined  || !number || type === undefined || semester === undefined || hoursCount === 0 || hoursCount === undefined) {
      setCheckLesson(true)
      return
    }
    setCheckLesson(false)
    await axios[isNew ? 'post' : 'put'](PagesURl.LESSON, {
      id,
      number,
      type,
      semester,
      hoursCount,
      themeId: selectedTheme?.id,
      subjectId: selectedSubject?.id
    })
    setNewLesson(undefined)
    setEditLesson(undefined)
    handleGetAllLessons(selectedTheme?.id)
  }

  const changeIsEditSubject = (index: number) => {
    if (!allSubjects) return
    const newSubjects = [...allSubjects]
    const targetSubject = newSubjects[index]
    let editFlag = false
    const result = newSubjects.map((el) => {
      if (el.isEdit && el.id !== targetSubject.id) {
        editFlag = true
        return { ...el, isWarning: true }
      }
      return { ...el, isWarning: false }
    })
    if (!editFlag) {
      result[index].isEdit = true
    }
    setAllSubjects(result)
  }

  useEffect(() => {
    handleGetAllDirections()
  }, [])

  useEffect(()=>{
    if (selectedDirection) {
      setSelectedSubject(undefined)
      setAllSubjects(undefined)
      setSelectedTheme(undefined)
      setAllThemes(undefined)
      setAllLessons(undefined)
      setSearchDirection('')
      setSearchSubject('')
      setSearchTheme('')
      setSearchLesson('')
      handleGetAllSubjects(selectedDirection.id)
    }
  },[selectedDirection])
  useEffect(()=>{
    if (selectedSubject) {
      setSelectedTheme(undefined)
      setAllThemes(undefined)
      setAllLessons(undefined)
      setSearchSubject('')
      setSearchTheme('')
      setSearchLesson('')
      handleGetAllThemes(selectedSubject.id)
    }
  },[selectedSubject])
  useEffect(()=>{
    if (selectedTheme) {
      setSearchTheme('')
      setSearchLesson('')
      setAllLessons(undefined)
      handleGetAllLessons(selectedTheme?.id)
    }
  },[selectedTheme])


  if (!allDirections) return <></>

  return (
    <>
      <h3 className={styles.container__subtitle}>Настройки тематического плана</h3>
      <SettingsList isSelected={selectedDirection !== undefined} changeIsOpen={setIsOpensDirections} isOpenList={isOpenDirections} title={selectedDirection ? selectedDirection.name : 'Направления'}>
        <>
          <SearchInput searchValue={searchDirection} changeSearchValue={setSearchDirection}/>
          <div className={styles.container__list}>
            {allDirections.filter((el) => el.name.toLowerCase().includes(searchDirection.toLowerCase())).map((el, index) => (
              <HiddenInputBlock
                isEdit={el.isEdit}
                isWarning={el.isWarning}
                key={`${el.id}--${index}`}
                value={`${el.name} (${el.type!==undefined ? DIRECTION_TYPE[el.type] : DIRECTION_TYPE[0]})`}
                onEdit={() => setEditDirection({...el, type: el.type!==undefined ? {name: DIRECTION_TYPE[el.type], id: el.type} : {name: DIRECTION_TYPE[0], id: 1}})}
                onSelect={() => { setSelectedDirection({ name: `${el.name} (${el.type!==undefined ? DIRECTION_TYPE[el.type] : DIRECTION_TYPE[0]})`, id: el.id }); setIsOpensDirections(false) }}
                onDelete={() => { setConfirmDeleteDirectionId(el.id) }}
              />
            ))}
          </div>
          <Button onClick={()=>setNewDirection({})} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
        </>
      </SettingsList>
      {allSubjects && 
        <SettingsList isSelected={selectedSubject !== undefined} changeIsOpen={setIsOpenSubjects} isOpenList={isOpenSubjects} title={selectedSubject ? selectedSubject.name : 'Предметы'}>
          <>
            <SearchInput searchValue={searchSubject} changeSearchValue={setSearchSubject}/>
            <div className={styles.container__list}>
              {allSubjects.filter((el) => el.name.toLowerCase().includes(searchSubject.toLowerCase())).map((el, index) => (
                <HiddenInputBlock
                  isEdit={el.isEdit}
                  isWarning={el.isWarning}
                  key={`${el.id}--${index}`}
                  value={el.name}
                  onEdit={() => changeIsEditSubject(index)}
                  onSelect={() => { setSelectedSubject({ name: el.name, id: el.id }); setIsOpenSubjects(false) }}
                  onDelete={() => { setConfirmDeleteSubjectId(el.id) }}
                  onEnter={(val) => { handleEditSubject(el.id, val) }}
                />
              ))}
            </div>
            <Button onClick={handleCreateSubject} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
          </>
        </SettingsList>
      }
      {allThemes &&
        <SettingsList isSelected={selectedTheme !== undefined} changeIsOpen={setIsOpenThemes} isOpenList={isOpenThemes} title={selectedTheme ? selectedTheme.name.toString() : 'Темы'}>
          <>
            <SearchInput searchValue={searchTheme} changeSearchValue={setSearchTheme}/>
            {allThemes.filter((el)=>`Тема ${el.number}`.toLowerCase().includes(searchTheme.toLowerCase())).map((el, index) => (
              <HiddenInputBlock
                isEdit={el.isEdit}
                isWarning={el.isWarning}
                key={`${el.id}--${index}`}
                value={`Тема ${el.number}`}
                onEdit={() => setEditTheme(el)}
                onSelect={() => { setSelectedTheme({ name: `Тема ${el.number}`, id: el.id });setIsOpenThemes(false) }}
                onDelete={() => { setConfirmDeleteThemeId(el.id) }}
              />
            ))}
            <Button onClick={()=>setNewTheme({number: undefined})} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey'/></Button>
          </>
        </SettingsList>
      }
      {allLessons && 
        <SettingsList changeIsOpen={setIsOpenLessons} isOpenList={isOpenLessons} title={'Занятия'}>
          <>
            <SearchInput searchValue={searchLesson} changeSearchValue={setSearchLesson}/>
            {allLessons.filter((el)=>`Занятие ${el.number}`.toLowerCase().includes(searchLesson.toLowerCase())).map((el, index) => (
              <HiddenInputBlock
                isEdit={el.isEdit}
                isWarning={el.isWarning}
                key={`${el.id}--${index}`}
                value={`Занятие ${el.number} (${LESSON_TYPE[el.type].name})`}
                onEdit={() => {setEditLesson(
                  {...el, 
                    type: {name: LESSON_TYPE[el.type].name, id: el.type}, 
                    semester: {name: el.semester, id: el.semester}})}}
                onDelete={() => { setConfirmDeleteLessonId(el.id) }}
              />
            ))}
            <Button onClick={()=>setNewLesson({number: undefined, type: undefined})} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey'/></Button>
          </>
        </SettingsList>
      }
      {confirmDeleteDirectionId &&
        <DeletePopup
          title='Удаление направления'
          text='Вы уверены, что хотите удалить направление?'
          onCancel={() => setConfirmDeleteDirectionId(undefined)}
          onDelete={() => handleDeleteDirection(confirmDeleteDirectionId)}
        />
      }
      {confirmDeleteSubjectId &&
        <DeletePopup
          title='Удаление предмета'
          text='Вы уверены, что хотите удалить предмет?'
          onCancel={() => setConfirmDeleteSubjectId(undefined)}
          onDelete={() => handleDeleteSubject(confirmDeleteSubjectId)}
        />
      }
      {confirmDeleteThemeId &&
        <DeletePopup
          title='Удаление темы'
          text='Вы уверены, что хотите удалить тему?'
          onCancel={() => setConfirmDeleteThemeId(undefined)}
          onDelete={() => handleDeleteTheme(confirmDeleteThemeId)}
        />
      }
      {confirmDeleteLessonId &&
        <DeletePopup
          title='Удаление занятия'
          text='Вы уверены, что хотите удалить занятие?'
          onCancel={() => setConfirmDeleteLessonId(undefined)}
          onDelete={() => handleDeleteLesson(confirmDeleteLessonId)}
        />
      }
      {editDirection && 
        <PopupContainer onClose={()=>{setEditDirection(undefined);setCheckDirection(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Редактирование направления</h2>
            <div className={styles.popup__block}>
              <p>Название направления</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkDirection} value={editDirection.name} placeholder='Введите название направления' onChange={(val) => setEditDirection({ ...editDirection, name: val })} />
            </div>
            <div className={styles.popup__block}>
              <p>Тип направления</p>
              <AddInput
                singleMode
                title='Выберите тип'
                allList={DIRECTION_TYPE.map((el, index)=>({name: el, id: index}))}
                selectedList={editDirection.type!==undefined ? [editDirection.type] : []}
                changeInputList={(newList)=>setEditDirection({...editDirection, type: newList[0]})}
              />
            </div>          
            <Button onClick={()=>handleCreateDirection(false, editDirection.name, Number(editDirection.type.id), editDirection.id)}>Сохранить</Button>
          </div>
        </PopupContainer>
      }
      {newDirection && 
        <PopupContainer onClose={()=>{setNewDirection(undefined);setCheckDirection(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Создание направления</h2>
            <div className={styles.popup__block}>
              <p>Название направления</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkDirection} value={newDirection.name ? newDirection.name : ''} placeholder='Введите название направления' onChange={(val) => setNewDirection({ ...newDirection, name: val })} />
            </div>
            <div className={styles.popup__block}>
              <p>Тип направления</p>
              <AddInput
                singleMode
                title='Выберите тип'
                allList={DIRECTION_TYPE.map((el, index)=>({name: el, id: index}))}
                selectedList={newDirection.type!==undefined ? [newDirection.type] : []}
                changeInputList={(newList)=>setNewDirection({...newDirection, type: newList[0]})}
              />
            </div>          
            <Button onClick={()=>handleCreateDirection(
              true, 
              newDirection.name, 
              newDirection.type ? Number(newDirection.type.id) : undefined, 
              undefined)
            }>Создать направление</Button>
          </div>
        </PopupContainer>
      }
      {editTheme && 
        <PopupContainer onClose={()=>{setEditTheme(undefined);setCheckTheme(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Редактирование темы</h2>
            <div className={styles.popup__block}>
              <p>Номер темы</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkTheme} value={editTheme.number.toString()} placeholder='Введите номер темы' onChange={(val) => setEditTheme({ ...editTheme, number: Number(val) })} />
            </div>
            <Button onClick={()=>handleCreateTheme(false, editTheme.number, editTheme.id)}>Сохранить</Button>
          </div>
        </PopupContainer>
      }
      {newTheme && 
        <PopupContainer onClose={()=>{setNewTheme(undefined);setCheckTheme(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Создание темы</h2>
            <div className={styles.popup__block}>
              <p>Номер темы</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkTheme} value={newTheme.number ? newTheme.number.toString() : ''} placeholder='Введите номер темы' onChange={(val) => setNewTheme({ ...newTheme, number: Number(val) })} />
            </div>
            <Button onClick={()=>handleCreateTheme(true, newTheme.number, undefined)}>Создать тему</Button>
          </div>
        </PopupContainer>
      }
      {editLesson &&
        <PopupContainer onClose={()=>{setEditLesson(undefined);setCheckLesson(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Редактирование занятия</h2>
            <div className={styles.popup__block}>
              <p>Номер занятия</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkLesson} value={editLesson.number.toString()} placeholder='Введите номер занятия' onChange={(val) => setEditLesson({ ...editLesson, number: Number(val) })} />
            </div>
            <div className={styles.popup__block}>
              <p>Количество часов</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkLesson} value={editLesson.hoursCount ? editLesson.hoursCount.toString() : ''} placeholder='Введите количество часов' onChange={(val) => setEditLesson({ ...editLesson, hoursCount: Number(val) })} />
            </div>            
            <div className={styles.popup__block}>
              <p>Тип занятия</p>
              <AddInput
                isError={checkLesson}
                minWidth={340}
                title={'Выберите тип занятия'}
                singleMode
                allList={LESSON_TYPE.map((el, index) => ({ name: el.name, id: index }))}
                selectedList={[{ name: editLesson.type.name, id: editLesson.type.id }]}
                changeInputList={(list) => setEditLesson({ ...editLesson, type: list[0] })}
              />
            </div>
            <div className={styles.popup__block}>
              <p>Семестр занятия</p>
              <AddInput
                isError={checkLesson}
                minWidth={340}
                title={'Выберите семестр занятия'}
                singleMode
                allList={SEMESTER_COUNT.map((el) => ({ name: el, id: el }))}
                selectedList={[{ name: editLesson.semester.name, id: editLesson.semester.id }]}
                changeInputList={(list) => setEditLesson({ ...editLesson, semester: list[0] })}
              />
            </div>
            <Button onClick={()=>handleCreateLesson(false, editLesson.number, Number(editLesson.type.id), Number(editLesson.semester.id), editLesson.hoursCount, editLesson.id)}>Сохранить</Button>
          </div>
        </PopupContainer>
      }
      {newLesson &&
        <PopupContainer onClose={()=>{setNewLesson(undefined);setCheckLesson(false)}} displayClose>
          <div className={styles.popup}>
            <h2>Создание занятия</h2>
            <div className={styles.popup__block}>
              <p>Номер занятия</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkLesson} value={newLesson.number!== undefined ? newLesson.number.toString() : ''} placeholder='Введите номер занятия' onChange={(val) => setNewLesson({ ...newLesson, number: Number(val) })} />
            </div>
            <div className={styles.popup__block}>
              <p>Количество часов</p>
              <Input errorText=' ' validateChecker={(number)=>{return !number || Number(number)!==0}} isError={checkLesson} value={newLesson.hoursCount!== undefined ? newLesson.hoursCount.toString() : ''} placeholder='Введите количество часов' onChange={(val) => setNewLesson({ ...newLesson, hoursCount: Number(val) })} />
            </div>
            <div className={styles.popup__block}>
              <p>Тип занятия</p>
              <AddInput
                isError={checkLesson}
                minWidth={340}
                title={'Выберите тип занятия'}
                singleMode
                allList={LESSON_TYPE.map((el, index) => ({ name: el.name, id: index }))}
                selectedList={newLesson.type ? [{ name: newLesson.type.name, id: newLesson.type.id }] : []}
                changeInputList={(list) => setNewLesson({ ...newLesson, type: list[0] })}
              />
            </div>
            <div className={styles.popup__block}>
              <p>Семестр занятия</p>
              <AddInput
                isError={checkLesson}
                minWidth={340}
                title={'Выберите семестр занятия'}
                singleMode
                allList={SEMESTER_COUNT.map((el) => ({ name: el, id: el }))}
                selectedList={newLesson.semester ? [{ name: newLesson.semester.name, id: newLesson.semester.id }] : []}
                changeInputList={(list) => setNewLesson({ ...newLesson, semester: list[0] })}
              />
            </div>
            <Button onClick={()=>handleCreateLesson(
              true, 
              newLesson.number, 
              newLesson.type ? Number(newLesson.type.id) : undefined, 
              newLesson.semester ? Number(newLesson.semester.id) : undefined, 
              newLesson.hoursCount,
              undefined
            )}>Создать занятие</Button>
          </div>
        </PopupContainer>
      }
    </>
  )
}