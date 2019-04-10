import React from "react";
//import Swipeable from "react-swipeable";
import { connect } from "react-redux";
import { Helmet } from "react-helmet";
import { NavLink } from "react-router-dom";
import { push } from "connected-react-router";

import "./MobileHeader.css";

class MobileHeader extends React.Component {
  state = {
    pageTitle: window.document.title
  };

  /**
   * This fires VERY often from react-helmet
   * @param newState
   */
  setPageTitle(newState) {
    this.setState({
      pageTitle: newState.title
    });
  }

  redirectToSidebar() {
    this.props.push("/sidebar");
  }

  render() {
    if (this.props.router.location.pathname.startsWith("/sidebar")) {
      return null;
    }

    return (
      <div className="mobile-nav">
        <Helmet onChangeClientState={this.setPageTitle.bind(this)} />
        
        <NavLink to="/sidebar">
          <div className="burger-button">
            <span className="burger-button__bar" />
            <span className="burger-button__bar" />
            <span className="burger-button__bar" />
          </div>
        </NavLink>
        <p className="mobile-nav__title">{this.state.pageTitle}</p>
      </div>
    );
  }
}

const mapStateToProps = ({ router }) => ({ router });

const mapDispatchToProps = dispatch => ({
  push: path => {
    dispatch(push(path));
  }
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MobileHeader);
