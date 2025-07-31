import { searchArticlesAdvanced } from '$lib/api/client.js';
import type GridState from '$lib/types/GridState.js';

export async function load({ fetch, url }) {
    let pageParam = url.searchParams.get('p');
    let limitParam = url.searchParams.get('limit');

    let gridState: GridState = {
        p: pageParam ? parseInt(pageParam) : undefined,
        limit: limitParam ? parseInt(limitParam) : undefined,
        q: url.searchParams.get('q'),
        sortBy: url.searchParams.get('sortBy'),
        sortOrder: url.searchParams.get('sortOrder'),
    };

    let response = await searchArticlesAdvanced(gridState, fetch);

    return {
        response,
        gridState,
    }
}