import type RatingData from "./RatingData";
import type UserLink from "./UserLinkType";

export default interface TopArticleItemType {
    ArticleId: number,
    Author: UserLink,
    Title: string,
    ViewCount: number,
    CommentCount: number,
    DatePublished: string,
    RatingData: RatingData,
}