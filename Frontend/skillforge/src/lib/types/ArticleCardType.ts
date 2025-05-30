import type UserLink from "./UserLink";

export default interface ArticleCardType {
    ArticleId: number,
    Author: UserLink,
    Title: string,
    CoverImage?: string,
    CategoryName: string,
    CategoryCode: string,
    DatePublished: string,
    Rating: number,
}