import { getCurrentUser } from "$lib/api/client";
import type SvelteFetch from "$lib/types/SvelteFetch";
import type UserInfo from "$lib/types/UserInfo";
import { writable } from "svelte/store"

export async function loadCurrentUser(fetch: SvelteFetch) {
    let userInfo = await getCurrentUser(fetch);
    currentUserStore.set(userInfo);

    return userInfo;
}

export var currentUserStore = writable<UserInfo | null>();