<script lang="ts">
	import Block from '$components/layout/Block.svelte';
	import UserLink from '$components/link/UserLink.svelte';
	import RateButtons from '$components/rating/RateButtons.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import { formatRelativeTime } from '$lib/util';

	interface Props {
		data: ArticleCardType;
	}

	let { data }: Props = $props();
</script>

<Block>
	{#snippet header()}
		<div class="d-flex justify-content-between">
			<UserLink data={data.Author} />
			<small class="text-body-secondary text-end">{formatRelativeTime(data.DatePublished)}</small>
		</div>
	{/snippet}
	{#if data.CoverImage}
		<img src={PUBLIC_BACKEND_DOMAIN + data.CoverImage} class="card-img-top" alt="Article cover" style="height: 250px; object-fit: cover" />
	{/if}
	<div class="card-body">
		<h2 class="card-title">
            <a href="/article/{data.ArticleId}" class="text-decoration-none">{data.Title}</a>
        </h2>
	</div>
	{#snippet footer()}
		<RateButtons data={data.RatingData} subjectId={data.ArticleId} type="article" readonly={true} />
	{/snippet}
</Block>
