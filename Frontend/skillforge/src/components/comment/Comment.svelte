<script lang="ts">
	import Button from '$components/button/Button.svelte';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownDivider from '$components/dropdown/DropdownDivider.svelte';
	import DropdownItem from '$components/dropdown/DropdownItem.svelte';
	import Form from '$components/form/Form.svelte';
	import TextEditor from '$components/form/TextEditor.svelte';
	import Icon from '$components/icon/Icon.svelte';
	import Block from '$components/layout/Block.svelte';
	import RateButtons from '$components/rating/RateButtons.svelte';
	import ReportModal from '$components/report/ReportModal.svelte';
	import AuthorBox from '$components/user/AuthorBox.svelte';
	import { deleteComment } from '$lib/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import { addToast } from '$lib/stores/toastStore';
	import type ArticleContext from '$lib/types/ArticleContext';
	import type CommentFormData from '$lib/types/CommentFormData';
	import type CommentModel from '$lib/types/CommentModel';
	import { getContext } from 'svelte';
	import { writable } from 'svelte/store';

	interface Props {
		data: CommentModel;
	}

	let { data }: Props = $props();

	let showReportModal = $state<boolean>(false);
	let editMode = $state<boolean>(false);

	let articleContext = getContext<ArticleContext>('article');

	let editFormData = writable<CommentFormData>({
		CommentId: data.CommentId,
		Content: data.Content
	});

	function onEditSuccess(response: CommentModel) {
		editMode = false;
		data.Content = response.Content;
		$editFormData.Content = response.Content;
		data.DateWritten = response.DateWritten;
		data.DateEdited = response.DateEdited;

		addToast('Comment edited successfully');
	}

	function cancelEdit() {
		editMode = false;
		$editFormData.Content = data.Content;
	}

	async function onDelete() {
		let commentId = data.CommentId;
		await deleteComment(commentId).then((r) => {
			articleContext.deleteComment(commentId);
			addToast('Comment deleted successfully');
		});
	}
</script>

<Block>
	{#snippet header()}
		<div class="row mb-3 pt-2">
			<div class="col">
				<AuthorBox
					name={data.User.Name}
					avatarImage={data.User.AvatarImage}
					date={data.DateWritten}
					editedDate={data.DateEdited}
					size="small"
					indent={false}
				/>
			</div>
			{#if $currentUserStore}
				<div class="col-3 text-end">
					<Dropdown menuClass="dropdown-menu-end dropdown-menu-xl-start">
						{#snippet buttonSnippet()}
							<Icon type="three-dots-vertical" />
						{/snippet}
						{#if $currentUserStore.Name == data.User.Name}
							<DropdownItem type="button" onclick={() => (editMode = true)}>
								<Icon type="pencil-square" />
								Edit
							</DropdownItem>
							<DropdownItem type="button" onclick={onDelete}>
								<Icon type="trash" />
								Delete
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

	{#if editMode}
		<div class="card-body">
			<Form action="/Comment/Upsert" formData={$editFormData} onSuccess={onEditSuccess}>
				<TextEditor
					id="CommentEditContent-{data.CommentId}"
					name="Content"
					label={null}
					height={200}
					bind:content={$editFormData.Content}
					imageUploadType="comment"
				/>

				<div class="text-center">
					<Button color="secondary" onclick={cancelEdit} mod="me-3">Cancel</Button>
					<Button isSubmitButton={true}>Edit</Button>
				</div>
			</Form>
		</div>
	{:else}
		<div class="card-body">
			<div class="text-break rich-text-content">
				{@html data.Content}
			</div>
		</div>
	{/if}

	{#snippet footer()}
		<RateButtons
			data={data.RatingData}
			subjectId={data.CommentId}
			type="comment"
			readonly={!$currentUserStore}
		/>
	{/snippet}
</Block>

<ReportModal entityId={data.CommentId} action="/CommentReport/Create" bind:show={showReportModal} />
