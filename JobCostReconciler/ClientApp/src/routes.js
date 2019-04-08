import React from "react";
import { connect } from "react-redux";
import { Route, Switch, withRouter } from "react-router";
import { CSSTransition, TransitionGroup } from "react-transition-group";
import PrivateRoute from "./components/Route/PrivateRoute";
import MediaRoute from "./components/Route/MediaRoute";

import HomePage from "./containers/HomePage/HomePage";
import NotFound from "./containers/NotFound/NotFound";
import Sidebar from "./containers/Sidebar/Sidebar";

export const SidebarRoutes = () => (
  <MediaRoute path="/sidebar" component={Sidebar} />
);

export const Routes = ({ location }) => (
  <TransitionGroup component={null}>
    <CSSTransition key={location.key} classNames="content__fade" timeout={300}>
      <ConnectedSwitch>
        {/* Home (main) route */}
        <Route exact path="/" component={HomePage} />

        {/* Routes requiring login */}
        <PrivateRoute path="/home-logged-in" component={HomePage} />

        {/* Routes */}

        {/* Render nothing on sidebar route */}
        <Route path="/sidebar" component={null} />
        {/* Catch all route */}
        <Route component={NotFound} />
      </ConnectedSwitch>
    </CSSTransition>
  </TransitionGroup>
);

export default withRouter(Routes);

// This is needed for redux
const ConnectedSwitch = connect()(Switch);
