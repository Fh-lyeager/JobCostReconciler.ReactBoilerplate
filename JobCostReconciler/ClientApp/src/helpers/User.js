import { containsAll } from "./Contains";
import config from "../config";

/**
 * User-can check to user throughout application
 *
 * @param auth
 * @param permissions
 * @returns {boolean|*}
 */
export const userCan = (auth, permissions = []) => {
  return auth.isLoggedIn && containsAll(auth.access.scopes, permissions);
};

/**
 * Check auth expiration
 * @param auth
 * @returns {boolean}
 */
export const hasExpired = auth => {
  if (auth.expiresAt) {
    const now = new Date().getTime();
    return now > auth.expiresAt;
  }

  return true;
};

/**
 * Get expires in based on auth.expiresAt
 * @param auth
 * @param expirationBuffer
 * @returns {number}
 */
export const getExpiresIn = (
  auth,
  expirationBuffer = config.oauth.expirationBuffer
) => {
  if (auth.expiresAt) {
    const expiration = auth.expiresAt - expirationBuffer;
    const now = new Date().getTime();

    if (expiration > now) {
      return expiration - now;
    }
  }

  return 0;
};
