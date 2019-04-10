import { LOCATION_CHANGE } from "connected-react-router";
import { decodeToken } from "../../helpers/Token";
import actionTypes from "../actionTypes";

export const initialState = {
  access: {
    scopes: []
  },
  expiresAt: null,
  isLoggedIn: false,
  timeoutAt: null,
  token: null,
  user: {
    auth_driver: null,
    guid: null,
    id: null,
    name: null,
    username: null
  }
};

export const actionsToResetAuth = [
  actionTypes.app.errors.API_FAILURE,
  actionTypes.app.errors.API_INVALID_ACCESS_TOKEN,
  actionTypes.app.LOGOUT,
  actionTypes.app.OAUTH_NOT_AUTHENTICATED,
  actionTypes.app.TIMED_OUT
];

export default (state = initialState, action) => {
  if (actionsToResetAuth.includes(action.type)) {
    return initialState;
  }

  switch (action.type) {
    case actionTypes.app.TIMED_OUT_WARNING:
      return Object.assign({}, state, {
        timeoutAt: action.timeoutAt
      });

    case LOCATION_CHANGE:
    case actionTypes.app.TIMEOUT_START:
      return Object.assign({}, state, {
        timeoutAt: null
      });

    case actionTypes.app.OAUTH_SUCCESS:
      return Object.assign({}, state, unpackAuth(action));

    case actionTypes.app.errors.OAUTH_IFRAME_FAILURE:
      return iframeFailure(state, action);
    default:
      return state;
  }
};

/**
 * This will take the oauth response and setup the auth state
 *
 * @param action
 * @returns object
 */
const unpackAuth = action => {
  const expiresIn = action.payload.expires_in
    ? parseInt(action.payload.expires_in, 10)
    : NaN;
  const expiresAt = !isNaN(expiresIn)
    ? new Date().getTime() + expiresIn * 1000
    : null;

  const user = decodeToken(action.payload.access_token);
  const access = decodeToken(action.payload.access_token);

  return {
    access,
    expiresAt: expiresAt,
    isLoggedIn: true,
    token: action.payload.access_token,
    user: Object.assign({}, user, {
      id: user.sub
    })
  };
};

const iframeFailure = (state, action) => {
  const payload403 = action.payload && action.payload.status === 403;
  const error18 = action.error && action.error.code === 18;

  if (payload403 || error18) {
    return initialState;
  } else {
    return state;
  }
};
