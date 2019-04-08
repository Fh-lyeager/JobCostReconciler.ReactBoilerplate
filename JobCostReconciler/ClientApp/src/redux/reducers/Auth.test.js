import Auth, { actionsToResetAuth, initialState } from "./Auth";
import actionTypes from "../actionTypes";

describe("reducers", () => {
  const dirtyState = Object.assign({}, initialState, {
    expiresAt: 123,
    token: "dirty"
  });

  it("should return the initial state", () => {
    expect(Auth(undefined, {})).toEqual(initialState);
  });

  it("should handle oauth success", () => {
    expect(
      Auth(undefined, {
        type: actionTypes.app.OAUTH_SUCCESS,
        payload: {
          access_token:
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImFmZTMwOTg1MDFkMmIzNGQ1M2I2NmYzODM2ZTExY2Y2MTczNTgyYTQ2YjU1ZjQxMGVjMTg0ZjM0YTZmYzVjOWVlZWM3ZjAxNzk5NGM1YTJiIn0.eyJhdWQiOiIxIiwianRpIjoiYWZlMzA5ODUwMWQyYjM0ZDUzYjY2ZjM4MzZlMTFjZjYxNzM1ODJhNDZiNTVmNDEwZWMxODRmMzRhNmZjNWM5ZWVlYzdmMDE3OTk0YzVhMmIiLCJpYXQiOjE0OTk4NzQ5MzMsIm5iZiI6MTQ5OTg3NDkzMywiZXhwIjoxNTMxNDEwOTMzLCJzdWIiOiIiLCJzY29wZXMiOltdfQ.G6QnPqzxYgqNZeb7ABwbwPF7xX-izyBv0gQH3zB1N54nc0gpyAnF1-558IhAQ22wjwPKiaTP9352nN91i2lZLno40xIuga5P2FybZxxcieVXkh4TcE9nOaFT2CCdZYueNTzl1QKlABp4R_I1Z9inJ74MqSUDFNET1-Lzsd02sgseTCdyFnsjcFRBNX0odem6_gNAVr4lwPB_iO_ikP3_ZWmeBxDLQ-1qZNyRWgCd2TQTvWEfmXLAEU__RrEA_A1S2GSYguSsWw14t7WbrrAlYs44O6nXFexdpmCqxe_to7hkfz2RAE9eJ0LkP-wKAPeNk66bFgey_inMxQDHQCCx-YYlhGSocv6U2CNSY3g2Axe-3cjNFzSqQxEKz7gvEZx2qWZpJF2L3fb1K1_khRj7Cs-PAAe_Znn7deajlLuZQvQ0z_9ST8svXe5Of79K_GaUzk10D9YxeyGgZa2bzbXy2FxUEeIFHF4GCOqq3ETbXdINUoQFeVlhJ2l0m50xg8sCxnwEv9SwzvliagkFyoI9psru8F76R_aMcPMfK1v1X-kiMFPpJZ4trIGj7u51JB9dxIEv2KPwvE5HuVyjyb2IlW_gnzD8SXqrF657DzTIdna3vdQjgMrkIjxhileCDTf1zT-flmL2zflTAONBFERKC-wrCvmZs3gz-LFnNWDT1pA",
          expires_in: 0
        }
      })
    ).toMatchObject({
      token:
        "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImFmZTMwOTg1MDFkMmIzNGQ1M2I2NmYzODM2ZTExY2Y2MTczNTgyYTQ2YjU1ZjQxMGVjMTg0ZjM0YTZmYzVjOWVlZWM3ZjAxNzk5NGM1YTJiIn0.eyJhdWQiOiIxIiwianRpIjoiYWZlMzA5ODUwMWQyYjM0ZDUzYjY2ZjM4MzZlMTFjZjYxNzM1ODJhNDZiNTVmNDEwZWMxODRmMzRhNmZjNWM5ZWVlYzdmMDE3OTk0YzVhMmIiLCJpYXQiOjE0OTk4NzQ5MzMsIm5iZiI6MTQ5OTg3NDkzMywiZXhwIjoxNTMxNDEwOTMzLCJzdWIiOiIiLCJzY29wZXMiOltdfQ.G6QnPqzxYgqNZeb7ABwbwPF7xX-izyBv0gQH3zB1N54nc0gpyAnF1-558IhAQ22wjwPKiaTP9352nN91i2lZLno40xIuga5P2FybZxxcieVXkh4TcE9nOaFT2CCdZYueNTzl1QKlABp4R_I1Z9inJ74MqSUDFNET1-Lzsd02sgseTCdyFnsjcFRBNX0odem6_gNAVr4lwPB_iO_ikP3_ZWmeBxDLQ-1qZNyRWgCd2TQTvWEfmXLAEU__RrEA_A1S2GSYguSsWw14t7WbrrAlYs44O6nXFexdpmCqxe_to7hkfz2RAE9eJ0LkP-wKAPeNk66bFgey_inMxQDHQCCx-YYlhGSocv6U2CNSY3g2Axe-3cjNFzSqQxEKz7gvEZx2qWZpJF2L3fb1K1_khRj7Cs-PAAe_Znn7deajlLuZQvQ0z_9ST8svXe5Of79K_GaUzk10D9YxeyGgZa2bzbXy2FxUEeIFHF4GCOqq3ETbXdINUoQFeVlhJ2l0m50xg8sCxnwEv9SwzvliagkFyoI9psru8F76R_aMcPMfK1v1X-kiMFPpJZ4trIGj7u51JB9dxIEv2KPwvE5HuVyjyb2IlW_gnzD8SXqrF657DzTIdna3vdQjgMrkIjxhileCDTf1zT-flmL2zflTAONBFERKC-wrCvmZs3gz-LFnNWDT1pA"
    });
  });

  it("should handle logout and etc", () => {
    actionsToResetAuth.forEach(type => {
      expect(Auth(dirtyState, { type })).toMatchObject(initialState);
    });
  });

  it("should handle iframe failure", () => {
    expect(
      Auth(dirtyState, {
        type: actionTypes.app.errors.OAUTH_IFRAME_FAILURE,
        payload: {
          status: 403
        }
      })
    ).toMatchObject(initialState);
    expect(
      Auth(dirtyState, {
        type: actionTypes.app.errors.OAUTH_IFRAME_FAILURE,
        error: {
          code: 18
        }
      })
    ).toMatchObject(initialState);
  });
});
