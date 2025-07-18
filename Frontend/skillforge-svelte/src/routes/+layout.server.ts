import { getCurrentUser, getReportFormOptions } from "skillforge-common/api/client";
import { JWT_TOKEN_COOKIE_NAME } from "$lib/auth";
import type ReportFormOptions from "skillforge-common/types/ReportFormOptions.js";
import type UserInfo from "skillforge-common/types/UserInfo.js";

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
            getCurrentUser(fetch, authToken),
            getReportFormOptions(fetch, authToken),
        ]);
    }

    return {
        currentUserInfo,
        reportFormOptions,
        authToken
    };
}