<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import type { Snippet } from "svelte";

    interface Props {
        title: string,
        itemPlaceholderCount?: number,
        itemSnippet: Snippet,
    }

    let {
        title,
        itemPlaceholderCount = 6,
        itemSnippet
    }: Props = $props();
</script>

<div class="mb-5">
    <div class="mb-4">
        <h2 class="h4">{title}</h2>
    </div>
    <ul class="ps-0">
        {#each {length: itemPlaceholderCount} as _}
            {@render itemSnippet()}
        {/each}
    </ul>
    <div class="text-center">
        <Button mod="px-4" disabled>View all</Button>
    </div>
</div>