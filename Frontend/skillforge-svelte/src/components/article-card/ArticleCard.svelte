<script lang="ts">
	import CommentLimited from '$components/comment-limited/CommentLimited.svelte';
	import Block from '$components/block/Block.svelte';
	import Link from '$components/link/Link.svelte';
	import TagLink from '$components/tag-link/TagLink.svelte';
	import RateButtons from '$components/rate-buttons/RateButtons.svelte';
	import AuthorBox from '$components/author-box/AuthorBox.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticleCardType from 'skillforge-common/types/ArticleCardType';
	import Icon from '$components/icon/Icon.svelte';

	interface Props {
		data: ArticleCardType;
		showComments?: boolean;
		mod?: string;
	}

	let {
		data,
		showComments = true,
		mod,
	}: Props = $props();
</script>

<Block {mod}>
	{#if data.CoverImage}
		<img src={PUBLIC_BACKEND_DOMAIN + data.CoverImage} class="card-img-top rounded-top-3 object-fit-cover" alt="Article cover" style:height="250px" style:max-width="100%" />
	{/if}
	<AuthorBox name={data.Author.Name} avatarImage={data.Author.AvatarImage} date={data.DatePublished} mod="mt-3" />
	<div class="card-body mx-5">
		<div class="d-flex align-items-start">
			<h2 class="card-title fw-bold">
				<a href="/article/{data.ArticleId}" class="text-decoration-none">{data.Title}</a>
			</h2>
		</div>
		<div class="row justify-content-between">
			<div class="col-12 col-md-6">
				<div class="tags d-flex flex-wrap flex-md-nowrap gap-1 justify-content-center justify-content-md-start">
					{#each data.Tags as tag}
						<TagLink size="small" background="fill" muted={true} data={tag} />
					{/each}
				</div>
			</div>
			<div class="col-12 col-md-6">
				<div class="d-flex justify-content-center justify-content-md-end align-items-center">
					<RateButtons data={data.RatingData} subjectId={data.ArticleId} size="small" type="article" readonly={true} />
					{#if data.Comments.length}
						<div class="text-primary ms-3">
							<Icon type="chat" mod="fs-6" />
							<small>{data.TotalComments}</small>
						</div>
					{/if}
				</div>
			</div>
		</div>
	</div>
	{#if showComments}
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
	{/if}
</Block>