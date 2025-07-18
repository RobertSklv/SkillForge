import { viewArticle } from 'skillforge-common/api/client.js';
import type ArticlePageModel from 'skillforge-common/types/ArticlePageModel.js';

export async function load({ fetch, params }) {
    let article: ArticlePageModel = await viewArticle(fetch, parseInt(params.id));

    return article;
}