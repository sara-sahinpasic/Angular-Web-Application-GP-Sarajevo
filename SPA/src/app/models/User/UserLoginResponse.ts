import { UserTokenData } from "./userToken";

export interface UserLoginResponse {
  loginData: UserTokenData | string,
  isTwoWayAuth: boolean
}
