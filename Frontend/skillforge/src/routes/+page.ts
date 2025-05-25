import { loadCurrentUser } from "$lib/stores/currentUserStore";
import type SiteData from "$lib/types/SiteData";

export async function load({ fetch }): Promise<SiteData> {
    let currentUser = await loadCurrentUser(fetch);

    return {
        currentUser
    };
}