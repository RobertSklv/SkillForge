<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import TagLink from "$components/link/TagLink.svelte";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type TagListItemType from "$lib/types/TagListItemType";

    interface Props {
        data: TagListItemType,
        mod?: string
    }

    let {
        data,
        mod
    }: Props = $props();
</script>

<li class="d-flex justify-content-between align-items-center {mod}">
    <TagLink data={data.Link} />
    {#if $currentUserStore && $currentUserStore.Name != data.Link.Name}
        {#if data.IsFollowedByCurrentUser}
            <Button size="sm" isOutline>Unfollow</Button>
        {:else}
            <Button size="sm">Follow</Button>
        {/if}
    {/if}
</li>