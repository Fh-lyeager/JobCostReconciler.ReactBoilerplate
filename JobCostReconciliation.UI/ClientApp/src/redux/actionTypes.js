export default {
  /**
   * Add your application action types here
   */

  app: {
    API_FETCH: "API_FETCH",
    API_SUCCESS: "API_SUCCESS",

    CSRF_TOKEN: "CSRF_TOKEN",

    ERROR_CLEAR: "ERROR_CLEAR",
    ERROR_SET: "ERROR_SET",

    LOGIN_ATTEMPT: "LOGIN_ATTEMPT",
    LOGOUT: "LOGOUT",

    INFO_CLEAR: "INFO_CLEAR",
    INFO_SET: "INFO_SET",

    OAUTH_SUCCESS: "OAUTH_SUCCESS",
    OAUTH_REFRESH_ACCESS_TOKEN: "OAUTH_REFRESH_ACCESS_TOKEN",

    TIMED_OUT: "TIMED_OUT",
    TIMED_OUT_WARNING: "TIMED_OUT_WARNING",
    TIMEOUT_START: "TIMEOUT_START",

    errors: {
      API_FAILURE: "API_FAILURE",
      API_INVALID_ACCESS_TOKEN: "API_INVALID_ACCESS_TOKEN",

      CSRF_TOKEN_MISMATCH: "CSRF_TOKEN_MISMATCH",

      OAUTH_IFRAME_FAILURE: "OAUTH_IFRAME_FAILURE",
      OAUTH_IFRAME_INVALID_STATE: "OAUTH_IFRAME_INVALID_STATE",
      OAUTH_NOT_AUTHENTICATED: "OAUTH_NOT_AUTHENTICATED"
    }
  }
};
