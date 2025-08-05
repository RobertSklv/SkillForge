import { AccountEditPage } from '@/components/account-edit-page/AccountEditPage';
import { getCurrentUser, loadAccountInfoForm } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import AccountInfoFormData from '@/lib/types/AccountInfoFormData';
import { Metadata } from 'next';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export async function generateMetadata(): Promise<Metadata> {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    return {
        title: `SkillForge | Edit account`,
        description: `Edit account`,
        robots: 'noindex,nofollow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/user/${currentUserInfo?.Name}`
        }
    };
}

export default async function AccountEdit() {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    if (!currentUserInfo) {
        throw redirect('/join');
    }

    let accountInfo: AccountInfoFormData = await loadAccountInfoForm(authToken);

    return <AccountEditPage data={accountInfo} />;
}
