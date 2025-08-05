import { AnchorList } from "@/components/anchor-list/AnchorList";
import { Article } from "@/components/article/ArticlePage";
import { AuthorBlock } from "@/components/author-block/AuthorBlock";
import { TwoColumns } from "@/components/layout/two-columns/TwoColumns";
import { TopArticleLink } from "@/components/top-article-link/TopArticleLink";
import { GetServerSidePropsContext, Metadata } from "next";
import { viewArticle } from "@/lib/api/client";
import type ArticlePageModel from '@/lib/types/ArticlePageModel';
import { getFrontendUrl, htmlToText, truncateText } from "@/lib/util";
import { cookies } from "next/headers";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";

export async function generateMetadata({ params }: GetServerSidePropsContext): Promise<Metadata> {
    const data: ArticlePageModel = await viewArticle(parseInt(params?.id as string));

    return {
        title: `SkillForge | ${data.Title}`,
        description: `${truncateText(htmlToText(data.Content), 120)}, Tags: ${data.Tags.join(', ')}`,
        robots: 'index,follow',
        alternates: {
            canonical: getFrontendUrl(`/article/${params?.id}`)
        }
    };
}

export default async function ArticleView({ params }: GetServerSidePropsContext) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;
    const data: ArticlePageModel = await viewArticle(parseInt(params?.id as string), authToken);

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