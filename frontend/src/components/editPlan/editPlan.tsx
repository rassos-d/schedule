import { Direction } from '../../types/directions'
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
import { LESSON_TYPE } from '../../consts'

const NEW_DIRECTION_NAME = 'Новое направление'
const NEW_SUBJECT_NAME = 'Новый предмет'

export function EditPlan() {

  const [isOpenDirections, setIsOpensDirections] = useState<boolean>(false)
  const [allDirections, setAllDirections] = useState<(Direction & { isEdit: boolean, isWarning: boolean })[]>()
  const [confirmDeleteDirectionId, setConfirmDeleteDirectionId] = useState<string>()
  const [selectedDirection, setSelectedDirection] = useState<{name: string, id: string}>()
  const [searchDirection, setSearchDirection] = useState('')

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
  const handleEditDirection = async (id: string, name: string) => {
    await axios.put(PagesURl.DIRECTION, {
      id,
      name
    })
    handleGetAllDirections()
  }
  const handleDeleteDirection = async (id: string) => {
    await axios.delete(PagesURl.DIRECTION + `/${id}`)
    setConfirmDeleteDirectionId(undefined)
    handleGetAllDirections()
  }
  const handleCreateDirection = async () => {
    await axios.post(PagesURl.DIRECTION, {
      name: NEW_DIRECTION_NAME
    })
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
  const handleCreateLesson = async (isNew: boolean, number: number | undefined, type: number | undefined, semester: number | undefined, id: string | undefined) => {
    if (number === undefined  || !number || type === undefined || semester === undefined) {
      setCheckLesson(true)
      return
    }
    setCheckLesson(false)
    await axios[isNew ? 'post' : 'put'](PagesURl.LESSON, {
      id,
      number,
      type,
      semester,
      themeId: selectedTheme?.id,
      subjectId: selectedSubject?.id
    })
    setNewLesson(undefined)
    setEditLesson(undefined)
    handleGetAllLessons(selectedTheme?.id)
  }

  const changeIsEditDirection = (index: number) => {
    if (!allDirections) return
    const newDirections = [...allDirections]
    const targetDirection = newDirections[index]
    let editFlag = false
    const result = newDirections.map((el) => {
      if (el.isEdit && el.id !== targetDirection.id) {
        editFlag = true
        return { ...el, isWarning: true }
      }
      return { ...el, isWarning: false }
    })
    if (!editFlag) {
      result[index].isEdit = true
    }
    setAllDirections(result)
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
                value={el.name}
                onEdit={() => changeIsEditDirection(index)}
                onSelect={() => { setSelectedDirection({ name: el.name, id: el.id }); setIsOpensDirections(false) }}
                onDelete={() => { setConfirmDeleteDirectionId(el.id) }}
                onEnter={(val) => { handleEditDirection(el.id, val) }}
              />
            ))}
          </div>
          <Button onClick={handleCreateDirection} size={'max'} variant={'whiteMain'}><Icon glyph='add' glyphColor='grey' /></Button>
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
                value={`Занятие ${el.number}`}
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
      {editTheme && 
        <PopupContainer onClose={()=>setEditTheme(undefined)} displayClose>
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
                allList={[1, 2, 3, 4, 5].map((el) => ({ name: el, id: el }))}
                selectedList={[{ name: editLesson.semester.name, id: editLesson.semester.id }]}
                changeInputList={(list) => setEditLesson({ ...editLesson, semester: list[0] })}
              />
            </div>
            <Button onClick={()=>handleCreateLesson(false, editLesson.number, Number(editLesson.type.id), Number(editLesson.semester.id), editLesson.id)}>Сохранить</Button>
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
                allList={[1, 2, 3, 4, 5].map((el) => ({ name: el, id: el }))}
                selectedList={newLesson.semester ? [{ name: newLesson.semester.name, id: newLesson.semester.id }] : []}
                changeInputList={(list) => setNewLesson({ ...newLesson, semester: list[0] })}
              />
            </div>
            <Button onClick={()=>handleCreateLesson(
              true, 
              newLesson.number, 
              newLesson.type ? Number(newLesson.type.id) : undefined, 
              newLesson.semester ? Number(newLesson.semester.id) : undefined, 
              undefined
            )}>Создать занятие</Button>
          </div>
        </PopupContainer>
      }
    </>
  )
}