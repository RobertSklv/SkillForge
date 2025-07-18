import { loadUserPage } from 'skillforge-common/api/client.js';
import type UserPageData from 'skillforge-common/types/UserPageData';

export async function load({ fetch, params, parent }) {
    let { authToken } = await parent();

    let data: UserPageData = await loadUserPage(params.name, fetch, authToken);

    return data;
}