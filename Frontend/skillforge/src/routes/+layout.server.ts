import { getCurrentUser, getReportFormOptions, JWT_TOKEN_COOKIE_NAME } from "$lib/api/client";
import type ReportFormOptions from "$lib/types/ReportFormOptions.js";
import type UserInfo from "$lib/types/UserInfo.js";
import { parse } from 'cookie';

export async function load({ fetch, request }): Promise<any> {
	const cookieHeader = request.headers.get('cookie');
	let token: string | undefined;

	if (cookieHeader) {
		const cookies = parse(cookieHeader);
		token = cookies[JWT_TOKEN_COOKIE_NAME];
	}

    let currentUserInfo: UserInfo | undefined;
    let reportFormOptions: ReportFormOptions | undefined;

    if (token) {
        [
            currentUserInfo,
            reportFormOptions
        ] = await Promise.all([
            getCurrentUser(fetch, token),
            getReportFormOptions(fetch, token),
        ]);
    }

    return {
        currentUserInfo,
        reportFormOptions
    };
}