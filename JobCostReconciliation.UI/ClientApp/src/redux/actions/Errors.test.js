import { clearError, clearInfo, setError } from "./Errors";
import actionTypes from "../actionTypes";

describe("actions", () => {
  it("should create an action to display an error", () => {
    expect(setError("New error")).toEqual({
      type: actionTypes.app.ERROR_SET,
      message: "New error",
      hint: null
    });
  });

  it("should create an action to clear an error", () => {
    expect(clearError()).toEqual({
      type: actionTypes.app.ERROR_CLEAR
    });
  });

  it("should create an action to clear a message", () => {
    expect(clearInfo()).toEqual({
      type: actionTypes.app.INFO_CLEAR
    });
  });
});
