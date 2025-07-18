import { loadAccountInfoForm } from 'skillforge-common/api/client.js';
import type AccountInfoFormData from 'skillforge-common/types/AccountInfoFormData';
import { redirect } from '@sveltejs/kit';

export async function load({ parent }) {
    let { currentUserInfo, authToken } = await parent();

    if (!currentUserInfo) {
        throw redirect(302, '/join');
    }

    let accountInfo: AccountInfoFormData = await loadAccountInfoForm(authToken);

    return {
        accountInfo
    };
}