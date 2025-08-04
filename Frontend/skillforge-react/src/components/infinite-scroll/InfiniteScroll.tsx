'use client'

import { useScrollableContext } from "@/context/ScrollableContext";
import { GutterLevel } from "@/lib/types/GutterLevel";
import { ReactNode, useEffect, useRef, useState } from "react";

export interface IInfiniteScrollProps<T> {
    classes?: string;
    batchSize: number;
    autoLoadOnMount?: boolean;
    preloadedBatches?: [T[]];
    listDirection?: 'row' | 'column',
    gap?: GutterLevel,
    loadMore: (batchIndex: number) => Promise<T[]>;
    itemSnippet: (item: T) => ReactNode;
    placeholderSnippet?: ReactNode;
    outOfItemsSnippet?: ReactNode;
}

export function InfiniteScroll<T>({
    classes,
    batchSize,
    autoLoadOnMount,
    preloadedBatches,
    listDirection = 'column',
    gap = 3,
    loadMore,
    itemSnippet,
    placeholderSnippet,
    outOfItemsSnippet
}: IInfiniteScrollProps<T>) {
    const [items, setItems] = useState<T[]>(preloadedBatches?.flat() ?? []);
    const [batchIndex, setBatchIndex] = useState<number>(preloadedBatches?.length ?? 0);
    const [outOfItems, setOutOfItems] = useState<boolean>(false);
    const outOfItemsRef = useRef<boolean>(outOfItems);
    const containerElement = useRef<HTMLDivElement>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const scrollable = useScrollableContext();

    function getScrollableHeight() {
        if (
            scrollable &&
            typeof scrollable.getClientHeight !== 'undefined' &&
            scrollable.getClientHeight != null
        ) {
            return scrollable.getClientHeight();
        }

        return document.documentElement.clientHeight;
    }

    function updateItemList(newItems: T[]) {
        setItems(items.concat(newItems));

        if (newItems.length < batchSize) {
            setOutOfItems(true);
            outOfItemsRef.current = true;
        }

        setBatchIndex(batchIndex + 1);
    }

    async function loadMoreAndUpdateItemList() {
        if (isLoading || outOfItemsRef.current) {
            return;
        }

        setIsLoading(true);

        let newItems = await loadMore(batchIndex);
        updateItemList(newItems);

        setIsLoading(false);
    }

    async function onScroll() {
        if (outOfItemsRef.current) {
            return;
        }

        if (containerElement?.current && containerElement.current.getBoundingClientRect().bottom <= getScrollableHeight()) {
            await loadMoreAndUpdateItemList();
        }
    }

    useEffect(() => {
        if (scrollable?.onScroll) {
            scrollable.onScroll(onScroll);
        } else {
            window.addEventListener('scroll', onScroll);
        }

        if (autoLoadOnMount) {
            loadMoreAndUpdateItemList();
        }

        return () => {
            window.removeEventListener('scroll', onScroll);
        };
    }, []);

    return (
        <div ref={containerElement} className={`d-flex flex-${listDirection} gap-${gap} ${classes}`}>
            {items.map(itemSnippet)}
            {outOfItems && outOfItemsSnippet}
            {isLoading && placeholderSnippet}
        </div>
    );
}
