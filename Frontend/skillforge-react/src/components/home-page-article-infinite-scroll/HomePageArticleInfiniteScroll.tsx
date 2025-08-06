'use client'

import { getLatestArticles } from '@/lib/api/client';
import ArticleCardType from '@/lib/types/ArticleCardType';
import * as React from 'react';
import { InfiniteScroll } from '../infinite-scroll/InfiniteScroll';
import { ArticleCard } from '../article-card/ArticleCard';
import { ArticleCardPlaceholder } from '../article-card-placeholder/ArticleCardPlaceholder';
import { OutOfArticlesBlock } from '../out-of-articles-block/OutOfArticlesBlock';
import HomePageData from '@/lib/types/HomePageData';

export interface IHomePageArticleInfiniteScrollProps {
    data: HomePageData;
}

export function HomePageArticleInfiniteScroll ({ data }: IHomePageArticleInfiniteScrollProps) {
    const BATCH_SIZE: number = 6;

    function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
        return getLatestArticles(batchIndex, BATCH_SIZE);
    }

  return (
    <InfiniteScroll<ArticleCardType>
        gap={4}
        batchSize={BATCH_SIZE}
        loadMore={loadMore}
        preloadedBatches={[data.LatestArticles]}
        itemSnippet={item => <ArticleCard data={item} key={item.ArticleId} />}
        placeholderSnippet={(
                    <>
                        <ArticleCardPlaceholder />
                        <ArticleCardPlaceholder />
                        <ArticleCardPlaceholder />
                    </>
                )}
        outOfItemsSnippet={<OutOfArticlesBlock />}
    />
  );
}
