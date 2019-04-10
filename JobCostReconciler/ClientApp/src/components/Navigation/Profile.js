import React from "react";
import { connect } from "react-redux";
import { push } from "connected-react-router";
import { loginAttempt, logout } from "../../redux/actions/Authentication";

class Profile extends React.Component {
  renderLink(onClick, text) {
    return <a onClick={onClick}>{text}</a>;
  }

  render() {
    if (this.props.isLoggedIn) {
      return this.renderLink(this.props.logout, "Log out");
    }

    return this.renderLink(this.props.login, "Log in");
  }
}

const mapStateToProps = ({ auth }) => ({
  isLoggedIn: auth.isLoggedIn,
  name: auth.user.name
});

const mapDispatchToProps = dispatch => {
  return {
    login: () => {
      dispatch(loginAttempt());
    },
    logout: e => {
      e.preventDefault();
      dispatch(logout());
      dispatch(push("/"));
    }
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Profile);
