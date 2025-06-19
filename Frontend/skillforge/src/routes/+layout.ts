import { getCurrentUser } from "$lib/api/client";
import type UserInfo from "$lib/types/UserInfo";

export async function load({ fetch }): Promise<UserInfo | undefined> {
    let currentUserInfo = await getCurrentUser(fetch);

    return currentUserInfo;
}