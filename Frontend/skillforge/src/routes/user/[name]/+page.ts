import { loadUserPage } from '$lib/api/client.js';
import type UserPageData from '$lib/types/UserPageData';

export async function load({ fetch, params }) {
    let data: UserPageData = await loadUserPage(params.name, fetch);

    return data;
}