import { UserTokenData } from "./UserToken";

export interface UserLoginResponse {
  loginData: UserTokenData | string,
  isTwoWayAuth: boolean
}
