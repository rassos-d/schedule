import { useEffect, useRef } from 'react';

const useOutsideClick = (
  target: React.RefObject<HTMLElement> | undefined,
  handler: (event: MouseEvent) => void,
  options = ['button', 'a[href]'],
  isActive?: boolean | undefined,
) => {
  const isFirstClick = useRef(true);

  useEffect(() => {
    if (!target || !handler) return;

    const handleClickOutside = (event: MouseEvent) => {
      const element = target.current;
      if (!element || isActive === false) {
        return;
      }
      if (element.contains(event.target as Node)){
        return
      }

      const isExcluded = options.some(selector => 
        (event.target as Element)?.closest(selector)
      );

      if (isExcluded) {
        return
      }
      handler(event);
    };

    document.addEventListener('mousedown', handleClickOutside);

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      isFirstClick.current = true;
    };
  }, [target, handler, options, isActive]);
};

export default useOutsideClick;