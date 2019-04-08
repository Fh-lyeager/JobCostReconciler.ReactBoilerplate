import React from "react";

export default props => {
  const { className, ...remainingProps } = props;

  return (
    <button className={"btn btn-light " + className} {...remainingProps} />
  );
};
