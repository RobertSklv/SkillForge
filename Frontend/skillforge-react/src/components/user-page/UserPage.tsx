'use client';

import { AnchorList } from '@/components/anchor-list/AnchorList';
import { Block } from '@/components/block/Block';
import { DropdownItem } from '@/components/dropdown-item/DropdownItem';
import { Dropdown } from '@/components/dropdown/Dropdown';
import { Icon } from '@/components/icon/Icon';
import { ThreeColumns } from '@/components/layout/three-columns/ThreeColumns';
import { LoginCta } from '@/components/login-cta/LoginCta';
import { TopArticleLink } from '@/components/top-article-link/TopArticleLink';
import { useCurrentUser } from '@/hooks/useCurrentUser';
import { getLatestArticlesByAuthor, getUserFollowers, getUserFollowings, getUserTagFollowings } from '@/lib/api/client';
import ArticleCardType from '@/lib/types/ArticleCardType';
import TagListItemType from '@/lib/types/TagListItemType';
import UserListItemType from '@/lib/types/UserListItemType';
import UserPageData from '@/lib/types/UserPageData';
import { getImagePath } from '@/lib/util';
import { useState } from 'react';
import { InfiniteScroll } from '../infinite-scroll/InfiniteScroll';
import { ArticleCard } from '../article-card/ArticleCard';
import { ArticleCardPlaceholder } from '../article-card-placeholder/ArticleCardPlaceholder';
import { OutOfArticlesBlock } from '../out-of-articles-block/OutOfArticlesBlock';
import { FollowList } from '../follow-list/FollowList';
import { UserListItem } from '../user-list-item/UserListItem';
import { TagListItem } from '../tag-list-item/TagListItem';
import { Modal } from '../modal/Modal';
import { FollowButton } from '../follow-button/FollowButton';
import styles from './UserPage.module.css';
import { ReportModal } from '../report-modal/ReportModal';

const ARTICLE_BATCH_SIZE: number = 4;
const FOLLOW_LIST_BATCH_SIZE: number = 15;

export interface IUserPageProps {
    data: UserPageData;
}

export function UserPage({ data }: IUserPageProps) {
    const { currentUser } = useCurrentUser();
    const [isAvatarModalOpen, setIsAvatarModalOpen] = useState<boolean>(false);
    const [showReportModal, setShowReportModal] = useState<boolean>(false);

    function header() {
        return <div className="row">
            <div className="col offset-0 offset-lg-2">
                <h1 className="h2">{data.Name}</h1>
            </div>
            {currentUser &&
                <div className="col-3 text-end">
                    <Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start" buttonSnippet={<Icon type="three-dots-vertical" />} hideChevron>
                        {currentUser.Name == data.Name ? (
                            <DropdownItem href="/account">
                                <Icon type="pencil-square" classes='me-2' />
                                Edit
                            </DropdownItem>
                        ) : (
                            <DropdownItem type="button" onClick={() => setShowReportModal(true)}>
                                <Icon type="exclamation-triangle" classes='me-2' />
                                Report
                            </DropdownItem>
                        )}
                    </Dropdown>
                </div>
            }
        </div>;
    }

    function leftColumn() {
        return <div className="row flex-row flex-xl-column text-center text-xl-start">
            <div className="col-xs-12 col-4 col-xl-12">
                <FollowList
                    title="Followers"
                    totalCount={data.FollowersCount}
                    initiallyVisibleItems={data.LatestFollowers}
                    loadMore={loadMoreFollowers}
                    itemSnippet={item => <UserListItem data={item} key={item.Link.Name} />}
                    getItemKey={item => item.Link.Name}
                />
            </div>
            <div className="col-xs-12 col-4 col-xl-12">
                <FollowList
                    title="Users followed"
                    totalCount={data.FollowingsCount}
                    initiallyVisibleItems={data.LatestFollowings}
                    loadMore={loadMoreFollowings}
                    itemSnippet={item => <UserListItem data={item} key={item.Link.Name} />}
                    getItemKey={item => item.Link.Name}
                />
            </div>
            <div className="col-xs-12 col-4 col-xl-12">
                <FollowList
                    title="Tags Followed"
                    totalCount={data.TagFollowingsCount}
                    initiallyVisibleItems={data.LatestTagFollowings}
                    loadMore={loadMoreTagFollowings}
                    itemSnippet={item => <TagListItem data={item} key={item.Link.Name} />}
                    getItemKey={item => item.Link.Name}
                />
            </div>
        </div>;
    }

    function rightColumn() {
        return <div className="d-flex flex-column gap-4">
            {data.TopArticles.length > 0 &&
                <AnchorList title="Top Articles" items={data.TopArticles} itemSnippet={item => (
                    <li className="list-group-item p-0" key={item.ArticleId}>
                        <TopArticleLink data={item} />
                    </li>
                )} />
            }
            <LoginCta
                ctaText="Join us"
                description="to receive personalized content and interact with the rest of the community."
            />
        </div>;
    }

    function openAvatarModal() {
        setIsAvatarModalOpen(true);
    }

    function loadMoreArticles(batchIndex: number): Promise<ArticleCardType[]> {
        if (!data) return Promise.resolve([]);

        return getLatestArticlesByAuthor(data.Name, batchIndex, ARTICLE_BATCH_SIZE);
    }

    function loadMoreFollowers(batchIndex: number): Promise<UserListItemType[]> {
        if (!data) return Promise.resolve([]);

        return getUserFollowers(data?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
    }

    function loadMoreFollowings(batchIndex: number): Promise<UserListItemType[]> {
        if (!data) return Promise.resolve([]);

        return getUserFollowings(data?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
    }

    function loadMoreTagFollowings(batchIndex: number): Promise<TagListItemType[]> {
        if (!data) return Promise.resolve([]);

        return getUserTagFollowings(data?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
    }

    return (
        <>
            <Block classes="mb-4" header={header()}>
                <div className="card-body">
                    <div className="row align-items-center">
                        <div className="col-12 col-md-4 col-lg-2 d-flex justify-content-center">
                            <button
                                className={`bg-transparent border-0 rounded-circle ${styles['avatar-image-wrapper']}`}
                                type="button"
                                onClick={openAvatarModal}
                            >
                                <img
                                    src={getImagePath(data.AvatarImage)}
                                    alt={`${data.Name} profile`}
                                    className="round-image w-100"
                                />
                            </button>
                            <Modal show={isAvatarModalOpen} setShow={setIsAvatarModalOpen} verticallyCentered>
                                <img
                                    src={getImagePath(data.AvatarImage)}
                                    alt={`${data.Name} profile full size`}
                                    className="w-100 object-fit-cover"
                                />
                            </Modal>
                        </div>
                        <div className="col-12 col-md-8 col-lg-4 text-center text-md-start mt-3 mt-md-0">
                            {data.Bio && <p>{data.Bio}</p>}
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
                        <div className="col text-center">
                            <strong className="fs-4">{data.FollowingsCount}</strong>
                            <br />
                            <span>followings</span>
                        </div>
                        {(currentUser && currentUser.Name != data.Name) && (
                            <div className="col text-center">
                                <FollowButton
                                    subjectName={data.Name}
                                    type="user"
                                    isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
                                />
                            </div>
                        )}
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

            <ReportModal
                title="Report user"
                entityName={data.Name}
                action="/UserReport/Create"
                show={showReportModal}
                setShow={setShowReportModal}
            />
        </>
    );
}
