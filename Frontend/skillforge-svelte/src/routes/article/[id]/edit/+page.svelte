<script lang="ts">
	import type ArticleUpsertPageModel from "skillforge-common/types/ArticleUpsertPageModel";
	import { setContext } from "svelte";
	import ArticleCreateForm from "$components/article-upsert-form/ArticleUpsertForm.svelte";
	import { PUBLIC_BASE_URL } from "$env/static/public";

	interface Props {
		data: ArticleUpsertPageModel
	};

	let {
		data
	}: Props = $props();

	setContext('page', data);
</script>

<svelte:head>
	<title>SkillForge | Edit article '{data.CurrentState?.Model.Title}'</title>
	<meta name="description" content="Edit article '{data.CurrentState?.Model.Title}'">
	<meta name="robots" content="noindex,nofollow">
	<link rel="canonical" href="{PUBLIC_BASE_URL}/article/{data.CurrentState?.Model.Id}">
</svelte:head>

<ArticleCreateForm />