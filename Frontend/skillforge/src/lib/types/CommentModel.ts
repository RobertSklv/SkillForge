import type RatingData from "./RatingData";
import type UserLink from "./UserLinkType";

export default interface CommentModel {
    CommentId: number,
    User: UserLink,
    Content: string,
    RatingData: RatingData,
    DateWritten: string,
}