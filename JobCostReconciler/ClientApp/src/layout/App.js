import React from "react";
import { Helmet } from "react-helmet";
import { Provider } from "react-redux";
import { ConnectedRouter } from "connected-react-router";

import "bootstrap/dist/css/bootstrap.css";
import "./IconLibrary.js";
import "./App.css";

import Routes, { SidebarRoutes } from "../routes";
import MobileHeader from "./MobileHeader";
import Notifications from "./Notifications";

/**
 * Wrapper of entire application
 *
 * @returns {*}
 */
export default ({ history, store }) => (
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <div className="app">
        <Helmet defaultTitle="Fischer Homes" />

        <header className="header">
          <MobileHeader />
          <SidebarRoutes />
        </header>

        <main className="content">
          <Routes />
        </main>

        <div className="notifications">
          <Notifications />
        </div>
      </div>
    </ConnectedRouter>
  </Provider>
);
