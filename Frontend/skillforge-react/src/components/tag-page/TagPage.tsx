'use client'

import { Block } from '../block/Block';
import { ThreeColumns } from '../layout/three-columns/ThreeColumns';
import { InfiniteScroll } from '../infinite-scroll/InfiniteScroll';
import { ArticleCard } from '../article-card/ArticleCard';
import { ArticleCardPlaceholder } from '../article-card-placeholder/ArticleCardPlaceholder';
import TagPageData from '@/lib/types/TagPageData';
import { OutOfArticlesBlock } from '../out-of-articles-block/OutOfArticlesBlock';
import { AnchorList } from '../anchor-list/AnchorList';
import { LoginCta } from '../login-cta/LoginCta';
import { TopArticleLink } from '../top-article-link/TopArticleLink';
import { useCurrentUser } from '@/hooks/useCurrentUser';
import { FollowButton } from '../follow-button/FollowButton';
import ArticleCardType from '@/lib/types/ArticleCardType';
import UserListItemType from '@/lib/types/UserListItemType';
import { getLatestArticlesByTag, getTagFollowers } from '@/lib/api/client';
import { FollowList } from '../follow-list/FollowList';
import { UserListItem } from '../user-list-item/UserListItem';

const ARTICLE_BATCH_SIZE: number = 4;
const TAG_FOLLOWER_BATCH_SIZE: number = 15;

export interface ITagPageProps {
    data: TagPageData;
}

export function TagPage({ data }: ITagPageProps) {
    const { currentUser } = useCurrentUser();

    function loadMoreArticles(batchIndex: number): Promise<ArticleCardType[]> {
        if (!data) return Promise.resolve([]);

        return getLatestArticlesByTag(data.Name, batchIndex, ARTICLE_BATCH_SIZE);
    }

    function loadMoreTagFollowers(batchIndex: number): Promise<UserListItemType[]> {
        if (!data) return Promise.resolve([]);

        return getTagFollowers(data?.Name, batchIndex, TAG_FOLLOWER_BATCH_SIZE);
    }

    function leftColumn() {
        return <div className="row flex-row flex-lg-column text-center text-lg-start">
            <div className="offset-xs-0 col-xs-12 offset-4 col-4 offset-lg-0 col-lg-12">
                <FollowList
                    title="Followers"
                    totalCount={data.FollowersCount}
                    initiallyVisibleItems={data.LatestFollowers}
                    loadMore={loadMoreTagFollowers}
                    itemSnippet={item => <UserListItem data={item} key={item.Link.Name} />}
                    getItemKey={item => item.Link.Name}
                />
            </div>
        </div>;
    }

    function rightColumn() {
        return <div className="d-flex flex-column gap-4">
            <AnchorList title="Top Articles" items={data.TopArticles} itemSnippet={item => (
                <li className="list-group-item p-0" key={item.ArticleId}>
                    <TopArticleLink data={item} />
                </li>
            )}>
            </AnchorList>
            <LoginCta
                ctaText="Join us"
                description="to receive personalized content and interact with the rest of the community."
            />
        </div>;
    }

    return (
        <>
            <Block classes="mb-4" header={<h1 className="h2">#{data.Name}</h1>}>
                <div className="card-body">
                    <div className="row align-items-center">
                        <div className="col-12 col-md-6">
                            {data.Description && <p>{data.Description}</p>}
                        </div>
                        <div className="col text-center">
                            <strong className="fs-4">{data.ArticlesCount}</strong>
                            <br />
                            <span>articles</span>
                        </div>
                        <div className="col text-center">
                            <strong className="fs-4">{data.FollowersCount}</strong>
                            <br />
                            <span>followers</span>
                        </div>
                        {currentUser &&
                            <div className="col text-center">
                                <FollowButton
                                    subjectName={data.Name}
                                    type="tag"
                                    isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
                                />
                            </div>
                        }
                    </div>
                </div>
            </Block>

            <ThreeColumns leftColumn={leftColumn()} rightColumn={rightColumn()} hideRightColumnOnMobile>
                {data.LatestArticles ? (
                    <InfiniteScroll
                        gap={4}
                        batchSize={ARTICLE_BATCH_SIZE}
                        loadMore={loadMoreArticles}
                        preloadedBatches={[data.LatestArticles]}
                        itemSnippet={item => <ArticleCard data={item} key={item.ArticleId} />}
                        placeholderSnippet={
                            <>
                                <ArticleCardPlaceholder />
                                <ArticleCardPlaceholder />
                                <ArticleCardPlaceholder />
                            </>}
                        outOfItemsSnippet={<OutOfArticlesBlock />}
                    />
                ) : <OutOfArticlesBlock message="No articles published yet." />}
            </ThreeColumns>
        </>);
}
