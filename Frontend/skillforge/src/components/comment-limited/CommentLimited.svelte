<script lang="ts">
	import AuthorBox from '$components/author-box/AuthorBox.svelte';
	import type CommentModel from '$lib/types/CommentModel';

	interface Props {
		data: CommentModel;
	}

	let { data }: Props = $props();
</script>

<AuthorBox
	name={data.User.Name}
	avatarImage={data.User.AvatarImage}
	date={data.DateWritten}
	editedDate={data.DateEdited}
	size="small"
/>
<div class="mx-5 p-3">
	<div class="content-wrapper rounded-3 p-3">
		<div class="content text-break rich-text-content">
			{@html data.Content}
		</div>
	</div>
</div>

<style lang="scss">
	.content {
		line-clamp: 2;
		-webkit-line-clamp: 2;
		display: -webkit-box;
		-webkit-box-orient: vertical;
		overflow: hidden;

		&-wrapper {
			background-color: rgba(111, 66, 193, 50%);
		}
	}
</style>
