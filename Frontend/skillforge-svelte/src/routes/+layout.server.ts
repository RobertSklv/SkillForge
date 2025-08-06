import { getCurrentUser, getReportFormOptions } from "$lib/api/client";
import { JWT_TOKEN_COOKIE_NAME } from "$lib/auth.js";
import type ReportFormOptions from "$lib/types/ReportFormOptions.js";
import type UserInfo from "$lib/types/UserInfo.js";

export async function load({ fetch, cookies, depends }): Promise<any> {
    depends('app:auth');

    let authToken = cookies.get(JWT_TOKEN_COOKIE_NAME);
    let currentUserInfo: UserInfo | undefined;
    let reportFormOptions: ReportFormOptions | undefined;

    if (authToken) {
        [
            currentUserInfo,
            reportFormOptions
        ] = await Promise.all([
            getCurrentUser(authToken, fetch),
            getReportFormOptions(authToken, fetch),
        ]);
    }

    return {
        currentUserInfo,
        reportFormOptions,
        authToken
    };
}