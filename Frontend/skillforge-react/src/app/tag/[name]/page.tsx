import { TagPage } from '@/components/tag-page/TagPage';
import { loadTagPage } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import TagPageData from '@/lib/types/TagPageData';
import { GetServerSidePropsContext, Metadata } from 'next';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export async function generateMetadata({ params }: GetServerSidePropsContext): Promise<Metadata> {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;
    let data: TagPageData = await loadTagPage(params?.name as string, authToken);

    return {
        title: `SkillForge | #${params?.name}`,
        description: data?.Description ?? `${params?.name}'s profile'`,
        robots: 'index,nofollow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/tag/${params?.name}`
        }
    };
}

export default async function TagView({ params }: GetServerSidePropsContext) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    if (!params) {
        throw redirect('/');
    }

    let data: TagPageData = await loadTagPage(params.name as string, authToken);

    return <TagPage data={data} />;
}
