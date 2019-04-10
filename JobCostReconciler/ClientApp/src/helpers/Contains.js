export const containsAll = (haystack = [], needles = []) => {
  const matches = needles.filter(needle => haystack.includes(needle));
  return needles.length === matches.length;
};

export const containsAny = (haystack = [], needles = []) => {
  const matches = needles.filter(needle => haystack.includes(needle));
  return !!matches.length;
};
