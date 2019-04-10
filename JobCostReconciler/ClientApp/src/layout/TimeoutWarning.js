import React from "react";
import { connect } from "react-redux";
import moment from "moment";
import actionTypes from "../redux/actionTypes";
import Modal from "../components/Modal/Modal";
import { logout } from "../redux/actions/Authentication";

/**
 * Display warning message when user is about to be timed out
 */
class TimeoutWarning extends React.Component {
  state = {
    expiresIn: null,
    expiresInInterval: null
  };

  componentWillMount() {
    this.setState({
      expiresInInterval: setInterval(() => {
        this.setState({
          expiresIn: this.props.getLoggedOutIn()
        });
      }, 1000)
    });
  }

  componentWillUnmount() {
    clearInterval(this.state.expiresInInterval);
  }

  render() {
    if (this.props.show()) {
      return (
        <Modal
          close="Refresh"
          onClose={this.props.refresh}
          onSubmit={this.props.softLogout}
          submit="Logout"
          title="Your session is about to end."
        >
          <p>
            You will be logged out in {this.props.getLoggedOutIn() + " "}
            at {this.props.getTimeOfTimeout()}
          </p>
        </Modal>
      );
    }

    return null;
  }
}

const getTimeoutIn = auth => {
  if (auth.timeoutAt) {
    const timeout = auth.timeoutAt;
    const now = new Date().getTime();

    if (timeout > now) {
      return timeout - now;
    }
  }

  return 0;
};

const mapStateToProps = ({ auth }) => ({
  getTimeOfTimeout: () => {
    if (auth.timeoutAt) {
      return moment(auth.timeoutAt).format("h:mm:ss a");
    }

    return null;
  },
  getLoggedOutIn: () => {
    const timeoutIn = getTimeoutIn(auth);

    if (timeoutIn > 5000) {
      return moment.duration(timeoutIn).humanize();
    }

    return moment.duration(timeoutIn).seconds() + 1;
  },
  show: () => auth.timeoutAt && moment().isBefore(auth.timeoutAt)
});

const mapDispatchToProps = dispatch => {
  return {
    refresh: () => {
      dispatch({ type: actionTypes.app.OAUTH_REFRESH_ACCESS_TOKEN });
    },
    softLogout: () => {
      dispatch(logout());
    }
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TimeoutWarning);
