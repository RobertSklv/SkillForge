import { getCurrentUser, getReportFormOptions } from "$lib/api/client";

export async function load({ fetch }): Promise<any> {
    let [
        currentUserInfo,
        reportFormOptions
    ] = await Promise.all([
        getCurrentUser(fetch),
        getReportFormOptions(fetch),
    ]);

    return {
        currentUserInfo,
        reportFormOptions
    };
}