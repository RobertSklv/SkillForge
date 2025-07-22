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
	import {
		getLatestArticlesByAuthor,
		getUserFollowers,
		getUserFollowings,
		getUserTagFollowings
	} from 'skillforge-common/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from 'skillforge-common/types/ArticleCardType';
	import type UserPageData from 'skillforge-common/types/UserPageData';
	import { page } from '$app/stores';
	import type UserListItemType from 'skillforge-common/types/UserListItemType';
	import type TagListItemType from 'skillforge-common/types/TagListItemType';
	import FollowList from '$components/follow-list/FollowList.svelte';
	import TagListItem from '$components/tag-list-item/TagListItem.svelte';
	import { getFrontendUrl, getImagePath } from 'skillforge-common/util';
	import Modal from '$components/modal/Modal.svelte';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownItem from '$components/dropdown-item/DropdownItem.svelte';
	import DropdownDivider from '$components/dropdown-divider/DropdownDivider.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import {
		generateArticleCardItemsSchema,
		generateTopArticleItemsSchema
	} from '$lib/structuredDataUtil';
	import FollowButton from '$components/follow-button/FollowButton.svelte';
	import OutOfArticlesBlock from '$components/out-of-articles-block/OutOfArticlesBlock.svelte';
	import Icon from '$components/icon/Icon.svelte';
	import ReportModal from '$components/report/ReportModal.svelte';

	const ARTICLE_BATCH_SIZE: number = 4;
	const FOLLOW_LIST_BATCH_SIZE: number = 15;

	interface Props {
		data: UserPageData;
	}

	let { data }: Props = $props();

	let backendData = $state<UserPageData>(data);
	let isAvatarModalOpen = $state<boolean>(false);
	let showReportModal = $state<boolean>(false);

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
			<div class="col offset-0 offset-lg-2">
				<h1 class="h2">{backendData.Name}</h1>
			</div>
			{#if $currentUserStore}
				<div class="col-3 text-end">
					<Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start">
						{#snippet buttonSnippet()}
							<Icon type="three-dots-vertical" />
						{/snippet}
						{#if $currentUserStore.Name == backendData.Name}
							<DropdownItem href="/account">
								<Icon type="pencil-square" />
								Edit
							</DropdownItem>
						{:else}
							<DropdownItem type="button" onclick={() => (showReportModal = true)}>
								<Icon type="exclamation-triangle" />
								Report
							</DropdownItem>
						{/if}
					</Dropdown>
				</div>
			{/if}
		</div>
	{/snippet}

	<div class="card-body">
		<div class="row align-items-center">
			<div class="col-4 col-lg-2 d-flex justify-content-center">
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
			<div class="col-8 col-lg-4">
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
				<div class="col text-center">
					<FollowButton
						subjectName={backendData.Name}
						type="user"
						isFollowedByCurrentUser={backendData.IsFollowedByCurrentUser}
					/>
				</div>
			{/if}
		</div>
	</div>
</Block>

<ThreeColumns hideRightColumnOnMobile>
	{#snippet leftColumn()}
		<div class="row flex-row flex-xl-column text-center text-xl-start">
			<div class="col-xs-12 col-4 col-xl-12">
				<FollowList
					title="Followers"
					totalCount={backendData.FollowersCount}
					initiallyVisibleItems={backendData.LatestFollowers}
					loadMore={loadMoreFollowers}
				>
					{#snippet itemSnippet(item)}
						<UserListItem data={item} />
					{/snippet}
				</FollowList>
			</div>
			<div class="col-xs-12 col-4 col-xl-12">
				<FollowList
					title="Users followed"
					totalCount={backendData.FollowingsCount}
					initiallyVisibleItems={backendData.LatestFollowings}
					loadMore={loadMoreFollowings}
				>
					{#snippet itemSnippet(item)}
						<UserListItem data={item} />
					{/snippet}
				</FollowList>
			</div>
			<div class="col-xs-12 col-4 col-xl-12">
				<FollowList
					title="Tags Followed"
					totalCount={backendData.TagFollowingsCount}
					initiallyVisibleItems={backendData.LatestTagFollowings}
					loadMore={loadMoreTagFollowings}
				>
					{#snippet itemSnippet(item)}
						<TagListItem data={item} />
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
			{#if backendData.TopArticles.length > 0}
				<AnchorList title="Top Articles" items={backendData.TopArticles}>
					{#snippet itemSnippet(item)}
						<li class="list-group-item p-0">
							<TopArticleLink data={item} />
						</li>
					{/snippet}
				</AnchorList>
			{/if}
			<LoginCta
				ctaText="Join us"
				description="to receive personalized content and interact with the rest of the community."
			/>
		</div>
	{/snippet}
</ThreeColumns>

<ReportModal
	entityName={backendData.Name}
	action="/UserReport/Create"
	bind:show={showReportModal}
/>

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'Person',
		mainEntityOfPage: getFrontendUrl('/user/' + backendData.Name),
		name: backendData.Name,
		description: backendData.Bio,
		image: getImagePath(backendData.AvatarImage)
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
	}

	@media (min-width: 992px) {
		.avatar-image-wrapper {
			margin-top: -50px;
		}
	}
</style>
