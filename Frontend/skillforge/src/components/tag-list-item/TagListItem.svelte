<script lang="ts">
	import FollowButton from "$components/follow-button/FollowButton.svelte";
	import TagLink from "$components/tag-link/TagLink.svelte";
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

<div class="d-flex justify-content-center justify-content-lg-between align-items-center {mod}">
    <TagLink data={data.Link} />
    {#if $currentUserStore}
        <FollowButton
            subjectName={data.Link.Name}
            type="tag"
            bind:isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
            btnSize="sm"
        />
    {/if}
</div>