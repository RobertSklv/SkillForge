import { UserPage } from '@/components/user-page/UserPage';
import { loadUserPage } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import UserPageData from '@/lib/types/UserPageData';
import { GetServerSidePropsContext, Metadata } from 'next';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export async function generateMetadata({ params }: GetServerSidePropsContext): Promise<Metadata> {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;
    let data: UserPageData = await loadUserPage(params?.name as string, authToken);

    return {
        title: `SkillForge | '${params?.name}'`,
        description: data?.Bio ?? `${params?.name}'s profile'`,
        robots: 'index,follow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/user/${params?.name}`
        }
    };
}

export default async function UserView({ params }: GetServerSidePropsContext) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    if (!params) {
        throw redirect('/');
    }

    let data: UserPageData = await loadUserPage(params.name as string, authToken);

    return <UserPage data={data} />;
}
