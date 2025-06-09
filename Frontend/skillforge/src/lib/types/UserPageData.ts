import type TagListItemType from "./TagListItemType";
import type TopArticleItemType from "./TopArticleItemType";
import type UserListItemType from "./UserListItemType";

export default interface UserPageData {
    Name: string,
    Bio?: string,
    AvatarImage?: string,
    FollowersCount: number,
    FollowingsCount: number,
    TagFollowingsCount: number,
    ArticlesCount: number,
    IsFollowedByCurrentUser: boolean,
    LatestTagFollowings: TagListItemType[],
    LatestFollowers: UserListItemType[],
    LatestFollowings: UserListItemType[],
    TopArticles: TopArticleItemType[],
}