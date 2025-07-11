import { useEffect, useRef, useState } from 'react'
import styles from './input.module.scss'
import { removeElementAtIndex } from '../../utils'
import { Icon } from '../icon'
import { AddInputList } from '../../types/input'

type InputProps = {
  placeholder?: string
  required?: boolean
  name?: string
  type?: 'email' | 'password' | 'search' | 'date' | 'time'
  value: string
  maxValues?: number
  isError?: boolean
  errorText?: string
  onChange: (value: string) => void
  validateChecker?: (value: string) => boolean
}

export function Input ({placeholder, value, required, type, maxValues, isError, onChange, name, validateChecker, errorText}:InputProps) {

  const textarea = useRef<HTMLTextAreaElement>(null)
  const [rows, setRows] = useState(1);

  const changeTextarea = (value: string) => {
    onChange(value)
    if (textarea.current) {
      const cursorIndex = textarea.current.selectionStart;
      const textBeforeCursor = textarea.current.value.substring(0, cursorIndex);
      const lines = textBeforeCursor.split('\n');
      const line = lines.length - 1;
      setRows(line + 1);
    }
  }

  const isValid = () => {
    if (isError !== true) return true
    return validateChecker ? validateChecker(value) : value !== ''
  }

  const changeValue = (value: string) => {
    onChange(value)
  }

  useEffect(() => { 
    if (textarea.current) {
      const lineHeight = parseInt(window.getComputedStyle(textarea.current).lineHeight, 10);
      const newRows = Math.ceil(textarea.current.scrollHeight / lineHeight) - 1;
      setRows(newRows);
    }
  }, [textarea]);

  if (type === 'date') {
    return <input type="date" value={value} className={`${styles.input} ${isError ? styles.input_error : ''}`} onChange={(e)=>{changeValue(e.target.value)}}/>
  }
  if (type === 'time') {
    return  <input
    type='time'
    min="08:00" 
    max="19:00"
    name={name}
    maxLength={maxValues} required={required} 
    className={`${styles.input} ${isError ? styles.input_error : ''}`} 
    placeholder={placeholder} value={value} 
    onChange={(e)=>{changeValue(e.target.value)}}
  />
  }
  return (
    <div className={`${styles.inputBlock} ${!isValid() ? styles.inputBlock_error : ''}`}>
      {type ? 
        <input
          name={name}
          maxLength={maxValues} type={type} required={required} 
          className={`${styles.input} ${isError ? styles.input_error : ''}`} 
          placeholder={placeholder} value={value} 
          onChange={(e)=>{changeValue(e.target.value)}}
        /> :
    <textarea name={name} required={required} maxLength={maxValues} ref={textarea} rows={rows} className={`${styles.input} ${!isValid() ? styles.input_error : ''}`}  placeholder={placeholder} value={value} onChange={(e)=>{changeTextarea(e.target.value)}}/>}
    {type === 'search' && value !== '' && <img onClick={()=>{onChange('')}} className={styles.input__clear} src='/icons/close.svg'/>}
    {!isValid() && <p className={styles.errorText}>{errorText ? errorText : 'Это поле обязательное для заполнения'}</p>}
    </div>
  )
}

type AddInputProps = {
  changeInputList: (newList: AddInputList[]) => void
  onSeeMore?: (searchValue?:string) => void
  onSearch?: (searchValue?:string) => void
  selectedList: AddInputList[]
  allList: AddInputList[]
  title: string
  placeholder: string
  singleMode?:boolean
  displaySelected?: boolean
  totalParts: number
  currentPart: number
  isError?: boolean
}


export function AddInput({ selectedList, changeInputList, allList, title, placeholder, singleMode, totalParts, currentPart, onSeeMore, onSearch, isError }: AddInputProps) {

  const [searchValue, setSearchValue] = useState('')
  const [displayList, setDisplayList] = useState(false)

  const changeSearchValue = (newvalue: string) => {
    setSearchValue(newvalue)
    if (onSearch){
      onSearch(newvalue!=='' ? newvalue : undefined)
    }
  }


  const changeList = (value: AddInputList) => {
    let newList = selectedList.slice()
    const valueIndex = selectedList.findIndex((item)=>item.id === value.id)
    if (valueIndex !== -1) {
      newList = removeElementAtIndex(newList, valueIndex)
      changeInputList(newList)
      return
    }
    if (singleMode) {
      newList = [value]
      setDisplayList(false)
    } else {
      newList.push(value)
    }
    changeInputList(newList)
  }

  const seeMore = () => {
    if (!onSeeMore){
      return
    }
    onSeeMore(searchValue!=='' ? searchValue : undefined)
  }

  const getTitleColor = () => {
    if (displayList) {
      return 'white'
    }
    if (singleMode && selectedList[0]) {
      return 'white'
    }
    return 'grey'
  }

  const isValid = () => {
    if (!isError) return true
    return selectedList.length !== 0
  }

  return (
    <div className={`${styles.list}`}>
      <div onClick={() => { setDisplayList(!displayList) }} className={`${styles.list__title} ${getTitleColor() === 'white' ? styles.list__title_active : ''} ${!isValid() ? styles.list__title_error : ''}`}>
        <p>{selectedList[0] && singleMode ? selectedList[0].name : title}</p>
        <Icon glyph={`arrow-${displayList ? 'up' : 'down'}`} glyphColor={getTitleColor()} />
      </div>
      {displayList && <div className={styles.list__list}>
        {onSearch && 
        <div className={styles.list__line}>
          <Icon glyph='search' glyphColor='grey' />
          <input value={searchValue} onChange={(e) => { changeSearchValue(e.target.value) }} placeholder={`${placeholder}...`} className={styles.list__search} />
          {searchValue!=='' && <img onClick={()=>{changeSearchValue('')}} className={styles.input__clear} src='/icons/close.svg'/>}
        </div>}
        {allList.map((el) => (
          <p key={el.id} onClick={() => { changeList(el) }} className={styles.list__line}>
            <img src={`/icons/${singleMode ? 'radioButton': 'checkbox'}/${selectedList.findIndex((item)=>item.id === el.id) !== -1 ? 'active' : 'disable'}.svg`} />
            <p>{el.name}</p>
          </p>
        ))}
        {totalParts !== currentPart && onSeeMore && allList.length!==0 && <p onClick={seeMore} className={styles.list__showMore}>Показать еще...</p>}
      </div>}
    </div>
  )
}