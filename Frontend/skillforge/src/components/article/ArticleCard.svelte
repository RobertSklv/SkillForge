<script lang="ts">
	import RateButtons from '$components/rating/RateButtons.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import type ArticleCardType from '$lib/types/ArticleCardType';
	import { formatRelativeTime } from '$lib/util';

	interface Props {
		data: ArticleCardType;
	}

	let { data }: Props = $props();
</script>

<div class="card mb-4">
	{#if data.CoverImage}
		<img src={PUBLIC_BACKEND_DOMAIN + data.CoverImage} class="card-img-top" alt="Article cover" style="height: 250px; object-fit: cover" />
	{/if}
	<div class="card-body">
		<h2 class="card-title">
            <a href="/article/{data.ArticleId}" class="text-decoration-none">{data.Title}</a>
        </h2>
		<p class="card-text">
			<small class="text-body-secondary">{formatRelativeTime(data.DatePublished)}</small>
		</p>
	</div>
	<div class="card-footer">
		<RateButtons data={data.RatingData} subjectId={data.ArticleId} type="article" readonly={true} />
	</div>
</div>
