import type RatingData from "./RatingData";
import type UserLink from "./UserLink";

export default interface CommentModel {
    CommentId: number,
    User: UserLink,
    Content: string,
    RatingData: RatingData,
    DateWritten: string,
}