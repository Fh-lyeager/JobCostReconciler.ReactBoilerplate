import { LOCATION_CHANGE } from "connected-react-router";
import config from "../../config";
import actionTypes from "../actionTypes";

export const initialState = {
  error: null,
  info: null
};

export default (state = initialState, action) => {
  // Add from config.js
  if (action.type in config.errors) {
    return Object.assign({}, state, {
      error: {
        message: config.errors[action.type]
      }
    });
  } else if (action.type in config.info) {
    return Object.assign({}, state, {
      info: {
        message: config.info[action.type]
      }
    });
  }

  switch (action.type) {
    case LOCATION_CHANGE:
    case actionTypes.app.OAUTH_SUCCESS:
      return initialState;
    case actionTypes.app.ERROR_CLEAR:
      return Object.assign({}, state, {
        error: null
      });
    case actionTypes.app.INFO_CLEAR:
      return Object.assign({}, state, {
        info: null
      });
    case actionTypes.app.ERROR_SET:
      return Object.assign({}, state, {
        error: {
          hint: action.hint,
          message: action.message
        }
      });
    case actionTypes.app.INFO_SET:
      return Object.assign({}, state, {
        info: {
          hint: action.hint,
          message: action.message
        }
      });

    default:
      return state;
  }
};
