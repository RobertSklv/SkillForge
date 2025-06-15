import { loadTagPage } from '$lib/api/client.js';
import type TagPageData from '$lib/types/TagPageData';

export async function load({ fetch, params }) {
    let data: TagPageData = await loadTagPage(params.name, fetch);

    return data;
}