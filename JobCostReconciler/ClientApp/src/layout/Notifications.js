import React from "react";
import { connect } from "react-redux";
import Alert from "../components/Alert/Alert";
import TimeoutWarning from "./TimeoutWarning";
import { clearError, clearInfo } from "../redux/actions/Errors";

const Notifications = ({ clearError, clearInfo, messages }) => [
  <Alert
    className="alert-danger"
    key={1}
    message={messages.error}
    onClose={clearError}
  />,
  <Alert
    className="alert-info"
    key={2}
    message={messages.info}
    onClose={clearInfo}
  />,
  <TimeoutWarning key={3} />
];

const mapStateToProps = ({ messages }) => ({ messages });

const mapDispatchToProps = dispatch => {
  return {
    clearError: () => {
      dispatch(clearError());
    },
    clearInfo: () => {
      dispatch(clearInfo());
    }
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Notifications);
