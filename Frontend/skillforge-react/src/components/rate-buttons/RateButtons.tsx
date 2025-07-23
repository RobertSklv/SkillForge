import { Icon } from '@components/icon/Icon';
import { useState } from 'react';
import { getRating, rate } from 'skillforge-common/api/client';
import type RatingData from 'skillforge-common/types/RatingData';

const RATE_ITEMS_BATCH_SIZE = 15;

export interface IRateButtonsProps {
    data: RatingData;
    subjectId: number;
    type: 'article' | 'comment';
    size?: 'normal' | 'small';
    readonly?: boolean;
}

export function RateButtons({ data, subjectId, type, size = 'normal', readonly }: IRateButtonsProps) {
    const [thumbsUp, setThumbsUp] = useState<number>(data.ThumbsUp);
    const [thumbsDown, setThumbsDown] = useState<number>(data.ThumbsDown);
    const [currentUserRate, setCurrentUserRate] = useState(data.UserRating);
    const [isThumbsUpModalOpen, setIsThumbsUpModalOpen] = useState<boolean>(false);
    const [isThumbsDownModalOpen, setIsThumbsDownModalOpen] = useState<boolean>(false);

    function toggleThumbsUp() {
        let newRate: -1 | 0 | 1 = 0;

        if (currentUserRate != 1) {
            newRate = 1;
        }

        rate(subjectId, newRate, type).then(() => {
            if (currentUserRate == 1) {
                setThumbsUp(thumbsUp - 1);
            } else {
                setThumbsUp(thumbsUp + 1);
            }

            if (currentUserRate == -1) {
                setThumbsDown(thumbsDown - 1);
            }

            setCurrentUserRate(newRate);
        });
    }

    function toggleThumbsDown() {
        let newRate: -1 | 0 | 1 = 0;

        if (currentUserRate != -1) {
            newRate = -1;
        }

        rate(subjectId, newRate, type).then(() => {
            if (currentUserRate == -1) {
                setThumbsDown(thumbsDown - 1);
            } else {
                setThumbsDown(thumbsDown + 1);
            }

            if (currentUserRate == 1) {
                setThumbsUp(thumbsUp - 1);
            }

            setCurrentUserRate(newRate);
        });
    }

    function loadMoreRates(batchIndex: number, rateType: 'positive' | 'negative') {
        return getRating(subjectId, rateType, type, batchIndex, RATE_ITEMS_BATCH_SIZE);
    }

    return (
        <div className="text-end">
            <button
                type="button"
                className="rate-btn rate-btn__positive bg-transparent border-0 text-primary pe-0"
                aria-label="Thumbs up"
                title="Thumbs up"
                onClick={toggleThumbsUp}
                disabled={readonly}
            >
                <Icon
                    type="hand-thumbs-up{currentUserRate == 1 ? '-fill' : ''}"
                    classes={size === 'normal' ? 'fs-3' : 'fs-6'}
                />
            </button>
            <button
                type="button"
                aria-label="View positive raters"
                title="View positive raters"
                onClick={() => (setIsThumbsUpModalOpen(true))}
                className="bg-transparent border-0 text-primary {size === 'normal' ? 'fs-5' : 'fs-6 p-0'}"
                disabled={data.ThumbsUp === 0 || readonly}
            >
                {thumbsUp}
            </button>
            {/* {(data.ThumbsUp > 0 && !readonly) &&
            <Modal bind:show={isThumbsUpModalOpen} verticallyCentered scrollable>
                {#snippet header()}
                    <ModalHeader title="Thumbs up"></ModalHeader>
                {/snippet}
                <InfiniteScroll
                    batchSize={RATE_ITEMS_BATCH_SIZE}
                    loadMore={(i) => loadMoreRates(i, 'positive')}
                    autoLoadOnMount
                >
                    {#snippet itemSnippet(item)}
                        <UserListItem data={item} />
                    {/snippet}
                    {#snippet placeholderSnippet()}
                        <div className="d-flex justify-content-center">
                            <div className="spinner-border" role="status">
                                <span className="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    {/snippet}
                </InfiniteScroll>
            </Modal>
        } */}

            <button
                type="button"
                className="rate-btn rate-btn__negative bg-transparent border-0 text-primary pe-0 ms-2"
                aria-label="Thumbs down"
                title="Thumbs down"
                onClick={toggleThumbsDown}
                disabled={readonly}
            >
                <Icon
                    type="hand-thumbs-down{currentUserRate == -1 ? '-fill' : ''}"
                    classes={size === 'normal' ? 'fs-3' : 'fs-6'}
                />
            </button>
            <button
                type="button"
                aria-label="View negative raters"
                title="View negative raters"
                onClick={() => (setIsThumbsDownModalOpen(true))}
                className="bg-transparent border-0 text-primary {size === 'normal' ? 'fs-5' : 'fs-6 p-0'}"
                disabled={data.ThumbsDown === 0 || readonly}
            >
                {thumbsDown}
            </button>
            {/* {(data.ThumbsDown > 0 && !readonly) &&
            <Modal bind:show={isThumbsDownModalOpen} verticallyCentered scrollable>
                {#snippet header()}
                    <ModalHeader title="Thumbs down"></ModalHeader>
                {/snippet}
                <InfiniteScroll
                    batchSize={RATE_ITEMS_BATCH_SIZE}
                    loadMore={(i) => loadMoreRates(i, 'negative')}
                    autoLoadOnMount
                >
                    {#snippet itemSnippet(item)}
                        <UserListItem data={item} />
                    {/snippet}
                    {#snippet placeholderSnippet()}
                        <div className="d-flex justify-content-center">
                            <div className="spinner-border" role="status">
                                <span className="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    {/snippet}
                </InfiniteScroll>
            </Modal>
        } */}
        </div>
    );
}
