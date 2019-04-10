import qs from "qs";

export const getResponseFromHash = hash => {
  return qs.parse(getHash(hash));
};

export const getResponseFromSearch = search => {
  return qs.parse(search);
};

export const hashStateEquals = (hash, compare) => {
  if (hash) {
    const response = getResponseFromHash(hash);

    if (response.state) {
      return response.state === compare;
    }
  }

  return false;
};

export const stringify = query => {
  return qs.stringify(query);
};

const getHash = hash => {
  if (hash.charAt(0) === "#") {
    return hash.slice(1);
  }

  return hash;
};
