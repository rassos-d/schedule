import { Colors } from './types';

export interface IconProps extends React.HTMLAttributes<HTMLSpanElement> {
  glyph: glyphs;
  size?: number;
  glyphColor?: IconColor;
  containerStyle?: string
}

export type glyphs = 
  'add' | 
  'admin' |
  'arrow-right' |
  'arrow-left' |
  'arrow-down' |
  'arrow-up' |
  'download' |
  'dropdown' |
  'edit' |
  'export' |
  'filter' |
  'institute' |
  'link' |
  'module' |
  'people' |
  'privilege' |
  'profile' |
  'rating' |
  'review' |
  'search' |
  'settings' |
  'sort' |
  'subject' |
  'schedule' |
  'teacher' |
  'time' |
  'event' |
  'close' |
  'check' |
  'trash';

type IconColor = keyof typeof Colors;
