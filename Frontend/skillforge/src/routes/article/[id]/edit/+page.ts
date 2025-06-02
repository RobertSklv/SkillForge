import { loadArticleUpsertPage } from '$lib/api/client.js';
import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';

export async function load({ fetch, params }): Promise<ArticleUpsertPageModel> {
    let pageModel = await loadArticleUpsertPage(fetch, parseInt(params.id));

    return pageModel;
}