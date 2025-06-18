<script lang="ts">
	import FollowButton from '$components/button/FollowButton.svelte';
	import Block from '$components/layout/Block.svelte';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import type Author from '$lib/types/Author';
	import { getImagePath } from '$lib/util';
	import moment from 'moment';

	interface Props {
		data: Author;
		mod?: string;
	}

	let { data, mod }: Props = $props();
</script>

<Block {mod}>
	{#snippet header()}
		<h2 class="text-center">Author</h2>
	{/snippet}

	<div class="card-body">
		<div class="p-4 text-center">
			<a href="/user/{data.Link.Name}">
				<img
					src={getImagePath(data.Link.AvatarImage)}
					alt="{data.Link.Name} avatar"
					class="rounded-circle w-100 object-fit-cover border border-2 border-primary mb-4"
					style:aspect-ratio="1"
					style:max-width="300px"
				/>
			</a>
		</div>
		<dl class="mb-0 text-center text-lg-start">
			<dt>Nickname</dt>
			<dd class="text-body-tertiary">
				{data.Link.Name}
			</dd>
			{#if data.Bio}
				<dt>Bio</dt>
				<dd class="text-body-tertiary">{data.Bio}</dd>
			{/if}
			<dt>Date joined</dt>
			<dd class="text-body-tertiary">
				{moment(data.JoinedDate).format('D MMMM YYYY')}
			</dd>
		</dl>
		{#if $currentUserStore && $currentUserStore.Name != data.Link.Name}
			<div class="text-center mt-5">
				<FollowButton
					subjectName={data.Link.Name}
					type="user"
					isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
				/>
			</div>
		{/if}
	</div>
</Block>
