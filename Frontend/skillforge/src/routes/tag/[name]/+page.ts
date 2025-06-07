import { loadTagPage } from '$lib/api/client.js';
import type TagPageData from '$lib/types/TagPageData.js';

export async function load({ fetch, params }) {
    let data: TagPageData = await loadTagPage(fetch, params.name);

    return data;
}