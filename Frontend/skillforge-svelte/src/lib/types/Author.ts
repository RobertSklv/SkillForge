import type UserLinkType from "./UserLinkType";

export default interface Author {
    Link: UserLinkType,
    Bio?: string,
    JoinedDate: string,
    IsFollowedByCurrentUser: boolean,
}