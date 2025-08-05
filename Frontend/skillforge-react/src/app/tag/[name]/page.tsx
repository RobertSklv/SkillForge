import { TagPage } from '@/components/tag-page/TagPage';
import { loadTagPage } from '@/lib/api/client';
import { JWT_TOKEN_COOKIE_NAME } from '@/lib/auth';
import TagPageData from '@/lib/types/TagPageData';
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
    let data: TagPageData = await loadTagPage(name, authToken);

    return {
        title: `SkillForge | #${params?.name}`,
        description: data?.Description ?? `${params?.name}'s profile'`,
        robots: 'index,nofollow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/tag/${params?.name}`
        }
    };
}

export default async function TagView({ params }: IProps) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const { name } = await params;

    let data: TagPageData = await loadTagPage(name, authToken);

    return <TagPage data={data} />;
}
