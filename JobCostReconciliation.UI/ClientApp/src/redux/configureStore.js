import { applyMiddleware, createStore } from "redux";
import logger from "redux-logger";
import Raven from "raven-js";
import { routerMiddleware } from "connected-react-router";
import throttle from "lodash/throttle";

import config from "../config";
import rootReducer from "./rootReducer";
import OAuth from "./middleware/OAuth";
import ApiFetch from "./middleware/ApiFetch";

// Start sentry.io error reporting, will not fail on empty DSN
Raven.config(config.sentryIODSN).install();

export default history => {
  const middleware = [
    // Libraries
    routerMiddleware(history),
    // Boilerplate middleware
    OAuth,
    ApiFetch

    /**
     * Add additional middleware here
     */
  ];

  if (process.env.NODE_ENV === "development") {
    middleware.push(logger);
  }

  const store = createStore(
    rootReducer(history),
    loadState(),
    applyMiddleware(...middleware)
  );

  // Subscribe to store changes and save state
  store.subscribe(
    throttle(() => {
      saveState(store.getState());
    }, 500)
  );

  return store;
};

// Load state from local storage
const loadState = () => {
  try {
    const serializedState = localStorage.getItem(config.key);
    if (serializedState === null) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch (e) {
    return undefined;
  }
};

// Save state to local storage
const saveState = state => {
  try {
    const serializedState = JSON.stringify(state);
    localStorage.setItem(config.key, serializedState);
  } catch (e) {
    // Ignore write errors
  }
};
