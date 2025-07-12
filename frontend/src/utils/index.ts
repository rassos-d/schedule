export function removeElementAtIndex<T>(array: T[], index: number): T[] {
  return [
      ...array.slice(0, index),
      ...array.slice(index + 1)
  ];
}

export function cloneObject<T>(object: T) {
  const result:T = JSON.parse(JSON.stringify(object))
  return result
}

export function getUniqueElements<T>(arr: T[]): T[] {
    return arr.filter(item => arr.indexOf(item) === arr.lastIndexOf(item));
}

export function createNestedArray<T>(length: number) {
  return Array.from({ length: length }, () => Array.from({ length: length }, () => [] as T[]));
}

export const copyLink = async (id: string) => {
  await navigator.clipboard.writeText(window.location.hostname + `/${id}`);
}