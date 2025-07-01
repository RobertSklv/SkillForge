import { PUBLIC_BASE_URL } from "$env/static/public";
import type { HandleFetch } from "@sveltejs/kit";

export const handleFetch: HandleFetch = async ({ request, fetch }) => {
    console.log('[SSR FETCH]', request.url);
    
    if (request.url.startsWith(PUBLIC_BASE_URL)) {
        // Workaround: https://github.com/sveltejs/kit/issues/6608
        request.headers.set('origin', PUBLIC_BASE_URL);
    }

    return fetch(request);
};