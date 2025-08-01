<script lang="ts">
	import Button from '$components/button/Button.svelte';
	import Comment from '$components/comment/Comment.svelte';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownDivider from '$components/dropdown-divider/DropdownDivider.svelte';
	import DropdownItem from '$components/dropdown-item/DropdownItem.svelte';
	import Form from '$components/form/Form.svelte';
	import TextEditor from '$components/form/text-editor/TextEditor.svelte';
	import Block from '$components/block/Block.svelte';
	import LoginCta from '$components/login-cta/LoginCta.svelte';
	import RateButtons from '$components/rate-buttons/RateButtons.svelte';
	import AuthorBox from '$components/author-box/AuthorBox.svelte';
	import { PUBLIC_BACKEND_DOMAIN } from '$env/static/public';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type ArticlePageModel from '$lib/types/ArticlePageModel';
	import type CommentModel from '$lib/types/CommentModel';
	import { writable } from 'svelte/store';
	import Icon from '$components/icon/Icon.svelte';
	import ReportModal from '$components/report/ReportModal.svelte';
	import type CommentFormData from '$lib/types/CommentFormData';
	import { setContext } from 'svelte';
	import type ArticleContext from '$lib/types/ArticleContext';
	import { fade } from 'svelte/transition';
	import { flip } from 'svelte/animate';

	interface Props {
		data: ArticlePageModel;
	}

	let { data }: Props = $props();

	let commentFormData = writable<CommentFormData>({
		ArticleId: data.ArticleId,
		CommentId: 0,
		Content: ''
	});

	let comments = $state<CommentModel[]>(data.Comments);
	let showReportModal = $state<boolean>(false);

	setContext<ArticleContext>('article', {
		deleteComment
	});

	async function addComment(comment: CommentModel) {
		if (!$currentUserStore) {
			throw Error('User not logged in');
		}

		comments.push(comment);
	}

	function deleteComment(id: number) {
		comments = comments.filter((c) => c.CommentId != id);
	}
</script>

<div class="d-flex flex-column gap-5">
	<Block>
		{#snippet header()}
			<div class="row mb-3 pt-2">
				<div class="col">
					<AuthorBox
						name={data.Author.Link.Name}
						avatarImage={data.Author.Link.AvatarImage}
						date={data.DatePublished}
						indent={false}
					/>
				</div>
				{#if $currentUserStore}
					<div class="col-3 text-end">
						<Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start">
							{#snippet buttonSnippet()}
								<Icon type="three-dots-vertical" />
							{/snippet}
							{#if $currentUserStore.Name == data.Author.Link.Name}
								<DropdownItem href="/article/{data.ArticleId}/edit">
									<Icon type="pencil-square" />
									Edit
								</DropdownItem>
								<DropdownDivider />
							{/if}
							<DropdownItem type="button" onclick={() => (showReportModal = true)}>
								<Icon type="exclamation-triangle" />
								Report
							</DropdownItem>
						</Dropdown>
					</div>
				{/if}
			</div>
		{/snippet}

		{#if data.CoverImage}
			<div
				class="cover-image"
				style="background-image: url('{PUBLIC_BACKEND_DOMAIN + data.CoverImage}')"
			></div>
		{/if}

		<article class="card-body">
			{#each data.Tags as tag}
				<a href="/tag/{tag}" class="me-2">#{tag}</a>
			{/each}
			<h1 class="card-title mb-4">{data.Title}</h1>
			<div class="card-text text-break rich-text-content">
				{@html data.Content}
			</div>
		</article>

		{#snippet footer()}
			<div class="row">
				<div class="col-3 fs-5 d-flex align-items-center">
					<Icon type="eye" mod="me-1" />
					<small class="text-muted">{data.Views}</small>
				</div>
				<div class="col-9">
					<RateButtons
						data={data.RatingData}
						subjectId={data.ArticleId}
						type="article"
						readonly={!$currentUserStore}
					/>
				</div>
			</div>
		{/snippet}
	</Block>

	<div class="d-flex flex-column gap-3">
		{#each comments as comment (comment.CommentId)}
			<div transition:fade={{ duration: 200 }} animate:flip={{ duration: 200 }}>
				<Comment data={comment} />
			</div>
		{/each}
	</div>

	{#if $currentUserStore}
		<Block>
			<div class="card-body">
				<Form
					action="/Comment/Upsert"
					formData={$commentFormData}
					onSuccess={addComment}
					resetMode="onsuccess"
				>
					<TextEditor
						id="CommentContent"
						name="Content"
						label={null}
						height={200}
						bind:content={$commentFormData.Content}
						imageUploadType="comment"
					/>

					<div class="text-center">
						<Button isSubmitButton={true}>Comment</Button>
					</div>
				</Form>
			</div>
		</Block>
	{/if}

	<LoginCta ctaText="Log in" description="to comment and rate content." inline={true} />
</div>

<ReportModal entityId={data.ArticleId} action="/ArticleReport/Create" bind:show={showReportModal} />

<style>
	.cover-image {
		height: 300px;
		box-shadow: 0px 0px 99px 32px rgba(0, 0, 0, 1) inset;
		-webkit-box-shadow: 0px 0px 99px 32px rgba(0, 0, 0, 1) inset;
		-moz-box-shadow: 0px 0px 99px 32px rgba(0, 0, 0, 1) inset;
		background-repeat: no-repeat;
		background-size: cover;
	}
</style>
