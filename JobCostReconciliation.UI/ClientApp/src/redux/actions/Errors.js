import actionTypes from "../actionTypes";

export const setError = (message, hint = null) => ({
  type: actionTypes.app.ERROR_SET,
  message,
  hint
});

export const clearError = () => ({
  type: actionTypes.app.ERROR_CLEAR
});

export const clearInfo = () => ({
  type: actionTypes.app.INFO_CLEAR
});
