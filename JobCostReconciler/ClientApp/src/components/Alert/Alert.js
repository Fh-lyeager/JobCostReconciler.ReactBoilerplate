import React from "react";
import CloseButton from "../Button/CloseButton";
import "./Alert.css";

export default ({ className, message, onClose }) => {
  if (message) {
    return (
      <div className={className + " alert alert-dismissible"} role="alert">
        <p>{message.message}</p>
        <small className="alert__hint">{renderHint(message.hint)}</small>
        <CloseButton onClick={onClose} />
      </div>
    );
  }

  return null;
};

const renderHint = hint => {
  if (hint instanceof Object) {
    return (
      <ul>
        {Object.keys(hint).map((field, key) => (
          <li key={key}>{hint[field]}</li>
        ))}
      </ul>
    );
  } else {
    return hint;
  }
};
