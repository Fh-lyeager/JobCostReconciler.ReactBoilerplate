import React from "react";
import CloseButton from "../Button/CloseButton";
import LightButton from "../Button/LightButton";
import PrimaryButton from "../Button/PrimaryButton";

/**
 * Modal component to load a popup window
 *
 * @property close default "Close" text for close button
 * @property onClose
 * @property onSubmit
 * @property submit default "Submit" text for submit button
 * @property title title on top of modal
 */
class Modal extends React.Component {
  componentWillMount() {
    document.addEventListener("keyup", e => {
      if (e.key === "Escape") {
        this.props.onClose();
      }
    });
  }

  render() {
    const {
      title,
      children,
      close = "Close",
      onClose = () => {},
      onSubmit = () => {},
      submit = "Submit"
    } = this.props;
    return (
      <div
        className="modal"
        onClick={e => {
          if (e.target.className === "modal") {
            onClose();
          }
        }}
        role="dialog"
        style={{ display: "block" }}
        tabIndex={-1}
      >
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h3 className="modal-title">{title}</h3>
              <CloseButton onClick={onClose} />
            </div>
            <div className="modal-body">{children}</div>
            <div className="modal-footer">
              <LightButton onClick={onClose}>{close}</LightButton>
              <PrimaryButton onClick={onSubmit}>{submit}</PrimaryButton>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Modal;
