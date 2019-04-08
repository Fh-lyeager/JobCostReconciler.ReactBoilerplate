import { INVALID_SCOPE } from "./redux/middleware/OAuth";
import actionTypes from "./redux/actionTypes";

export default {
  /**
   * App key, current for redux persist key
   */
  key: "redux",

  /**
   * Map actions to errors and info
   * [CONSTANT] as a key allows constants in keys (is not an array)
   */
  errors: {
    [actionTypes.app.errors.API_FAILURE]:
      "There was a problem with your login so we reset your session."
  },
  info: {
    [actionTypes.app.TIMED_OUT]: "You've been logged out because of inactivity."
  },

  /**
   * API_FETCH urls
   */
  api: {
    authUrl: process.env.REACT_APP_AUTH_API_URL
  },

  /**
   * OAuth config
   */
  oauth: {
    url: process.env.REACT_APP_FISCHER_OAUTH_AUTHORIZE_URL,
    client: process.env.REACT_APP_FISCHER_OAUTH_CLIENT_ID,
    /**
     * Permissions that will be used in the application
     */
    scope: process.env.REACT_APP_NEEDED_SCOPES
      ? process.env.REACT_APP_NEEDED_SCOPES.split(",")
      : [],
    expiration: 480000,
    /**
     * OAuth expiration buffer
     * For fetching a new token before it expires in milliseconds
     */
    expirationBuffer: 5000,
    /**
     * List of possible oauth error messages
     */
    errorsMessages: [INVALID_SCOPE]
  },

  /**
   * Session config
   */
  session: {
    /**
     * Timeout of the application in milliseconds
     * When the user doesn't do anything, it logs them out
     */
    timeout: 450000,
    /**
     * Time in milliseconds from timeout to warn the user of timing out
     */
    timeoutWarning: 40000
  },

  /**
   * @see https://sentry.io
   */
  sentryIODSN: process.env.REACT_APP_SENTRY_PUBLIC_DSN
};
