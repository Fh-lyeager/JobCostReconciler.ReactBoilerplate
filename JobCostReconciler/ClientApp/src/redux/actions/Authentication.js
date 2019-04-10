import actionTypes from "../actionTypes";
import config from "../../config";

export const loginAttempt = () => ({
  type: actionTypes.app.LOGIN_ATTEMPT
});

export const softLogout = () => ({
  type: actionTypes.app.LOGOUT
});

export const logout = () => ({
  type: actionTypes.app.API_FETCH,
  receive: actionTypes.app.LOGOUT,
  url: config.api.authUrl + "/logout",
  options: {
    credentials: "include",
    method: "POST"
  }
});
