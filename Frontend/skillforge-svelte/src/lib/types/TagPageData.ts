import type ArticleCardType from "./ArticleCardType";
import type TopArticleItemType from "./TopArticleItemType";
import type UserListItemType from "./UserListItemType";

export default interface TagPageData {
    Name: string,
    Description?: string,
    FollowersCount: number,
    ArticlesCount: number,
    IsFollowedByCurrentUser: boolean,
    LatestFollowers: UserListItemType[],
    LatestArticles: ArticleCardType[],
    TopArticles: TopArticleItemType[],
}