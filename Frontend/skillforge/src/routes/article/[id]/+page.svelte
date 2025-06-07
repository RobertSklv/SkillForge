<script lang="ts">
	import Article from "$components/article/Article.svelte";
	import Columns from "$components/layout/Columns.svelte";
	import type ArticlePageModel from "$lib/types/ArticlePageModel";

    interface Props {
        data: {
            article: ArticlePageModel
        }
    }

    let {
        data
    }: Props = $props();
</script>

<Columns>
	{#snippet leftColumn()}
		
	{/snippet}

    <Article data={data.article} />

	{#snippet rightColumn()}
		
	{/snippet}
</Columns>