import type ArticleCardType from "./ArticleCardType";
import type TagLink from "./TagLinkType";
import type TopArticleItemType from "./TopArticleItemType";
import type UserLink from "./UserLinkType";

export default interface HomePageData {
    LatestArticles: ArticleCardType[],
    TopArticles: TopArticleItemType[],
    TopUsers: UserLink[],
    TopTags: TagLink[],
}