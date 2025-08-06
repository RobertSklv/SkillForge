import type ArticleCardType from "./ArticleCardType";
import type TagLink from "./TagLinkType";
import type TopArticleItemType from "./TopArticleItemType";
import type UserLink from "./UserLinkType";

export default interface HomePageData {
    TopArticles: TopArticleItemType[],
    TopUsers: UserLink[],
    TopTags: TagLink[],
    LatestArticles: ArticleCardType[],
}