import { loadArticleUpsertPage } from '$lib/api/client.js';
import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';
import { redirect } from '@sveltejs/kit';

export async function load({ fetch, parent }): Promise<ArticleUpsertPageModel> {
    let { currentUserInfo, authToken } = await parent();

    if (!currentUserInfo) {
        throw redirect(302, '/join');
    }

    let pageModel = await loadArticleUpsertPage(authToken, undefined, fetch);

    return pageModel;
}