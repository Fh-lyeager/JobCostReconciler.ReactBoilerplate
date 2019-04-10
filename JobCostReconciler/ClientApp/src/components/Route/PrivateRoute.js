import React from "react";
import { Route } from "react-router";
import Private from "../Private/Private";
import NotAllowed from "../../containers/NotAllowed/NotAllowed";
import NotFound from "../../containers/NotFound/NotFound";

/**
 * Route component with Private base
 * See Private component
 *
 * @param props
 * @returns {*}
 */
export default props => {
  const {
    notAllowed = NotAllowed,
    notFound = NotFound,
    ...remainingProps
  } = props;

  return (
    <Private As={Route} Denied={notAllowed} Or={notFound} {...remainingProps} />
  );
};
