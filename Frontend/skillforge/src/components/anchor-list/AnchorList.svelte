<script lang="ts" generics="T">
	import Block from '$components/layout/Block.svelte';
	import type { Snippet } from 'svelte';

	interface Props {
		title: string;
		items: T[];
		itemSnippet: Snippet<[T]>;
	}

	let {
        title,
        items,
        itemSnippet
    }: Props = $props();
</script>

<Block>
	{#snippet header()}
		<h3 class="card-title h4">{title}</h3>
	{/snippet}
	<div class="card-body">
		<ul class="list-group">
			{#each items as item}
				{@render itemSnippet(item)}
			{/each}
		</ul>
	</div>
</Block>
