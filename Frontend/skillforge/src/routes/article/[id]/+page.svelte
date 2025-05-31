<script lang="ts">
	import Article from "$components/article/Article.svelte";
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

<div class="row">
    <div class="col-12 col-md-2"></div>
    <div class="col-12 col-md-8">
        <Article data={data.article} />
    </div>
</div>