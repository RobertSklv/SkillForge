import type UserInfo from "./UserInfo";

export default interface LoginResponse {
    CurrentUserInfo: UserInfo,
    AuthToken: string
}