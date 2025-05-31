import { viewArticle } from '$lib/api/client.js';
import type ArticlePageModel from '$lib/types/ArticlePageModel.js';

export async function load({ fetch, params }) {
    let article: ArticlePageModel = await viewArticle(fetch, parseInt(params.id));

    return {
        article
    }
}