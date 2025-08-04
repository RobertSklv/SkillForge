'use client';

import { Icon } from '@/components/icon/Icon';
import { useState } from 'react';
import { getRating, rate } from '@/lib/api/client';
import type RatingData from '@/lib/types/RatingData';
import styles from './RateButtons.module.scss';
import { Modal } from '../modal/Modal';
import { ModalHeader } from '../modal-header/ModalHeader';
import { InfiniteScroll } from '../infinite-scroll/InfiniteScroll';
import { UserListItem } from '../user-list-item/UserListItem';
import UserListItemType from '@/lib/types/UserListItemType';

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

    function loadMoreRates(batchIndex: number, rateType: 'positive' | 'negative'): Promise<UserListItemType[]> {
        return getRating(subjectId, rateType, type, batchIndex, RATE_ITEMS_BATCH_SIZE);
    }

    return (
        <div className="text-end">
            <button
                type="button"
                className={`${styles['rate-btn']} ${styles['rate-btn__positive']} bg-transparent border-0 text-primary pe-0`}
                aria-label="Thumbs up"
                title="Thumbs up"
                onClick={toggleThumbsUp}
                disabled={readonly}
            >
                <Icon
                    type={`hand-thumbs-up${currentUserRate == 1 ? '-fill' : ''}`}
                    classes={size === 'normal' ? 'fs-3' : 'fs-6'}
                />
            </button>
            <button
                type="button"
                aria-label="View positive raters"
                title="View positive raters"
                onClick={() => (setIsThumbsUpModalOpen(true))}
                className={`bg-transparent border-0 text-primary ${size === 'normal' ? 'fs-5' : 'fs-6 p-0'}`}
                disabled={data.ThumbsUp === 0 || readonly}
            >
                {thumbsUp}
            </button>
            {(data.ThumbsUp > 0 && !readonly) &&
                <Modal
                    show={isThumbsUpModalOpen}
                    setShow={setIsThumbsUpModalOpen}
                    header={<ModalHeader title="Thumbs up"></ModalHeader>}
                    verticallyCentered
                    scrollable
                >
                    <InfiniteScroll<UserListItemType>
                        batchSize={RATE_ITEMS_BATCH_SIZE}
                        loadMore={(i) => loadMoreRates(i, 'positive')}
                        itemSnippet={item => <UserListItem data={item} key={item.Link.Name} />}
                        placeholderSnippet={(
                            <div className="d-flex justify-content-center">
                                <div className="spinner-border" role="status">
                                    <span className="visually-hidden">Loading...</span>
                                </div>
                            </div>
                        )}
                        autoLoadOnMount
                    />
                </Modal>
            }

            <button
                type="button"
                className={`${styles['rate-btn']} ${styles['rate-btn__negative']} bg-transparent border-0 text-primary pe-0 ms-2`}
                aria-label="Thumbs down"
                title="Thumbs down"
                onClick={toggleThumbsDown}
                disabled={readonly}
            >
                <Icon
                    type={`hand-thumbs-down${currentUserRate == -1 ? '-fill' : ''}`}
                    classes={size === 'normal' ? 'fs-3' : 'fs-6'}
                />
            </button>
            <button
                type="button"
                aria-label="View negative raters"
                title="View negative raters"
                onClick={() => (setIsThumbsDownModalOpen(true))}
                className={`bg-transparent border-0 text-primary ${size === 'normal' ? 'fs-5' : 'fs-6 p-0'}`}
                disabled={data.ThumbsDown === 0 || readonly}
            >
                {thumbsDown}
            </button>
            {(data.ThumbsDown > 0 && !readonly) &&
                <Modal
                    show={isThumbsDownModalOpen}
                    setShow={setIsThumbsDownModalOpen}
                    header={<ModalHeader title="Thumbs down"></ModalHeader>}
                    verticallyCentered
                    scrollable
                >
                    <InfiniteScroll<UserListItemType>
                        batchSize={RATE_ITEMS_BATCH_SIZE}
                        loadMore={(i) => loadMoreRates(i, 'negative')}
                        itemSnippet={item => <UserListItem data={item} key={item.Link.Name} />}
                        placeholderSnippet={(
                            <div className="d-flex justify-content-center">
                                <div className="spinner-border" role="status">
                                    <span className="visually-hidden">Loading...</span>
                                </div>
                            </div>
                        )}
                        autoLoadOnMount
                    />
                </Modal>
            }
        </div>
    );
}
