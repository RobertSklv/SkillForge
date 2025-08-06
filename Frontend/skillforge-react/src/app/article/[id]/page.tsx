import { AnchorList } from "@/components/anchor-list/AnchorList";
import { Article } from "@/components/article/ArticlePage";
import { AuthorBlock } from "@/components/author-block/AuthorBlock";
import { TwoColumns } from "@/components/layout/two-columns/TwoColumns";
import { TopArticleLink } from "@/components/top-article-link/TopArticleLink";
import { Metadata } from "next";
import { viewArticle } from "@/lib/api/client";
import type ArticlePageModel from '@/lib/types/ArticlePageModel';
import { getFrontendUrl, htmlToText, truncateText } from "@/lib/util";
import { cookies } from "next/headers";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";

export interface IProps {
    params: {
        id: number;
    };
}

export async function generateMetadata({ params }: IProps): Promise<Metadata> {
    const { id } = await params;
    const data: ArticlePageModel = await viewArticle(id);

    return {
        title: `SkillForge | ${data.Title}`,
        description: `${truncateText(htmlToText(data.Content), 120)}, Tags: ${data.Tags.join(', ')}`,
        robots: 'index,follow',
        alternates: {
            canonical: getFrontendUrl(`/article/${id}`)
        }
    };
}

export default async function ArticleView({ params }: IProps) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;
    const cookieConsent = cookieContext.get('cookie_consent');
    const guestId = cookieContext.get('guest_id');

    const forwardedCookies = [];
    if (cookieConsent) forwardedCookies.push(cookieConsent);
    if (guestId) forwardedCookies.push(guestId);

    const { id } = await params;
    const data: ArticlePageModel = await viewArticle(id, authToken, forwardedCookies);

    function leftColumn() {
        return (
            <>
                <AuthorBlock data={data.Author} classes="mb-4" />

                {data.LatestArticlesByAuthor.length > 0 &&
                    <AnchorList
                        title="Latest by author"
                        items={data.LatestArticlesByAuthor}
                        itemSnippet={item => <TopArticleLink data={item} key={`top-article-${item.ArticleId}`} />}
                    />
                }
            </>
        );
    }

    return (
        <TwoColumns leftColumn={leftColumn()}>
            <Article data={data} />
        </TwoColumns>
    );
}