export const decodeToken = token => {
  // Second split is payload of access_token
  const payload = token.split(".")[1];
  return JSON.parse(atob(payload));
};
