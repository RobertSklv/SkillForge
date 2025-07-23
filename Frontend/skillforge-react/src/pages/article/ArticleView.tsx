import { AnchorList } from '@components/anchor-list/AnchorList';
import { Article } from '@components/article/Article';
import { AuthorBlock } from '@components/author-block/AuthorBlock';
import { TwoColumns } from '@components/layout/two-columns/TwoColumns';
import { TopArticleLink } from '@components/top-article-link/TopArticleLink';
import { useParams } from 'react-router-dom';
import type ArticlePageModel from 'skillforge-common/types/ArticlePageModel';

export interface IArticleViewProps {
    data: ArticlePageModel;
}

export function ArticleView({ data }: IArticleViewProps) {
    const { id } = useParams();

    function leftColumn() {
        return (
            <>
                <AuthorBlock data={data.Author} classes="mb-4" />

                {data.LatestArticlesByAuthor.length > 0 &&
                    <AnchorList
                        title="Latest by author"
                        items={data.LatestArticlesByAuthor}
                        itemSnippet={item => <TopArticleLink data={item} />}
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
