import { loginAttempt, logout } from "./Authentication";
import actionTypes from "../actionTypes";
import config from "../../config";

describe("actions", () => {
  it("should start a login redirect", () => {
    expect(loginAttempt()).toEqual({
      type: actionTypes.app.LOGIN_ATTEMPT
    });
  });

  it("should start a logout apiFetch", () => {
    expect(logout()).toEqual({
      type: actionTypes.app.API_FETCH,
      receive: actionTypes.app.LOGOUT,
      url: config.api.authUrl + "/logout",
      options: {
        credentials: "include",
        method: "POST"
      }
    });
  });
});
