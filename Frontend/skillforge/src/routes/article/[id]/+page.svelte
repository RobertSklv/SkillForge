<script lang="ts">
	import Article from '$components/article/Article.svelte';
	import TwoColumns from '$components/layout/TwoColumns.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import type ArticlePageModel from '$lib/types/ArticlePageModel';
	import { htmlToText, truncateText } from '$lib/util';

	interface Props {
		data: {
			article: ArticlePageModel;
		};
	}

    let {
        data
    }: Props = $props();
</script>

<svelte:head>
	<title>SkillForge | {data.article.Title}</title>
	<meta
		name="description"
		content="{truncateText(htmlToText(data.article.Content), 120)} Tags: {data.article.Tags.join(', ')}"
	/>
	<meta name="robots" content="index,follow" />
	<link rel="canonical" href="{PUBLIC_BASE_URL}/article/{data.article.ArticleId}" />
</svelte:head>

<TwoColumns>
	{#snippet leftColumn()}{/snippet}

	<Article data={data.article} />
</TwoColumns>
