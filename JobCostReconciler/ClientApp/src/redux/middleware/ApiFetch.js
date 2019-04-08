import { hasExpired } from "../../helpers/User";
import actionTypes from "../actionTypes";

/**
 * Start a fetch() request with auth headers through a redux action
 * - type: API_FETCH
 * - start[optional]: dispatches this first with payload
 * - payload[optional]: for start action
 * - receive[optional]: dispatches this on successful response with response payload, default API_SUCCESS
 * - failure[optional]: dispatches this on failure response with response payload or error, default API_FAILURE
 * - url: see whatwg-fetch
 * - options: see whatwg-fetch
 *
 * @param action
 */
export default ({ dispatch, getState }) => next => action => {
  if (action.type !== actionTypes.app.API_FETCH) {
    return next(action);
  }

  const { auth } = getState();

  const response = next(action);

  if (action.start) {
    dispatch(apiStart(action));
  }

  if (hasExpired(auth)) {
    dispatch(apiInvalidAccessToken());
  }

  const controller = new window.AbortController();

  const options = addToOptions(action.options, auth.token, controller.signal);

  // Cancel or abort fetch after 5 seconds
  setTimeout(() => controller.abort(), 5000);

  try {
    theFetch(action.url, options).then(response => {
      if (response.ok) {
        dispatch(apiSuccess(action.receive, response.json));
      } else {
        dispatch(apiFailure(action.failure, response, action));
      }
    });
  } catch (error) {
    dispatch(apiFailure(action.failure, error, action));
  }

  return response;
};

const theFetch = (url, options = {}) => {
  return fetch(url, options).then(response => {
    if (response.ok) {
      return response.json().then(json => ({
        json,
        ok: response.ok,
        status: response.status,
        statusText: response.statusText
      }));
    }

    return response;
  });
};

const addToOptions = (fetchOptions, token, signal) => {
  const options = Object.assign({}, fetchOptions, {});

  return Object.assign({}, options, {
    headers: Object.assign(
      {},
      {
        Accept: "application/json",
        Authorization: "Bearer " + token,
        "Content-Type": "application/json"
      },
      options.headers
    ),
    signal
  });
};

const apiSuccess = (type = actionTypes.app.API_SUCCESS, data) => ({
  payload: data,
  type
});

const apiFailure = (
  type = actionTypes.app.errors.API_FAILURE,
  response,
  action
) => ({
  action,
  response,
  type
});

const apiStart = action => ({
  type: action.start,
  payload: action.payload
});

const apiInvalidAccessToken = () => ({
  type: actionTypes.app.errors.API_INVALID_ACCESS_TOKEN
});
