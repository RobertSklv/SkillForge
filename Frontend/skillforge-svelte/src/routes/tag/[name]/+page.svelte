<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article-card/ArticleCard.svelte';
	import ArticleCardPlaceholder from '$components/article-card-placeholder/ArticleCardPlaceholder.svelte';
	import TopArticleLink from '$components/top-article-link/TopArticleLink.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Block from '$components/block/Block.svelte';
	import ThreeColumns from '$components/layout/ThreeColumns.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import UserListItem from '$components/user-list-item/UserListItem.svelte';
	import { getLatestArticlesByTag, getTagFollowers } from 'skillforge-common/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from 'skillforge-common/types/ArticleCardType';
	import type TagPageData from 'skillforge-common/types/TagPageData';
	import { page } from '$app/stores';
	import type UserListItemType from 'skillforge-common/types/UserListItemType';
	import FollowList from '$components/follow-list/FollowList.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import {
		generateArticleCardItemsSchema,
		generateTopArticleItemsSchema
	} from '$lib/structuredDataUtil';
	import FollowButton from '$components/follow-button/FollowButton.svelte';
	import OutOfArticlesBlock from '$components/out-of-articles-block/OutOfArticlesBlock.svelte';

	const ARTICLE_BATCH_SIZE: number = 4;
	const TAG_FOLLOWER_BATCH_SIZE: number = 15;

	interface Props {
		data: TagPageData;
	}

	let { data }: Props = $props();

	let backendData = $state<TagPageData>(data);

	$effect(() => {
		if ($page.params.name !== backendData?.Name) {
			backendData = data;
		}
	});

	function loadMoreArticles(batchIndex: number): Promise<ArticleCardType[]> {
		if (!backendData) return Promise.resolve([]);

		return getLatestArticlesByTag(backendData.Name, batchIndex, ARTICLE_BATCH_SIZE);
	}

	function loadMoreTagFollowers(batchIndex: number): Promise<UserListItemType[]> {
		if (!backendData) return Promise.resolve([]);

		return getTagFollowers(backendData?.Name, batchIndex, TAG_FOLLOWER_BATCH_SIZE);
	}
</script>

<svelte:head>
	<title>SkillForge | #{$page.params.name}</title>
	<meta name="description" content={backendData?.Description} />
	<meta name="robots" content="index,nofollow" />
	<link rel="canonical" href="{PUBLIC_BASE_URL}/tag/{$page.params.name}" />
</svelte:head>

<Block mod="mb-4">
	{#snippet header()}
		<h1 class="h2">#{backendData.Name}</h1>
	{/snippet}

	<div class="card-body">
		<div class="row align-items-center">
			<div class="col-6">
				{#if backendData.Description}
					<p>{backendData.Description}</p>
				{/if}
			</div>
			<div class="col text-center">
				<strong class="fs-4">{backendData.ArticlesCount}</strong>
				<br />
				<span>articles</span>
			</div>
			<div class="col text-center">
				<strong class="fs-4">{backendData.FollowersCount}</strong>
				<br />
				<span>followers</span>
			</div>
			{#if $currentUserStore}
				<div class="col text-center">
					<FollowButton
						subjectName={backendData.Name}
						type="tag"
						bind:isFollowedByCurrentUser={backendData.IsFollowedByCurrentUser}
					/>
				</div>
			{/if}
		</div>
	</div>
</Block>

<ThreeColumns hideRightColumnOnMobile>
	{#snippet leftColumn()}
		<div class="row flex-row flex-lg-column text-center text-lg-start">
			<div class="offset-xs-0 col-xs-12 offset-4 col-4 offset-lg-0 col-lg-12">
				<FollowList
					title="Followers"
					totalCount={backendData.FollowersCount}
					initiallyVisibleItems={backendData.LatestFollowers}
					loadMore={loadMoreTagFollowers}
				>
					{#snippet itemSnippet(item)}
						<UserListItem data={item} />
					{/snippet}
				</FollowList>
			</div>
		</div>
	{/snippet}

	{#key backendData.Name}
		{#if backendData.LatestArticles}
			<InfiniteScroll
				gap={4}
				batchSize={ARTICLE_BATCH_SIZE}
				loadMore={loadMoreArticles}
				preloadedBatches={[backendData.LatestArticles]}
			>
				{#snippet itemSnippet(item)}
					<ArticleCard data={item} />
				{/snippet}
				{#snippet placeholderSnippet()}
					<ArticleCardPlaceholder />
					<ArticleCardPlaceholder />
					<ArticleCardPlaceholder />
				{/snippet}
				{#snippet outOfItemsSnippet()}
					<OutOfArticlesBlock />
				{/snippet}
			</InfiniteScroll>
		{:else}
			<OutOfArticlesBlock message="No articles published yet." />
		{/if}
	{/key}

	{#snippet rightColumn()}
		<div class="d-flex flex-column gap-4">
			<AnchorList title="Top Articles" items={backendData.TopArticles}>
				{#snippet itemSnippet(item)}
					<li class="list-group-item p-0">
						<TopArticleLink data={item} />
					</li>
				{/snippet}
			</AnchorList>
			<LoginCta
				ctaText="Join us"
				description="to receive personalized content and interact with the rest of the community."
			/>
		</div>
	{/snippet}
</ThreeColumns>

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'ItemList',
		numberOfItems: backendData.LatestArticles.length ?? 0,
		itemListElement: generateArticleCardItemsSchema(backendData.LatestArticles)
	})}
</script>`}

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'ItemList',
		numberOfItems: backendData.TopArticles.length ?? 0,
		itemListElement: generateTopArticleItemsSchema(backendData.TopArticles)
	})}
</script>`}
