import { redirect } from '@sveltejs/kit';

export async function load({ parent }) {
    let { currentUserInfo } = await parent();

    if (!currentUserInfo) {
        throw redirect(302, '/join');
    }

    return {};
}