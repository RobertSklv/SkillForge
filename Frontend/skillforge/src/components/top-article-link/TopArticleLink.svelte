<script lang="ts">
	import type TopArticleItemType from "$lib/types/TopArticleItemType";
	import { formatRelativeTime } from "$lib/util";
	import moment from "moment";
	import Icon from "$components/icon/Icon.svelte";

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
		<Icon type="eye-fill" />
		<span>{data.ViewCount}</span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
		<Icon type="chat-fill" />
		<span>{data.CommentCount}</span>
        &nbsp;&nbsp;|&nbsp;&nbsp;
        <time datetime={moment(data.DatePublished).format('YYYY-MM-DD HH:mm')}>
			{formatRelativeTime(data.DatePublished)}
		</time>
	</small>
</a>
