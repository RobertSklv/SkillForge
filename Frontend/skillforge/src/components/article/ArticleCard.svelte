<script lang="ts">
	import CommentLimited from '$components/comment/CommentLimited.svelte';
	import Block from '$components/layout/Block.svelte';
	import Link from '$components/link/Link.svelte';
	import TagLink from '$components/link/TagLink.svelte';
	import RateButtons from '$components/rating/RateButtons.svelte';
	import AuthorBox from '$components/user/AuthorBox.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from '$lib/types/ArticleCardType';

	interface Props {
		data: ArticleCardType;
	}

	let { data }: Props = $props();
</script>

<Block>
	{#if data.CoverImage}
		<img src={PUBLIC_BACKEND_DOMAIN + data.CoverImage} class="card-img-top rounded-top-3 object-fit-cover mb-3" alt="Article cover" style="height: 250px" />
	{/if}
	<AuthorBox name={data.Author.Name} date={data.DatePublished} />
	<div class="card-body mx-5">
		<div class="d-flex align-items-start">
			<h2 class="card-title fw-bold">
				<a href="/article/{data.ArticleId}" class="text-decoration-none">{data.Title}</a>
			</h2>
			{#if $currentUserStore}
				<button class="bg-transparent border-0 text-primary ms-auto" aria-label="Save article">
					<i class="bi bi-bookmark fs-5"></i>
				</button>
			{/if}
		</div>
		<div class="d-flex justify-content-between">
			<div>
				<div class="tags d-flex gap-1">
					{#each data.Tags as tag}
						<TagLink size="small" background="fill" muted={true} data={tag} />
					{/each}
				</div>
			</div>
			<div>
				<div class="d-flex align-items-center">
					<RateButtons data={data.RatingData} subjectId={data.ArticleId} size="small" type="article" readonly={true} />
					{#if data.Comments.length}
						<div class="text-primary ms-2">
							<i class="bi bi-chat fs-6"></i>
							<small>{data.Comments.length}</small>
						</div>
					{/if}
				</div>
			</div>
		</div>
	</div>
	<div>
		{#each data.Comments as comment}
			<CommentLimited data={comment} />
		{/each}
	</div>
	{#if data.Comments.length < data.TotalComments}
		<div class="text-center mt-2 mb-4">
			<Link href="/article/{data.ArticleId}" background="fill">
				View all comments
			</Link>
		</div>
	{/if}
</Block>