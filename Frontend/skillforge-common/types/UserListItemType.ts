import type UserLinkType from "./UserLinkType";

export default interface UserListItemType {
    Link: UserLinkType,
    IsFollowedByCurrentUser: boolean,
}