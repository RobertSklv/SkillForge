<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import ArticleCardPlaceholder from '$components/article/ArticleCardPlaceholder.svelte';
	import TopArticleLink from '$components/article/TopArticleLink.svelte';
	import TopArticleLinkPlaceholder from '$components/article/TopArticleLinkPlaceholder.svelte';
	import Button from '$components/button/Button.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Block from '$components/layout/Block.svelte';
	import Columns from '$components/layout/Columns.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import UserListItem from '$components/user/UserListItem.svelte';
	import UserListItemPlaceholder from '$components/user/UserListItemPlaceholder.svelte';
	import {
		followTag,
		getLatestArticlesByTag,
		getTagFollowers,
		loadTagPage,
		unfollowTag
	} from '$lib/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type TagPageData from '$lib/types/TagPageData';
	import { page } from '$app/stores';
	import type UserListItemType from '$lib/types/UserListItemType';
	import FollowList from '$components/user/FollowList.svelte';
	import FollowListPlaceholder from '$components/user/FollowListPlaceholder.svelte';

	const ARTICLE_BATCH_SIZE: number = 4;
	const TAG_FOLLOWER_BATCH_SIZE: number = 15;

	let backendData = $state<TagPageData>();
	let loadPromise = $state<Promise<TagPageData>>();
	let isFollowedByCurrentUser = $state<boolean>(false);

	$effect(() => {
		loadPromise = loadTagPage($page.params.name).then((r) => {
			backendData = r;
			isFollowedByCurrentUser = backendData.IsFollowedByCurrentUser;

			return r;
		});
	});

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		if (!backendData) return Promise.resolve([]);

		return getLatestArticlesByTag(backendData.Name, batchIndex, ARTICLE_BATCH_SIZE);
	}

	function loadMoreTagFollowers(batchIndex: number): Promise<UserListItemType[]> {
		if (!backendData) return Promise.resolve([]);

		return getTagFollowers(backendData?.Name, batchIndex, TAG_FOLLOWER_BATCH_SIZE);
	}

	async function follow() {
		if (!backendData) return;

		await followTag(backendData.Name);

		isFollowedByCurrentUser = true;
	}

	async function unfollow() {
		if (!backendData) return;

		await unfollowTag(backendData.Name);

		isFollowedByCurrentUser = false;
	}
</script>

{#await loadPromise}
	<Block mod="placeholder-glow mb-4">
		{#snippet header()}
			<p class="h2">
				<span class="placeholder col-2"></span>
			</p>
		{/snippet}

		<div class="card-body">
			<div class="row align-items-center">
				<div class="col-6">
					<p>
						<span class="placeholder col-8"></span>
					</p>
				</div>
				<div class="col text-center">
					<strong class="fs-4">
						<span class="placeholder col-1"></span>
					</strong>
					<br />
					<span class="placeholder col-3"></span>
				</div>
				<div class="col text-center">
					<strong class="fs-4">
						<span class="placeholder col-1"></span>
					</strong>
					<br />
					<span class="placeholder col-3"></span>
				</div>
				{#if $currentUserStore}
					<div class="col-2 text-center">
						<Button mod="placeholder col-5" disabled></Button>
					</div>
				{/if}
			</div>
		</div>
	</Block>

	<Columns mod="placeholder-glow">
		{#snippet leftColumn()}
			<FollowListPlaceholder title="Followers">
				{#snippet itemSnippet()}
					<UserListItemPlaceholder mod="mb-3" />
				{/snippet}
			</FollowListPlaceholder>
		{/snippet}

		<div class="d-flex flex-column gap-4">
			<ArticleCardPlaceholder />
			<ArticleCardPlaceholder />
			<ArticleCardPlaceholder />
		</div>

		{#snippet rightColumn()}
			<div class="d-flex flex-column gap-4">
				<AnchorList title="Top Articles" items={[{}, {}, {}]}>
					{#snippet itemSnippet(item)}
						<TopArticleLinkPlaceholder />
					{/snippet}
				</AnchorList>
			</div>
		{/snippet}
	</Columns>
{:then data}
	{#if data}
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
						<br />
						<span>articles</span>
					</div>
					<div class="col text-center">
						<strong class="fs-4">{data.FollowersCount}</strong>
						<br />
						<span>followers</span>
					</div>
					{#if $currentUserStore}
						<div class="col-2 text-center">
							{#if isFollowedByCurrentUser}
								<Button isOutline={true} onclick={unfollow}>Unfollow</Button>
							{:else}
								<Button onclick={follow}>Follow</Button>
							{/if}
						</div>
					{/if}
				</div>
			</div>
		</Block>

		<Columns>
			{#snippet leftColumn()}
				<FollowList
					title="Followers"
					totalCount={data.FollowersCount}
					initiallyVisibleItems={data.LatestFollowers}
					loadMore={loadMoreTagFollowers}
				>
					{#snippet itemSnippet(item)}
						<UserListItem mod="mb-3" data={item} />
					{/snippet}
				</FollowList>
			{/snippet}

			<InfiniteScroll mod="d-flex flex-column gap-4" batchSize={ARTICLE_BATCH_SIZE} {loadMore}>
				{#snippet itemSnippet(item)}
					<ArticleCard data={item} />
				{/snippet}
				{#snippet placeholderSnippet()}
					<ArticleCardPlaceholder />
					<ArticleCardPlaceholder />
					<ArticleCardPlaceholder />
				{/snippet}
			</InfiniteScroll>

			{#snippet rightColumn()}
				<div class="d-flex flex-column gap-4">
					<AnchorList title="Top Articles" items={data.TopArticles}>
						{#snippet itemSnippet(item)}
							<TopArticleLink data={item} />
						{/snippet}
					</AnchorList>
					<LoginCta
						ctaText="Join us"
						description="to receive personalized content and interact with the rest of the community."
					/>
				</div>
			{/snippet}
		</Columns>
	{/if}
{/await}
