<script lang="ts">
	import type ArticleUpsertPageModel from 'skillforge-common/types/ArticleUpsertPageModel';
	import { setContext } from 'svelte';
	import ArticleCreateForm from '$components/article-upsert-form/ArticleUpsertForm.svelte';

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
