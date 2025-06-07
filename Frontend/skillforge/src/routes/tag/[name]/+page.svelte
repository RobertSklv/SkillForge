<script lang="ts">
	import AnchorList from "$components/anchor-list/AnchorList.svelte";
	import ArticleCard from "$components/article/ArticleCard.svelte";
	import ArticlePlaceholder from "$components/article/ArticlePlaceholder.svelte";
	import TopArticleLink from "$components/article/TopArticleLink.svelte";
	import Button from "$components/button/Button.svelte";
	import InfiniteScroll from "$components/infinite-scroll/InfiniteScroll.svelte";
	import Block from "$components/layout/Block.svelte";
	import Columns from "$components/layout/Columns.svelte";
	import LoginCta from "$components/login-cta/LoginCta.svelte";
	import UserListItem from "$components/user/UserListItem.svelte";
	import { getLatestArticlesByTag } from "$lib/api/client";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type ArticleCardType from "$lib/types/ArticleCardType";
	import type TagPageData from "$lib/types/TagPageData";
	import { onMount } from "svelte";

	const BATCH_SIZE: number = 10;

    interface Props {
        data: TagPageData
    }

    let {
        data
    }: Props = $props();

	let articlesInfiniteScrollComponent: any;

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		return getLatestArticlesByTag(data.Name, batchIndex, BATCH_SIZE);
	}

	onMount(() => {
		articlesInfiniteScrollComponent?.updateItemList(data.LatestArticles);
	});
</script>

<Block mod="mb-4">
	{#snippet header()}
		<h1 class="h2">#{data.Name}</h1>
	{/snippet}

	<div class="card-body">
		<div class="row align-items-center">
			<div class="col-6">
				{#if data.Description}
					<p>{data.Description}</p>
				{/if}
			</div>
			<div class="col text-center">
				<strong class="fs-4">{data.ArticlesCount}</strong>
				<br>
				<span>articles</span>
			</div>
			<div class="col text-center">
				<strong class="fs-4">{data.FollowersCount}</strong>
				<br>
				<span>followers</span>
			</div>
			{#if $currentUserStore}
				<div class="col-2 text-center">
					{#if data.IsFollowedByCurrentUser}
						<Button isOutline={true}>Unfollow</Button>
					{:else}
						<Button>Follow</Button>
					{/if}
				</div>
			{/if}
		</div>
	</div>
</Block>

<Columns>
	{#snippet leftColumn()}
		<ul>
			{#each data.LatestFollowers as userItem}
				<UserListItem data={userItem} />
			{/each}
		</ul>
	{/snippet}

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

	{#snippet rightColumn()}
		<div class="d-flex flex-column gap-4">
			<AnchorList title="Top Articles" items={data.TopArticles}>
				{#snippet itemSnippet(item)}
					<TopArticleLink data={item} />
				{/snippet}
			</AnchorList>
			<LoginCta ctaText="Join us" description="to receive personalized content and interact with the rest of the community." />
		</div>
	{/snippet}
</Columns>