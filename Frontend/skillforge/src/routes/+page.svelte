<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import ArticleCardPlaceholder from '$components/article/ArticleCardPlaceholder.svelte';
	import TopArticleLink from '$components/article/TopArticleLink.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Columns from '$components/layout/Columns.svelte';
	import TagLink from '$components/link/TagLink.svelte';
	import UserLink from '$components/link/UserLink.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import { getLatestArticles } from '$lib/api/client';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type HomePageData from '$lib/types/HomePageData';

	const BATCH_SIZE: number = 10;

	interface Props {
		data: HomePageData;
	}

	let { data }: Props = $props();

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		return getLatestArticles(batchIndex, BATCH_SIZE);
	}
</script>

<Columns>
	{#snippet leftColumn()}
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
	{/snippet}

	<InfiniteScroll
		mod="d-flex flex-column gap-4"
		batchSize={BATCH_SIZE}
		{loadMore}
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