import type CommentModel from "./CommentModel"
import type RatingData from "./RatingData"
import type Author from "./Author"
import type TopArticleItemType from "./TopArticleItemType";

export default interface ArticlePageModel {
    ArticleId: number,
    Author: Author,
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
    LatestArticlesByAuthor: TopArticleItemType[],
}