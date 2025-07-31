import { ArticleUpsertForm } from "@/components/article-upsert-form/ArticleUpsertForm";
import { GetServerSidePropsContext } from "next";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import { getCurrentUser, loadArticleUpsertPage } from "@/lib/api/client";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";

export default async function ArticleEdit({ params }: GetServerSidePropsContext) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    if (!currentUserInfo) {
        throw redirect('/join');
    }

    let pageModel = await loadArticleUpsertPage(authToken, parseInt(params?.id as string));

    return <ArticleUpsertForm page={pageModel} />
}