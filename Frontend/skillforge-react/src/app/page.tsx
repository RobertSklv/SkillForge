import { AnchorList } from "@/components/anchor-list/AnchorList";
import { HomePageArticleInfiniteScroll } from "@/components/home-page-article-infinite-scroll/HomePageArticleInfiniteScroll";
import { ThreeColumns } from "@/components/layout/three-columns/ThreeColumns";
import { LoginCta } from "@/components/login-cta/LoginCta";
import { TagLink } from "@/components/tag-link/TagLink";
import { TopArticleLink } from "@/components/top-article-link/TopArticleLink";
import { UserLink } from "@/components/user-link/UserLink";
import { loadHomePage } from "@/lib/api/client";
import HomePageData from "@/lib/types/HomePageData";
import { Metadata } from "next";

export const metadata: Metadata = {
    title: 'SkillForge | Home',
    description: 'Discover and share high-quality articles on technology, development, and more. Follow tags and authors, engage with the community, and explore trending content.',
    robots: 'index,follow',
    alternates: {
        canonical: process.env.NEXT_PUBLIC_API_BASE_URL
    }
}

export default async function Home() {
    let data: HomePageData = await loadHomePage();

    async function leftColumn() {
        'use server'

        return (
            <div className="d-flex flex-column gap-4">
                <AnchorList title="Top Users" items={data.TopUsers} itemSnippet={item => (
                    <li className="list-group-item bg-transparent px-0" key={item.Name}>
                        <UserLink data={item} />
                    </li>
                )} />
                <AnchorList title="Popular Tags" items={data.TopTags} itemSnippet={item => (
                    <li className="list-group-item bg-transparent px-0" key={item.Name}>
                        <TagLink data={item} />
                    </li>
                )} />
            </div>
        );
    }

    async function rightColumn() {
        'use server'

        return (
            <div className="d-flex flex-column gap-4">
                <AnchorList title="Top Articles" items={data.TopArticles} itemSnippet={item => (
                    <li className="list-group-item p-0" key={item.ArticleId}>
                        <TopArticleLink data={item} key={item.ArticleId} />
                    </li>
                )} />
                <LoginCta
                    ctaText="Join us"
                    description="to receive personalized content and interact with the rest of the community."
                />
            </div>
        );
    }

    return (
        <ThreeColumns leftColumn={leftColumn()} rightColumn={rightColumn()} hideLeftColumnOnMobile hideRightColumnOnMobile>
            <HomePageArticleInfiniteScroll data={data} />
        </ThreeColumns>
    );
}
