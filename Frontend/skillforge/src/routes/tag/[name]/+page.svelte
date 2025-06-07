<script lang="ts">
	import AnchorList from "$components/anchor-list/AnchorList.svelte";
	import ArticleCard from "$components/article/ArticleCard.svelte";
	import ArticleCardPlaceholder from "$components/article/ArticleCardPlaceholder.svelte";
	import TopArticleLink from "$components/article/TopArticleLink.svelte";
	import TopArticleLinkPlaceholder from "$components/article/TopArticleLinkPlaceholder.svelte";
	import Button from "$components/button/Button.svelte";
	import InfiniteScroll from "$components/infinite-scroll/InfiniteScroll.svelte";
	import Block from "$components/layout/Block.svelte";
	import Columns from "$components/layout/Columns.svelte";
	import LoginCta from "$components/login-cta/LoginCta.svelte";
	import UserListItem from "$components/user/UserListItem.svelte";
	import UserListItemPlaceholder from "$components/user/UserListItemPlaceholder.svelte";
	import { followTag, getLatestArticlesByTag, loadTagPage, unfollowTag } from "$lib/api/client";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type ArticleCardType from "$lib/types/ArticleCardType";
	import type TagPageData from "$lib/types/TagPageData";
	import { onMount } from "svelte";

	const BATCH_SIZE: number = 10;

    interface Props {
		data: {
			tagName: string
		}
    }

    let {
        data
    }: Props = $props();

	let backendData = $state<TagPageData>();
	let loadPromise = $state<Promise<TagPageData>>();
	let isFollowedByCurrentUser = $state<boolean>(false);

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		return getLatestArticlesByTag(data.tagName, batchIndex, BATCH_SIZE);
	}

	async function follow() {
		await followTag(data.tagName);

		isFollowedByCurrentUser = true;
	}

	async function unfollow() {
		await unfollowTag(data.tagName);
		
		isFollowedByCurrentUser = false;
	}

	onMount(async () => {
		loadPromise = loadTagPage(data.tagName)
			.then(r => {
				backendData = r;
				isFollowedByCurrentUser = backendData.IsFollowedByCurrentUser;

				return r;
			});
	});
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
					<br>
					<span class="placeholder col-3"></span>
				</div>
				<div class="col text-center">
					<strong class="fs-4">
						<span class="placeholder col-1"></span>
					</strong>
					<br>
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
			<div class="mb-4">
				<h2 class="h4">Followers</h2>
			</div>
			<ul class="ps-0">
				<UserListItemPlaceholder mod="mb-3" />
				<UserListItemPlaceholder mod="mb-3" />
				<UserListItemPlaceholder mod="mb-3" />
				<UserListItemPlaceholder mod="mb-3" />
				<UserListItemPlaceholder mod="mb-3" />
				<UserListItemPlaceholder mod="mb-3" />
			</ul>
			<div class="text-center">
				<Button mod="px-4">View all</Button>
			</div>
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
				<div class="mb-4">
					<h2 class="h4">Followers</h2>
				</div>
				<ul class="ps-0 mb-4">
					{#each data.LatestFollowers as userItem}
						<UserListItem data={userItem} mod="mb-3" />
					{/each}
				</ul>
				{#if data.FollowersCount > data.LatestFollowers.length}
					<div class="text-center">
						<Button mod="px-4">View all</Button>
					</div>
				{/if}
			{/snippet}

			<InfiniteScroll
				mod="d-flex flex-column gap-4"
				batchSize={BATCH_SIZE}
				scrollableElement={() => document.documentElement}
				{loadMore}
				autoLoadOnMount
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
	{/if}
{/await}
