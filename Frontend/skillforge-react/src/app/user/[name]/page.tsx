import { UserPage } from '@/components/user-page/UserPage';
import { getCurrentUser, loadUserPage } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import UserPageData from '@/lib/types/UserPageData';
import { GetServerSidePropsContext } from 'next';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export default async function UserView({ params }: GetServerSidePropsContext) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    if (!currentUserInfo) {
        throw redirect('/join');
    }

    if (!params) {
        throw redirect('/');
    }

    let data: UserPageData = await loadUserPage(params.name as string, authToken);

    return <UserPage data={data} />;
}
