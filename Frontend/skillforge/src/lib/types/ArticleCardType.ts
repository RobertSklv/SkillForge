import type CommentModel from "./CommentModel";
import type RatingData from "./RatingData";
import type TagLinkType from "./TagLinkType";
import type UserLink from "./UserLinkType";

export default interface ArticleCardType {
    ArticleId: number,
    Author: UserLink,
    Title: string,
    CoverImage?: string,
    CategoryName: string,
    CategoryCode: string,
    DatePublished: string,
    RatingData: RatingData,
    Tags: TagLinkType[],
    Comments: CommentModel[],
    TotalComments: number,
}