import { ArticleUpsertForm } from "@/components/article-upsert-form/ArticleUpsertForm";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import { getCurrentUser, loadArticleUpsertPage } from "@/lib/api/client";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";
import { Metadata } from "next";

export const metadata: Metadata = {
    title: 'SkillForge | Create article',
    description: 'Create article',
    robots: 'noindex,nofollow'
};

export default async function ArticleCreate() {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    if (!currentUserInfo) {
        throw redirect('/join');
    }

    let pageModel = await loadArticleUpsertPage(authToken);

    return <ArticleUpsertForm page={pageModel} />
}