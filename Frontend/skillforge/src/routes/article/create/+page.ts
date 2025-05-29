import { requestApiRaw } from '$lib/api/client.js';
import type ArticleCreatePageModel from '$lib/types/ArticleCreatePageModel';

export async function load({ fetch }): Promise<ArticleCreatePageModel> {
    let pageModel = await requestApiRaw<ArticleCreatePageModel>(fetch, '/Article/LoadPage');

    return pageModel;
}