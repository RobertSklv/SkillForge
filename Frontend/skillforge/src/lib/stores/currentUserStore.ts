import { getCurrentUser, logout } from "$lib/api/client";
import type UserInfo from "$lib/types/UserInfo";
import { writable } from "svelte/store"

export async function logoutUser() {
    await logout()
        .then(r => {
            currentUserStore.set(undefined);
        })
        .catch(e => {
            console.error('Error on logout', e);
        });
}

export var currentUserStore = writable<UserInfo | undefined>();