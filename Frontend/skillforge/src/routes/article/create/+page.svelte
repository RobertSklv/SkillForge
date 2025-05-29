<script lang="ts">
	import type ArticleCreatePageModel from "$lib/types/ArticleCreatePageModel";
	import { setContext } from "svelte";
	import ArticleCreateForm from "../../../components/article/ArticleCreateForm.svelte";

	interface Props {
		data: ArticleCreatePageModel
	};

	let {
		data
	}: Props = $props();

	setContext('page', data);
</script>

<ArticleCreateForm />