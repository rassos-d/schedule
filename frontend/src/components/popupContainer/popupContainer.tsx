import { useEffect, useRef, useState } from 'react'
import styles from './popupContainer.module.scss'
import useOutsideClick from '../../hooks/useClickOutside'

type PopupContainerProps = {
  children: JSX.Element
  onClose?: () => void
  isActive?:boolean
}

export default function PopupContainer ({children, onClose, isActive}:PopupContainerProps) {
  const popupRef = useRef<HTMLDivElement>(null)
  const [isMounted, setIsMounted] = useState(false);

  const getIsActive = () => {
    if (isMounted) {
      return isActive
    }
    return false
  }
  useEffect(() => {
    window.scrollX = 0
    const scrollY = window.scrollY;
    document.body.style.position = 'fixed';
    document.body.style.top = `-${scrollY}px`;
    document.body.style.width = '100%';
    setIsMounted(true);
    return () => {
      const scrollY = parseInt(document.body.style.top || '0');
      document.body.style.position = '';
      document.body.style.top = '';
      window.scrollTo(0, -scrollY);
      setIsMounted(false)
    };
  }, []);

  useOutsideClick(popupRef, () => onClose ? onClose() : undefined, ['button', 'a[href]', 'p', '#datepicker', '.react-datepicker-popper'], getIsActive())


  return (
    <div className={styles.container}>
      <div ref={popupRef} className={styles.container__content}>
        {children}
      </div>
    </div>
  )
}