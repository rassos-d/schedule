import { useEffect, useRef} from 'react'
import styles from './popupContainer.module.scss'
import { Icon } from '../icon'

type PopupContainerProps = {
  children: JSX.Element
  onClose?: () => void
  isActive?:boolean
  displayClose?: boolean
}

export default function PopupContainer ({children, onClose, displayClose}:PopupContainerProps) {
  const popupRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    window.scrollX = 0
    const scrollY = window.scrollY;
    document.body.style.position = 'fixed';
    document.body.style.top = `-${scrollY}px`;
    document.body.style.width = '100%';
    return () => {
      const scrollY = parseInt(document.body.style.top || '0');
      document.body.style.position = '';
      document.body.style.top = '';
      window.scrollTo(0, -scrollY);
    };
  }, []);

  //useOutsideClick(popupRef, () => onClose ? onClose() : undefined, ['button', 'a[href]', 'p', '#datepicker', '.react-datepicker-popper'], getIsActive())


  return (
    <div className={styles.container}>
      <div ref={popupRef} className={styles.container__content}>
        {children}
        {displayClose && <div onClick={onClose} className={styles.container__close}>
          <Icon glyph='close' glyphColor='black'/>
        </div>}
      </div>
    </div>
  )
}