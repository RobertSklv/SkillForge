<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import ArticleCard from '$components/article/ArticleCard.svelte';
	import ArticleCardPlaceholder from '$components/article/ArticleCardPlaceholder.svelte';
	import OutOfArticlesBlock from '$components/article/OutOfArticlesBlock.svelte';
	import TopArticleLink from '$components/article/TopArticleLink.svelte';
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Block from '$components/layout/Block.svelte';
	import ThreeColumns from '$components/layout/ThreeColumns.svelte';
	import TagLink from '$components/link/TagLink.svelte';
	import UserLink from '$components/link/UserLink.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import { getLatestArticles } from '$lib/api/client';
	import { generateArticleCardItemsSchema, generateTopArticleItemsSchema, generateTopUserItemsSchema } from '$lib/structuredDataUtil';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import type HomePageData from '$lib/types/HomePageData';
	import { getFrontendUrl, getImagePath } from '$lib/util';

	const BATCH_SIZE: number = 6;

	interface Props {
		data: HomePageData;
	}

	let { data }: Props = $props();

	function loadMore(batchIndex: number): Promise<ArticleCardType[]> {
		return getLatestArticles(batchIndex, BATCH_SIZE);
	}
</script>

<svelte:head>
	<title>SkillForge | Home</title>
	<meta
		name="description"
		content="Discover and share high-quality articles on technology, development, and more. Follow tags and authors, engage with the community, and explore trending content."
	/>
	<meta name="robots" content="index,follow" />
	<link rel="canonical" href={PUBLIC_BASE_URL} />
</svelte:head>

<ThreeColumns>
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
		gap={4}
		batchSize={BATCH_SIZE}
		{loadMore}
		preloadedBatches={[data.LatestArticles]}
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

	{#snippet rightColumn()}
		<div class="d-flex flex-column gap-4">
			<AnchorList title="Top Articles" items={data.TopArticles}>
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
		mainEntityOfPage: getFrontendUrl(),
		numberOfItems: data.LatestArticles.length ?? 0,
		itemListElement: generateArticleCardItemsSchema(data.LatestArticles)
	})}
</script>`}

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'ItemList',
		numberOfItems: data.TopArticles.length ?? 0,
		itemListElement: generateTopArticleItemsSchema(data.TopArticles)
	})}
</script>`}

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'ItemList',
		numberOfItems: data.TopUsers.length ?? 0,
		itemListElement: generateTopUserItemsSchema(data.TopUsers)
	})}
</script>`}
