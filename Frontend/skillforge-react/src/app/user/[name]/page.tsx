import { UserPage } from '@/components/user-page/UserPage';
import { loadUserPage } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import UserPageData from '@/lib/types/UserPageData';
import { Metadata } from 'next';
import { cookies } from 'next/headers';

export interface IProps {
    params: {
        name: string;
    };
}

export async function generateMetadata({ params }: IProps): Promise<Metadata> {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const { name } = await params;
    let data: UserPageData = await loadUserPage(name, authToken);

    return {
        title: `SkillForge | '${params?.name}'`,
        description: data?.Bio ?? `${params?.name}'s profile'`,
        robots: 'index,follow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/user/${params?.name}`
        }
    };
}

export default async function UserView({ params }: IProps) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const { name } = await params;

    let data: UserPageData = await loadUserPage(name, authToken);

    return <UserPage data={data} />;
}
