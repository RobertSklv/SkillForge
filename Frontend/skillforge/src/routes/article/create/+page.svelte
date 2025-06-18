<script lang="ts">
	import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';
	import { setContext } from 'svelte';
	import ArticleCreateForm from '$components/article/ArticleUpsertForm.svelte';

	interface Props {
		data: ArticleUpsertPageModel;
	}

	let { data }: Props = $props();

	setContext('page', data);
</script>

<svelte:head>
	<title>SkillForge | Create article</title>
	<meta name="description" content="Create article" />
	<meta name="robots" content="noindex,nofollow" />
</svelte:head>

<ArticleCreateForm />
