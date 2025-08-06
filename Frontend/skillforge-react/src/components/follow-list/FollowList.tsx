import { ReactNode, useState } from "react";
import { Button } from "../button/Button";
import { Modal } from "../modal/Modal";
import { InfiniteScroll } from "../infinite-scroll/InfiniteScroll";
import './_follow-list.scss';
import { ModalHeader } from "../modal-header/ModalHeader";

export interface IFollowListProps<T> {
    title: string;
    totalCount: number;
    batchSize?: number;
    initiallyVisibleItems: T[];
    loadMore: (batchIndex: number) => Promise<T[]>;
    itemSnippet: (item: T) => ReactNode;
    getItemKey: (item: T) => string;
}

export function FollowList<T>({
    title,
    totalCount,
    batchSize = 15,
    initiallyVisibleItems,
    loadMore,
    itemSnippet,
    getItemKey,
}: IFollowListProps<T>) {
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    function spinner() {
        return <div className="d-flex justify-content-center">
            <div className="spinner-border" role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
        </div>;
    }
    
    if (!totalCount) return null;

    return (
        <div className="mb-5 follow-list">
            <div className="mb-4">
                <h2 className="h4">{title}</h2>
            </div>
            <ul className="ps-0 mb-4">
                {initiallyVisibleItems.map(item => (
                    <li className="d-block mb-3" key={getItemKey(item)}>
                        {itemSnippet(item)}
                    </li>
                ))}
            </ul>
            {totalCount > initiallyVisibleItems.length && (
                <>
                    <div className="text-center">
                        <Button classes="px-4" onClick={() => setIsModalOpen(true)}>View all</Button>
                    </div>
                    <Modal
                        show={isModalOpen}
                        setShow={setIsModalOpen}
                        header={<ModalHeader title={title} />}
                        verticallyCentered
                        scrollable>
                        <InfiniteScroll<T>
                            gap={3}
                            batchSize={batchSize}
                            loadMore={loadMore}
                            itemSnippet={itemSnippet}
                            placeholderSnippet={spinner()}
                            autoLoadOnMount
                        />
                    </Modal>
                </>
            )}
        </div>
    );
}
