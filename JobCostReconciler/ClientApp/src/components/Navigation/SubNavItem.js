import React from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PrivateNavLink from "./PrivateNavLink";

export default ({ link, ...remainingProps }) => {
  if (link.hasOwnProperty("icon")) {
    if (link.private === true) {
      return (
        <li {...remainingProps}>
          <PrivateNavLink exact to={link.href}>
            <FontAwesomeIcon icon={link.icon} /> {" " + link.name}
          </PrivateNavLink>
        </li>
      );
    } else {
      return (
        <li {...remainingProps}>
          <NavLink exact to={link.href}>
            <FontAwesomeIcon icon={link.icon} /> {" " + link.name}
          </NavLink>
        </li>
      );
    }
  } else {
    if (link.private === true) {
      return (
        <li {...remainingProps}>
          <PrivateNavLink exact to={link.href}>
            {" " + link.name}
          </PrivateNavLink>
        </li>
      );
    } else {
      return (
        <li {...remainingProps}>
          <NavLink exact to={link.href}>
            {" " + link.name}
          </NavLink>
        </li>
      );
    }
  }
};
