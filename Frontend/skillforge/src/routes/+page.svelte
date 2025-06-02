<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import ArticlePlaceholder from '$components/article/ArticlePlaceholder.svelte';
	import Button from '$components/button/Button.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Block from '$components/layout/Block.svelte';
	import TagLink from '$components/link/TagLink.svelte';
	import UserLink from '$components/link/UserLink.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import { getLatestArticles } from '$lib/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type HomePageData from '$lib/types/HomePageData';
	import { onMount } from 'svelte';

	const BATCH_SIZE: number = 10;

	interface Props {
		data: HomePageData;
	}

	let { data }: Props = $props();

	let articlesInfiniteScrollComponent: any;

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		return getLatestArticles(batchIndex, BATCH_SIZE);
	}

	onMount(() => {
		articlesInfiniteScrollComponent?.updateItemList(data.LatestArticles);
	});
</script>

<div class="row">
	<div class="d-none d-sm-block col-md-3">
		<div class="d-flex flex-column gap-4">
			<AnchorList title="Top Users" items={data.TopUsers}>
				{#snippet itemSnippet(item)}
					<li class="list-group-item bg-transparent px-0">
						<UserLink data={item} />
					</li>
				{/snippet}
			</AnchorList>
			<AnchorList title="Popular Tags" items={data.TopTags}>
				{#snippet itemSnippet(item)}
					<li class="list-group-item bg-transparent px-0">
						<TagLink data={item} />
					</li>
				{/snippet}
			</AnchorList>
		</div>
	</div>
	<div class="col-12 col-md-6">
		<InfiniteScroll
			mod="d-flex flex-column gap-4"
			batchSize={BATCH_SIZE}
			scrollableElement={() => document.documentElement}
			{loadMore}
			bind:this={articlesInfiniteScrollComponent}
		>
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
	<div class="d-none d-sm-block col-md-3">
		<div class="d-flex flex-column gap-4">
			<AnchorList title="Top Articles" items={data.TopArticles}>
				{#snippet itemSnippet(item)}
					<a href="/article/{item.ArticleId}" class="list-group-item list-group-item-action" aria-label="Article '{item.Title}' link">
						<div class="d-flex w-100 justify-content-between">
							<h4 class="h5 mb-1">{item.Title}</h4>
						</div>
						<small class="text-body-tertiary">{item.ViewCount} views | {item.CommentCount} comments | 3 days ago</small>
					</a>
				{/snippet}
			</AnchorList>
			<LoginCta ctaText="Join us" description="to receive personalized content and interact with the rest of the community." />
		</div>
	</div>
</div>
