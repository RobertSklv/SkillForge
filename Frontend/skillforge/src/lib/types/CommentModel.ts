import type RatingData from "./RatingData";
import type UserLink from "./UserLink";

export default interface CommentModel {
    User: UserLink,
    Content: string,
    RatingData: RatingData,
    DateWritten: string,
}