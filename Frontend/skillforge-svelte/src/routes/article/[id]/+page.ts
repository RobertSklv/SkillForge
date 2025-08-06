import { viewArticle } from '$lib/api/client.js';
import type ArticlePageModel from '$lib/types/ArticlePageModel.js';

export async function load({ fetch, params }) {
    let article: ArticlePageModel = await viewArticle(parseInt(params.id), fetch);

    return article;
}