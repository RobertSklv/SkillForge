<script lang="ts">
	import ArticleCard from "$components/article/ArticleCard.svelte";
	import ArticlePlaceholder from "$components/article/ArticlePlaceholder.svelte";
	import InfiniteScroll from "$components/infinite-scroll/InfiniteScroll.svelte";
	import { requestApi } from "$lib/api/client";
	import type ArticleCardType from "$lib/types/ArticleCardType";

    const BATCH_SIZE: number = 10;

    function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
        return requestApi<ArticleCardType[]>(`/Article/Latest?batchIndex=${batchIndex}&batchSize=${BATCH_SIZE}`);
    }
</script>

<div class="row">
    <div class="col-12 col-md-2"></div>
    <div class="col-12 col-md-8">
        <InfiniteScroll batchSize={BATCH_SIZE} {loadMore}>
            {#snippet itemSnippet(item)}
                <ArticleCard data={item} />
            {/snippet}
            {#snippet placeholderSnippet()}
                <ArticlePlaceholder />
                <ArticlePlaceholder />
                <ArticlePlaceholder />
            {/snippet}
        </InfiniteScroll>
    </div>
</div>