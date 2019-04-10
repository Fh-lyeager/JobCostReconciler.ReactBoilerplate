import React from "react";
import { Route } from "react-router";

const matchMediaFullScreen = window.matchMedia(`(min-width: 768px)`);

export default class MediaRoute extends React.Component {
  state = {
    matchMediaFullScreen: matchMediaFullScreen,
    pageTitle: window.document.title,
    sidebarDocked: false
  };

  componentWillMount() {
    matchMediaFullScreen.addListener(this.mediaQueryChanged.bind(this));
    this.setState({
      matchMediaFullScreen: matchMediaFullScreen,
      sidebarDocked: matchMediaFullScreen.matches
    });
  }

  componentWillUnmount() {
    this.state.matchMediaFullScreen.removeListener(this.mediaQueryChanged);
  }

  mediaQueryChanged() {
    this.setState({ sidebarDocked: this.state.matchMediaFullScreen.matches });
  }

  render() {
    const { path, fullScreenPath, ...remainingProps } = this.props;

    if (this.state.sidebarDocked) {
      return <Route path={fullScreenPath} {...remainingProps} />;
    }

    return <Route path={path} {...remainingProps} />;
  }
}
