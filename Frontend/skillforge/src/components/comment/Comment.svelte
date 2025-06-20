<script lang="ts">
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownDivider from '$components/dropdown/DropdownDivider.svelte';
	import DropdownItem from '$components/dropdown/DropdownItem.svelte';
	import Icon from '$components/icon/Icon.svelte';
	import Block from '$components/layout/Block.svelte';
	import RateButtons from '$components/rating/RateButtons.svelte';
	import ReportModal from '$components/report/ReportModal.svelte';
	import AuthorBox from '$components/user/AuthorBox.svelte';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type CommentModel from '$lib/types/CommentModel';

	interface Props {
		data: CommentModel;
	}

	let { data }: Props = $props();

	let showReportModal = $state<boolean>(false);
</script>

<Block>
	{#snippet header()}
		<div class="row mb-3 pt-2">
			<div class="col-10">
		<AuthorBox
			name={data.User.Name}
			avatarImage={data.User.AvatarImage}
			date={data.DateWritten}
			size="small"
			indent={false}
		/>
			</div>
			<div class="col-2 text-end">
				<Dropdown>
					{#snippet buttonSnippet()}
						<Icon type="three-dots-vertical" />
					{/snippet}
					{#if $currentUserStore}
						{#if $currentUserStore.Name == data.User.Name}
							<DropdownItem href="/">
								<Icon type="pencil-square" />
								Edit
							</DropdownItem>
							<DropdownDivider />
						{/if}
						<DropdownItem type="button" onclick={() => showReportModal = true}>
							<Icon type="exclamation-triangle" />
							Report
						</DropdownItem>
					{/if}
				</Dropdown>
			</div>
		</div>
	{/snippet}

	<div class="card-body">
		<div class="text-break">
			{@html data.Content}
		</div>
	</div>

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