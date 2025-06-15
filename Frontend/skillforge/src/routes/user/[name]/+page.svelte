<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import ArticleCardPlaceholder from '$components/article/ArticleCardPlaceholder.svelte';
	import TopArticleLink from '$components/article/TopArticleLink.svelte';
	import Button from '$components/button/Button.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Block from '$components/layout/Block.svelte';
	import ThreeColumns from '$components/layout/ThreeColumns.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import UserListItem from '$components/user/UserListItem.svelte';
	import {
		followUser,
		getLatestArticlesByAuthor,
		getUserFollowers,
		getUserFollowings,
		getUserTagFollowings,
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
	import { getFrontendUrl, getImagePath } from '$lib/util';
	import Modal from '$components/modal/Modal.svelte';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownItem from '$components/dropdown/DropdownItem.svelte';
	import DropdownDivider from '$components/dropdown/DropdownDivider.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import { generateArticleCardItemsSchema, generateTopArticleItemsSchema } from '$lib/structuredDataUtil';

	const ARTICLE_BATCH_SIZE: number = 4;
	const FOLLOW_LIST_BATCH_SIZE: number = 15;

	interface Props {
		data: UserPageData;
	}

	let {
		data
	}: Props = $props();

	let backendData = $state<UserPageData>(data);
	let isAvatarModalOpen = $state<boolean>(false);

	$effect(() => {
		if ($page.params.name !== backendData?.Name) {
			backendData = data;
		}
	});

	function openAvatarModal() {
		isAvatarModalOpen = true;
	}

	function loadMoreArticles(batchIndex: number): Promise<ArticleCardType[]> {
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

		backendData.IsFollowedByCurrentUser = true;
	}

	async function unfollow() {
		if (!backendData) return;

		await unfollowUser(backendData.Name);

		backendData.IsFollowedByCurrentUser = false;
	}
</script>

<svelte:head>
	<title>SkillForge | {$page.params.name}</title>
	<meta name="description" content={backendData?.Bio ?? `${$page.params.name}'s profile'`} />
	<meta name="robots" content="index,follow" />
	<link rel="canonical" href="{PUBLIC_BASE_URL}/user/{$page.params.name}" />
</svelte:head>

<Block mod="mb-4">
	{#snippet header()}
		<div class="row">
			<div class="col offset-2">
				<h1 class="h2">{backendData.Name}</h1>
			</div>
			<div class="col-2 text-end">
				<Dropdown>
					{#snippet buttonSnippet()}
						<i class="bi bi-three-dots-vertical"></i>
					{/snippet}
					{#if $currentUserStore && $currentUserStore.Name == backendData.Name}
						<DropdownItem href="/account">
							<i class="bi bi-pencil-square"></i>
							Edit
						</DropdownItem>
						<DropdownDivider />
					{/if}
					<DropdownItem href="/">
						<i class="bi bi-exclamation-triangle"></i>
						Report
					</DropdownItem>
				</Dropdown>
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
						src={getImagePath(backendData.AvatarImage)}
						alt="{backendData.Name} profile"
						class="round-image w-100"
					/>
				</button>
				<Modal bind:show={isAvatarModalOpen} verticallyCentered>
					<img
						src={getImagePath(backendData.AvatarImage)}
						alt="{backendData.Name} profile full size"
						class="w-100 object-fit-cover"
					/>
				</Modal>
			</div>
			<div class="col-4">
				{#if backendData.Bio}
					<p>{backendData.Bio}</p>
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
			<div class="col text-center">
				<strong class="fs-4">{backendData.FollowingsCount}</strong>
				<br />
				<span>followings</span>
			</div>
			{#if $currentUserStore && $currentUserStore.Name != backendData.Name}
				<div class="col-2 text-center">
					{#if backendData.IsFollowedByCurrentUser}
						<Button isOutline={true} onclick={unfollow}>Unfollow</Button>
					{:else}
						<Button onclick={follow}>Follow</Button>
					{/if}
				</div>
			{/if}
		</div>
	</div>
</Block>

<ThreeColumns>
	{#snippet leftColumn()}
		<FollowList
			title="Followers"
			totalCount={backendData.FollowersCount}
			initiallyVisibleItems={backendData.LatestFollowers}
			loadMore={loadMoreFollowers}
		>
			{#snippet itemSnippet(item)}
				<UserListItem mod="mb-3" data={item} />
			{/snippet}
		</FollowList>
		<FollowList
			title="Users followed"
			totalCount={backendData.FollowingsCount}
			initiallyVisibleItems={backendData.LatestFollowings}
			loadMore={loadMoreFollowings}
		>
			{#snippet itemSnippet(item)}
				<UserListItem mod="mb-3" data={item} />
			{/snippet}
		</FollowList>
		<FollowList
			title="Tags Followed"
			totalCount={backendData.TagFollowingsCount}
			initiallyVisibleItems={backendData.LatestTagFollowings}
			loadMore={loadMoreTagFollowings}
		>
			{#snippet itemSnippet(item)}
				<TagListItem mod="mb-3" data={item} />
			{/snippet}
		</FollowList>
	{/snippet}

	{#key backendData.Name}
		<InfiniteScroll
			mod="d-flex flex-column gap-4"
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
		</InfiniteScroll>
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
		'@type': 'Person',
		mainEntityOfPage: getFrontendUrl('/user/' + backendData.Name),
		name: backendData.Name,
		description: backendData.Bio,
		image: getImagePath(backendData.AvatarImage),
	})}
</script>`}

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

<style>
	.avatar-image-wrapper {
		width: 130px;
		height: 130px;
		margin-top: -50px;
	}
</style>
