<script lang="ts">
	import Block from '$components/layout/Block.svelte';
	import RateButtons from '$components/rating/RateButtons.svelte';
	import AuthorBox from '$components/user/AuthorBox.svelte';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type CommentModel from '$lib/types/CommentModel';

	interface Props {
		data: CommentModel;
	}

	let { data }: Props = $props();
</script>

<Block>
	{#snippet header()}
		<AuthorBox
			name={data.User.Name}
			avatarImage={data.User.AvatarImage}
			date={data.DateWritten}
			size="small"
			indent={false}
		/>
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
