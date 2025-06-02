<script lang="ts">
	import type TopArticleItemType from "$lib/types/TopArticleItemType";
	import { formatRelativeTime } from "$lib/util";

	interface Props {
		data: TopArticleItemType;
	}

	let { data }: Props = $props();
</script>

<a
	href="/article/{data.ArticleId}"
	class="list-group-item list-group-item-action"
	aria-label="Article '{data.Title}' link"
>
	<div class="d-flex w-100 justify-content-between">
		<h4 class="h5 mb-1">{data.Title}</h4>
	</div>
	<small class="text-body-tertiary opacity-75">
		<i class="bi bi-eye-fill"></i>
		<span>{data.ViewCount}</span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
		<i class="bi bi-chat-fill"></i>
		<span>{data.CommentCount}</span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
        <span>{formatRelativeTime(data.DatePublished)}</span>
	</small>
</a>
