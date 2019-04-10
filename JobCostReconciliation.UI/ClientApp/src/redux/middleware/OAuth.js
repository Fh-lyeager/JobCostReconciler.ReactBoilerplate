import { LOCATION_CHANGE, replace } from "connected-react-router";
import { logout, softLogout } from "../actions/Authentication";
import {
  getResponseFromHash,
  getResponseFromSearch,
  hashStateEquals,
  stringify
} from "../../helpers/Location";
import { setError } from "../actions/Errors";
import { getExpiresIn, hasExpired } from "../../helpers/User";
import IFrameHandler from "../../helpers/IFrameHandler";
import actionTypes from "../actionTypes";
import config from "../../config";
import { actionsToResetAuth } from "../reducers/Auth";

const REDIRECT_STATE = "REDIRECT";
const IFRAME_STATE = "IFRAME";

export const INVALID_SCOPE = "invalid_scope";

let sessionId = null;
let sessionTimeoutId = null;
let timeoutId = null;
let timeoutWarningId = null;

/**
 * OAuth implicit grant middleware
 *
 * @returns {function(*): function(*=)}
 */
export default ({ dispatch, getState }) => next => action => {
  // Try to stop the application if loaded from iframe
  if (action.type === LOCATION_CHANGE && isLoadedFromIFrame(action.payload)) {
    return false;
  }

  if (actionsToResetAuth.includes(action.type)) {
    clearInterval(sessionId);
    clearTimeout(sessionTimeoutId);
    clearTimeout(timeoutId);
    clearTimeout(timeoutWarningId);
  }

  if (action.type === actionTypes.app.OAUTH_REFRESH_ACCESS_TOKEN) {
    startTimeout(dispatch, getState());
    refreshAccessTokenWithIFrame(dispatch).then(() => {
      startTimeout(dispatch, getState());
    });
  }

  const response = next(action);

  if (action.type === actionTypes.app.LOGIN_ATTEMPT) {
    refreshAccessTokenWithIFrame(dispatch)
      .then(() => {
        startSession(dispatch, getState());
        startTimeout(dispatch, getState());
      })
      .catch(() => {
        loginRedirectToOAuth();
      });
  }

  if (action.type === LOCATION_CHANGE) {
    handleOAuthResponse(dispatch, getState(), action);
    startSession(dispatch, getState());
    startTimeout(dispatch, getState());
  }

  return response;
};

/**
 * Start iframe request and dispatch according to response
 * @param dispatch
 */
const refreshAccessTokenWithIFrame = dispatch => {
  return new Promise((resolve, reject) => {
    // TODO can you combine promise and iframe event handler?
    new IFrameHandler({
      url: authorizeUrl(IFRAME_STATE),
      callback: eventData => {
        if (handleIFrame(dispatch, eventData.location, IFRAME_STATE)) {
          resolve(eventData.location);
        } else {
          reject();
        }
      }
    }).init();
  });
};

/**
 * Hard redirect to OAuth
 */
export const loginRedirectToOAuth = () => {
  window.location.href = authorizeUrl(REDIRECT_STATE);
};

/**
 * Start app session
 * @param dispatch
 * @param auth
 */
const startSession = (dispatch, { auth }) => {
  if (auth.isLoggedIn === false) {
    return false;
  }

  if (auth.isLoggedIn && hasExpired(auth)) {
    dispatch(softLogout());
  }

  const expiration = firstExpiration(auth);

  sessionTimeoutId = setTimeout(() => {
    refreshAccessTokenWithIFrame(dispatch);

    sessionId = setInterval(() => {
      refreshAccessTokenWithIFrame(dispatch);
    }, config.oauth.expiration - config.oauth.expirationBuffer);
  }, expiration);
};

const firstExpiration = auth =>
  getExpiresIn(auth)
    ? getExpiresIn(auth)
    : config.oauth.expiration - config.oauth.expirationBuffer;

/**
 * Start auth timeout
 * @param dispatch
 * @param auth
 */
const startTimeout = (dispatch, { auth }) => {
  if (!auth.isLoggedIn) {
    return false;
  }

  clearTimeout(timeoutId);
  clearTimeout(timeoutWarningId);

  const timeoutAt = new Date().getTime() + config.session.timeout;

  dispatch(timeoutStart());

  timeoutId = setTimeout(() => {
    dispatch(logout());
    dispatch(timedOut());
  }, config.session.timeout);

  timeoutWarningId = setTimeout(() => {
    dispatch(timedOutWarning(timeoutAt));
  }, config.session.timeout - config.session.timeoutWarning);
};

const timeoutStart = () => ({
  type: actionTypes.app.TIMEOUT_START
});

const timedOut = () => ({
  type: actionTypes.app.TIMED_OUT
});

const timedOutWarning = timeoutAt => ({
  type: actionTypes.app.TIMED_OUT_WARNING,
  timeoutAt
});

/**
 * Handle returned response from OAuth
 * @param dispatch
 * @param auth
 * @param action
 * @returns {boolean}
 */
const handleOAuthResponse = (dispatch, { auth }, action) => {
  // 1. First scan location search for errors
  if (action.payload.location.search) {
    const response = getResponseFromSearch(action.payload.location.search);

    // OAuth error should be in this standard object
    if (response.error) {
      return dispatch(setError(response.message, response.hint));
    }
  }

  // 2. Secondly, make sure the hash state is for OAuth
  if (!isLoadedFromOAuthRedirect(action.payload.location.hash)) {
    return false;
  }

  // At this point we can clean the url
  dispatch(replace(action.payload.location.pathname));

  const response = getResponseFromHash(action.payload.location.hash);

  // 3. Check if there an error from hash
  if (response.error) {
    dispatch(setError(response.error));
  } else if (hashStateEquals(action.payload.location.hash, REDIRECT_STATE)) {
    // 4. Finally, we can login and set the auth state
    dispatch(oAuthSuccess(response));
  } else {
    dispatch(tokenMismatch());
  }
};

/**
 * Generate url to oauth token request from config and state
 *
 * @param state
 * @returns {string}
 */
const authorizeUrl = state => {
  const query = stringify({
    state: state,
    response_type: "token",
    client_id: config.oauth.client,
    scope: config.oauth.scope.join(" ")
  });

  return (
    config.oauth.url +
    (config.oauth.url.indexOf("?") === -1 ? "?" : "&") +
    query
  );
};

/**
 * IFrame callback
 *
 * @param dispatch
 * @param location
 * @param token
 * @returns {boolean}
 */
const handleIFrame = (dispatch, location, token) => {
  try {
    // Parse both hash and search from location
    const hashResponse = getResponseFromHash(location.hash);
    const searchResponse = getResponseFromSearch(location.search);

    // Check the oauth response
    if (searchResponse && searchResponse.error) {
      // There could be an error from search
      dispatch(oAuthIFrameFailure(searchResponse));
      dispatch(setError(searchResponse.message, searchResponse.hint));
    } else if (hashResponse.state === token) {
      // Validate state/CSRF which means we are good to go
      dispatch(oAuthSuccess(hashResponse));
      return true;
    } else {
      // Else invalid state
      dispatch(oAuthIFrameInvalidState());
    }
  } catch (error) {
    // DOMException here means not logged into auth
    if (error instanceof DOMException) {
      dispatch(oAuthNotAuthenticated());
    } else {
      // General unknown error
      dispatch(oAuthIFrameFailure(error));
    }
  }

  return false;
};

/**
 * Check location if loaded from iframe
 * @param location
 * @returns {*|boolean}
 */
const isLoadedFromIFrame = location => {
  const search = getResponseFromSearch(location.search);

  return (
    hashStateEquals(location.hash, IFRAME_STATE) ||
    (search && config.oauth.errorsMessages.includes(search.error))
  );
};

/**
 * Check location if returned from OAuth
 * @param hash
 * @returns {*|boolean}
 */
const isLoadedFromOAuthRedirect = hash =>
  hashStateEquals(hash, REDIRECT_STATE) && hash.includes("access_token");

const oAuthSuccess = payload => ({
  type: actionTypes.app.OAUTH_SUCCESS,
  payload
});
const oAuthIFrameFailure = () => ({
  type: actionTypes.app.errors.OAUTH_IFRAME_FAILURE
});
const oAuthIFrameInvalidState = () => ({
  type: actionTypes.app.errors.OAUTH_IFRAME_INVALID_STATE
});
const oAuthNotAuthenticated = () => ({
  type: actionTypes.app.errors.OAUTH_NOT_AUTHENTICATED
});
const tokenMismatch = () => ({
  type: actionTypes.app.errors.CSRF_TOKEN_MISMATCH
});
