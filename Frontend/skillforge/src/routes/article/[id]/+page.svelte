<script lang="ts">
	import AnchorList from '$components/anchor-list/AnchorList.svelte';
	import Article from '$components/article/Article.svelte';
	import TopArticleLink from '$components/article/TopArticleLink.svelte';
	import TwoColumns from '$components/layout/TwoColumns.svelte';
	import AuthorBlock from '$components/user/AuthorBlock.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import { generateUserLinkSchema } from '$lib/structuredDataUtil';
	import type ArticlePageModel from '$lib/types/ArticlePageModel';
	import { getFrontendUrl, getImagePath, htmlToText, truncateText } from '$lib/util';

	interface Props {
		data: ArticlePageModel;
	}

	let { data }: Props = $props();
</script>

<svelte:head>
	<title>SkillForge | {data.Title}</title>
	<meta
		name="description"
		content="{truncateText(htmlToText(data.Content), 120)} Tags: {data.Tags.join(', ')}"
	/>
	<meta name="robots" content="index,follow" />
	<link rel="canonical" href="{PUBLIC_BASE_URL}/article/{data.ArticleId}" />
</svelte:head>

<TwoColumns>
	{#snippet leftColumn()}
		<AuthorBlock data={data.Author} mod="mb-4" />

		{#if data.LatestArticlesByAuthor.length > 0}
			<AnchorList title="Latest by author" items={data.LatestArticlesByAuthor}>
				{#snippet itemSnippet(item)}
					<TopArticleLink data={item} />
				{/snippet}
			</AnchorList>
		{/if}
	{/snippet}

	<Article {data} />
</TwoColumns>

{@html `<script type="application/ld+json">
	${JSON.stringify({
		'@context': 'https://schema.org',
		'@type': 'Article',
		author: generateUserLinkSchema(data.Author.Link),
		mainEntityOfPage: getFrontendUrl('/article/' + data.ArticleId),
		articleBody: truncateText(htmlToText(data.Content), 400),
		datePublished: data.DatePublished,
		headline: data.Title,
		image: data.CoverImage ? getImagePath(data.CoverImage) : '',
		keywords: data.Tags.join(','),
		commentCount: data.Comments.length,
		comments: data.Comments.map((c) => {
			return {
				'@type': 'Comment',
				author: generateUserLinkSchema(c.User),
				text: truncateText(htmlToText(c.Content), 80),
				upvoteCount: c.RatingData.ThumbsUp,
				downvoteCount: c.RatingData.ThumbsDown,
				datePublished: c.DateWritten
			};
		})
	})}
</script>`}
