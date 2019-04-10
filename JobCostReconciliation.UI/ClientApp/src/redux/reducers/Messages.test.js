import Messages, { initialState } from "./Messages";
import { LOCATION_CHANGE } from "connected-react-router";
import actionTypes from "../actionTypes";

describe("reducers", () => {
  const dirtyState = Object.assign({}, initialState, {
    error: {
      message: "oh no",
      hint: null
    },
    info: {
      message: "hi",
      hint: null
    }
  });

  it("should return the initial state", () => {
    expect(Messages(undefined, {})).toEqual(initialState);
  });

  it("should reset on LOCATION_CHANGE", () => {
    expect(
      Messages(dirtyState, {
        type: LOCATION_CHANGE
      })
    ).toEqual(initialState);
  });

  it("should clear error on ERROR_CLEAR", () => {
    expect(
      Messages(dirtyState, {
        type: actionTypes.app.ERROR_CLEAR
      })
    ).toMatchObject({
      error: null
    });
  });

  it("should clear message on INFO_CLEAR", () => {
    expect(
      Messages(dirtyState, {
        type: actionTypes.app.INFO_CLEAR
      })
    ).toMatchObject({
      info: null
    });
  });

  it("should set error on ERROR_SET", () => {
    expect(
      Messages(dirtyState, {
        type: actionTypes.app.ERROR_SET,
        message: "hello",
        hint: "9"
      })
    ).toMatchObject({
      error: {
        message: "hello",
        hint: "9"
      }
    });
  });

  it("should set message on INFO_SET", () => {
    expect(
      Messages(dirtyState, {
        type: actionTypes.app.INFO_SET,
        message: "now is the time",
        hint: "hinting"
      })
    ).toMatchObject({
      info: {
        message: "now is the time",
        hint: "hinting"
      }
    });
  });
});
