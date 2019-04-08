import React from "react";
import { NavLink } from "react-router-dom";
import Private from "../Private/Private";

/**
 * NavLink component with Private base
 * See Private component
 *
 * @param props
 * @returns {*}
 */
export default props => <Private As={NavLink} {...props} />;
