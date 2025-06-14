import { loadArticleUpsertPage } from '$lib/api/client.js';
import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';

export async function load({ fetch }): Promise<ArticleUpsertPageModel> {
    let pageModel = await loadArticleUpsertPage(fetch);

    return pageModel;
}