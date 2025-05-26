import { getCurrentUser, logout } from "$lib/api/client";
import type UserInfo from "$lib/types/UserInfo";
import { writable } from "svelte/store"

export async function loadCurrentUser() {
    let userInfo = await getCurrentUser();
    currentUserStore.set(userInfo);

    return userInfo;
}

export async function logoutUser() {
    await logout()
        .then(r => {
            currentUserStore.set(null);
        })
        .catch(e => {
            console.error('Error on logout', e);
        });
}

export var currentUserStore = writable<UserInfo | null>();