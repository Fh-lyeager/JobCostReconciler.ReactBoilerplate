import React from "react";
import { userCan } from "../../helpers/User";
import { connect } from "react-redux";

/**
 * Private component will display based on permissions
 * - As represent component to load under allowed permissions for user
 * - Or is fallback component if failed access
 * - permissions[] are string permissions to check for user
 *
 * @param props
 * @returns {*}
 * @constructor
 */
const Private = ({
  As,
  checkUser,
  Denied = null,
  dispatch,
  isLoggedIn,
  Or = null,
  permissions = [],
  ...remainingProps
}) => {
  if (checkUser(permissions)) {
    return <As {...remainingProps} />;
  }

  if (isLoggedIn) {
    return <Denied {...remainingProps} />;
  }

  if (Or) {
    return <Or {...remainingProps} />;
  }

  return null;
};

const mapStateToProps = ({ auth }) => ({
  checkUser: permissions => {
    return userCan(auth, permissions);
  },
  isLoggedIn: auth.isLoggedIn
});

export default connect(mapStateToProps)(Private);
