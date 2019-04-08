import React from "react";

export default props => {
  const { className, ...remainingProps } = props;

  return (
    <button
      aria-label="Close"
      className={"close " + className}
      data-dismiss="modal"
      {...remainingProps}
    >
      <span aria-hidden="true">&times;</span>
    </button>
  );
};
