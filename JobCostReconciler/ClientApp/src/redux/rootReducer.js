import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";

import auth from "./reducers/Auth";
import messages from "./reducers/Messages";

export default history =>
  combineReducers({
    auth,
    messages,
    router: connectRouter(history)
  });
