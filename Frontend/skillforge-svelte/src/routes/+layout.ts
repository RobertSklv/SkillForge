import { browser } from "$app/environment";
import { goto } from "$app/navigation";
import { PUBLIC_BACKEND_DOMAIN } from "$env/static/public";
import { currentUserStore } from "$lib/stores/currentUserStore";
import { initEnv } from "skillforge-common/env";

export async function load() {
    initEnv({
        backendUrl: PUBLIC_BACKEND_DOMAIN,
        isBrowser: browser,
        onAuthError: () => {
            currentUserStore.set(undefined);
            goto('/join');
        }
    });
}