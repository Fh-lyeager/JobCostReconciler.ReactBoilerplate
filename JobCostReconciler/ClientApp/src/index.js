import React from "react";
import ReactDOM from "react-dom";
import { createBrowserHistory } from "history";

//import App from './App';
import App from "./layout/App";
import configureStore from "./redux/configureStore";

//const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
//const rootElement = document.getElementById('root');

// Use browser history in this application
const browserHistory = createBrowserHistory();

// Create store with reducers and middleware
const store = configureStore(browserHistory);

ReactDOM.render(
    <App history={browserHistory} store={store} />,
    document.getElementById("root")
);
