import type CommentModel from "./CommentModel"
import type RatingData from "./RatingData"
import type UserLink from "./UserLink"

export default interface ArticlePageModel {
    ArticleId: number,
    Author: UserLink,
    CoverImage?: string,
    Title: string,
    Content: string,
    CategoryName: string,
    CategoryCode: string,
    DatePublished: string,
    Tags: string[],
    Comments: CommentModel[]
    RatingData: RatingData,
    Views: number,
}