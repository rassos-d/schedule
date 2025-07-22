import { Button } from '../button/button'
import PopupContainer from '../popupContainer/popupContainer'
import styles from './deletePopup.module.scss'

type DeletePopupProps = {
  title: string
  text: string
  onDelete: () => void
  onCancel: () => void
}

export function DeletePopup ({title, text, onCancel, onDelete}:DeletePopupProps) {
  return (
    <PopupContainer onClose={onCancel}>
      <div className={styles.container}>
        <h1>{title}</h1>
        <p>{text}</p>
        <div className={styles.container__buttons}>
          <Button size={'max'} variant={'secondary'} onClick={onDelete}>Удалить</Button>
          <Button size={'max'} variant={'whiteMain'} onClick={onCancel}>Отмена</Button>
        </div>
      </div>
    </PopupContainer>
  )
}