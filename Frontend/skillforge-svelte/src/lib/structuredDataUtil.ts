import type ArticleCardType from "$lib/types/ArticleCardType";
import type TopArticleItemType from "$lib/types/TopArticleItemType";
import type UserLinkType from "$lib/types/UserLinkType";
import { getFrontendUrl, getImagePath } from "$lib/util";


export function generateArticleCardItemsSchema(items: ArticleCardType[]) {
    return items.map((item, index) => {
        return {
            '@type': 'ListItem',
            item: {
                '@type': 'Article',
                author: generateUserLinkSchema(item.Author),
                datePublished: item.DatePublished,
                headline: item.Title,
                url: getFrontendUrl('/article/' + item.ArticleId),
                commentCount: item.Comments.length,
                comments: item.Comments.map(c => {
                    return {
                        '@type': 'Comment',
                        author: generateUserLinkSchema(c.User),
                        text: c.Content,
                        upvoteCount: c.RatingData.ThumbsUp,
                        downvoteCount: c.RatingData.ThumbsDown,
                        datePublished: c.DateWritten,
                    };
                })
            },
            position: index
        };
    }) ?? [];
}

export function generateTopArticleItemsSchema(items: TopArticleItemType[]) {
    return items.map((item, index) => {
        return {
            '@type': 'ListItem',
            item: {
                '@type': 'Article',
                author: generateUserLinkSchema(item.Author),
                datePublished: item.DatePublished,
                headline: item.Title,
                commentCount: item.CommentCount,
                url: getFrontendUrl('/article/' + item.ArticleId)
            },
            position: index
        };
    }) ?? [];
}

export function generateTopUserItemsSchema(items: UserLinkType[]) {
    return items.map((item, index) => {
        return {
            '@type': 'ListItem',
            item: generateUserLinkSchema(item),
            position: index
        };
    }) ?? [];
}

export function generateUserLinkSchema(userLink: UserLinkType) {
    return {
        '@type': 'Person',
        name: userLink.Name,
        url: getFrontendUrl('/user/' + userLink.Name),
        image: getImagePath(userLink.AvatarImage)
    };
}