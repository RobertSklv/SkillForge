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
		followUser,
		getLatestArticlesByAuthor,
		getUserFollowers,
		getUserFollowings,
		getUserTagFollowings,
		loadUserPage,
		unfollowUser
	} from '$lib/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type UserPageData from '$lib/types/UserPageData';
	import { page } from '$app/stores';
	import type UserListItemType from '$lib/types/UserListItemType';
	import type TagListItemType from '$lib/types/TagListItemType';
	import FollowList from '$components/user/FollowList.svelte';
	import TagListItem from '$components/tag/TagListItem.svelte';
	import FollowListPlaceholder from '$components/user/FollowListPlaceholder.svelte';
	import { getImagePath } from '$lib/util';
	import { fade } from 'svelte/transition';
	import Modal from '$components/modal/Modal.svelte';

	const ARTICLE_BATCH_SIZE: number = 4;
	const FOLLOW_LIST_BATCH_SIZE: number = 15;

	let backendData = $state<UserPageData>();
	let loadPromise = $state<Promise<UserPageData>>();
	let isFollowedByCurrentUser = $state<boolean>(false);
	let isAvatarModalOpen = $state<boolean>(false);

	$effect(() => {
		loadPromise = loadUserPage($page.params.name).then((r) => {
			backendData = r;
			isFollowedByCurrentUser = backendData.IsFollowedByCurrentUser;

			return r;
		});
	});

	function openAvatarModal() {
		isAvatarModalOpen = true;
	}

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		if (!backendData) return Promise.resolve([]);

		return getLatestArticlesByAuthor(backendData.Name, batchIndex, ARTICLE_BATCH_SIZE);
	}

	function loadMoreFollowers(batchIndex: number): Promise<UserListItemType[]> {
		if (!backendData) return Promise.resolve([]);

		return getUserFollowers(backendData?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
	}

	function loadMoreFollowings(batchIndex: number): Promise<UserListItemType[]> {
		if (!backendData) return Promise.resolve([]);

		return getUserFollowings(backendData?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
	}

	function loadMoreTagFollowings(batchIndex: number): Promise<TagListItemType[]> {
		if (!backendData) return Promise.resolve([]);

		return getUserTagFollowings(backendData?.Name, batchIndex, FOLLOW_LIST_BATCH_SIZE);
	}

	async function follow() {
		if (!backendData) return;

		await followUser(backendData.Name);

		isFollowedByCurrentUser = true;
	}

	async function unfollow() {
		if (!backendData) return;

		await unfollowUser(backendData.Name);

		isFollowedByCurrentUser = false;
	}
</script>

{#await loadPromise}
	<div transition:fade={{ duration: 1000 }}>
		<Block mod="placeholder-glow mb-4">
			{#snippet header()}
				<div class="row">
					<div class="col offset-2">
						<p class="h2">
							<span class="placeholder col-2"></span>
						</p>
					</div>
				</div>
			{/snippet}

			<div class="card-body">
				<div class="row align-items-center">
					<div class="col-2 d-flex justify-content-center">
						<div class="rounded-circle avatar-image-wrapper placeholder"></div>
					</div>
					<div class="col-4">
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
					<div class="col text-center">
						<strong class="fs-4">
							<span class="placeholder col-1"></span>
						</strong>
						<br />
						<span class="placeholder col-3"></span>
					</div>
					{#if $currentUserStore && $currentUserStore.Name != $page.params.name}
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
				<FollowListPlaceholder title="Users followed">
					{#snippet itemSnippet()}
						<UserListItemPlaceholder mod="mb-3" />
					{/snippet}
				</FollowListPlaceholder>
				<FollowListPlaceholder title="Tags followed">
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
	</div>
{:then data}
	{#if data}
		<Block mod="mb-4">
			{#snippet header()}
				<div class="row">
					<div class="col offset-2">
						<h1 class="h2">{data.Name}</h1>
					</div>
				</div>
			{/snippet}

			<div class="card-body">
				<div class="row align-items-center">
					<div class="col-2 d-flex justify-content-center">
						<button
							class="bg-transparent border-0 rounded-circle avatar-image-wrapper"
							type="button"
							onclick={openAvatarModal}
						>
							<img
								src={getImagePath(backendData?.AvatarImage)}
								alt="{backendData?.Name} profile"
								class="rounded-circle object-fit-cover w-100"
							/>
						</button>
						<Modal bind:show={isAvatarModalOpen} verticallyCentered>
							<img
								src={getImagePath(backendData?.AvatarImage)}
								alt="{backendData?.Name} profile full size"
								class="w-100 object-fit-cover"
							/>
						</Modal>
					</div>
					<div class="col-4">
						{#if data.Bio}
							<p>{data.Bio}</p>
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
					<div class="col text-center">
						<strong class="fs-4">{data.FollowingsCount}</strong>
						<br />
						<span>followings</span>
					</div>
					{#if $currentUserStore && $currentUserStore.Name != backendData?.Name}
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
					loadMore={loadMoreFollowers}
				>
					{#snippet itemSnippet(item)}
						<UserListItem mod="mb-3" data={item} />
					{/snippet}
				</FollowList>
				<FollowList
					title="Users followed"
					totalCount={data.FollowingsCount}
					initiallyVisibleItems={data.LatestFollowings}
					loadMore={loadMoreFollowings}
				>
					{#snippet itemSnippet(item)}
						<UserListItem mod="mb-3" data={item} />
					{/snippet}
				</FollowList>
				<FollowList
					title="Tags Followed"
					totalCount={data.TagFollowingsCount}
					initiallyVisibleItems={data.LatestTagFollowings}
					loadMore={loadMoreTagFollowings}
				>
					{#snippet itemSnippet(item)}
						<TagListItem mod="mb-3" data={item} />
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

<style>
	.avatar-image-wrapper {
		width: 130px;
		height: 130px;
		margin-top: -50px;
	}
</style>
