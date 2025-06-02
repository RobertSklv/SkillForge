<script lang="ts">
	import type ArticleUpsertPageModel from "$lib/types/ArticleUpsertPageModel";
	import { setContext } from "svelte";
	import ArticleCreateForm from "$components/article/ArticleUpsertForm.svelte";

	interface Props {
		data: ArticleUpsertPageModel
	};

	let {
		data
	}: Props = $props();

	setContext('page', data);
</script>

<ArticleCreateForm />