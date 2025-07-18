import { loadTagPage } from 'skillforge-common/api/client.js';
import type TagPageData from 'skillforge-common/types/TagPageData';

export async function load({ fetch, params, parent }) {
    let { authToken } = await parent();

    let data: TagPageData = await loadTagPage(params.name, fetch, authToken);

    return data;
}