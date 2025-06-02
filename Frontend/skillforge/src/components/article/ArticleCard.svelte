<script lang="ts">
	import Block from '$components/layout/Block.svelte';
	import TagLink from '$components/link/TagLink.svelte';
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
		<div class="d-flex align-items-start">
			<h2 class="card-title fw-bold">
				<a href="/article/{data.ArticleId}" class="text-decoration-none">{data.Title}</a>
			</h2>
			<button class="bg-transparent border-0 text-primary ms-auto" aria-label="Save article">
				<i class="bi bi-bookmark fs-5"></i>
			</button>
		</div>
		{#each data.Tags as tag}
			<TagLink size="small" background="fill" muted={true} data={tag} />
		{/each}
	</div>
	{#snippet footer()}
		<div class="d-flex justify-content-between align-items-center">
			<div class="text-primary">
				{#if data.Comments.length}
					<i class="bi bi-chat fs-5"></i>
					<small>{data.Comments.length}</small>
				{/if}
			</div>
			<RateButtons data={data.RatingData} subjectId={data.ArticleId} type="article" readonly={true} />
		</div>
	{/snippet}
</Block>
