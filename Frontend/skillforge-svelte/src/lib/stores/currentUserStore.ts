import type UserInfo from "$lib/types/UserInfo";
import { writable } from "svelte/store"

export async function logoutUser() {
    currentUserStore.set(undefined);
}

export var currentUserStore = writable<UserInfo | undefined>();