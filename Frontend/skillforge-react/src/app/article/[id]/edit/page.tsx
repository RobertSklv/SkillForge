import { ArticleUpsertForm } from "@/components/article-upsert-form/ArticleUpsertForm";
import { Metadata } from "next";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import { getCurrentUser, loadArticleUpsertPage } from "@/lib/api/client";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";

export interface IProps {
    params: {
        id: number;
    };
}

export async function generateMetadata({ params }: IProps): Promise<Metadata> {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const { id } = await params;
    let data = await loadArticleUpsertPage(authToken, id);

    return {
        title: `SkillForge | Edit article '${data.CurrentState?.Model.Title}'`,
        description: `Edit article '${data.CurrentState?.Model.Title}'`,
        robots: 'noindex,nofollow',
        alternates: {
            canonical: `${process.env.NEXT_PUBLIC_BASE_URL}/article/${data.CurrentState?.Model.Id}`
        }
    };
}

export default async function ArticleEdit({ params }: IProps) {
    const cookieContext = await cookies();
    const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

    const currentUserInfo = await getCurrentUser(authToken);

    if (!currentUserInfo) {
        throw redirect('/join');
    }

    const { id } = await params;

    let pageModel = await loadArticleUpsertPage(authToken, id);

    return <ArticleUpsertForm page={pageModel} />;
}