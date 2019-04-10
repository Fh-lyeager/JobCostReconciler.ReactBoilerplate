import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import { createBrowserHistory } from "history";
import configureStore from "../redux/configureStore";

it("renders without crashing", () => {
  const div = document.createElement("div");
  const history = createBrowserHistory();
  const store = configureStore(history);

  ReactDOM.render(<App history={history} store={store} />, div);
});
