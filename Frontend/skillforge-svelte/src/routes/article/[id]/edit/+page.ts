import { loadArticleUpsertPage } from 'skillforge-common/api/client.js';
import type ArticleUpsertPageModel from 'skillforge-common/types/ArticleUpsertPageModel';
import { redirect } from '@sveltejs/kit';

export async function load({ fetch, params, parent }): Promise<ArticleUpsertPageModel> {
    let { currentUserInfo, authToken } = await parent();

    if (!currentUserInfo) {
        throw redirect(302, '/join');
    }

    let pageModel = await loadArticleUpsertPage(authToken, parseInt(params.id), fetch);

    return pageModel;
}