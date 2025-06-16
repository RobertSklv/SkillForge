<script lang="ts">
	import FollowButton from "$components/button/FollowButton.svelte";
	import UserLink from "$components/link/UserLink.svelte";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type UserListItemType from "$lib/types/UserListItemType";

    interface Props {
        data: UserListItemType,
        mod?: string
    }

    let {
        data,
        mod
    }: Props = $props();
</script>

<div class="d-flex justify-content-between align-items-center {mod}">
    <UserLink data={data.Link} />
    {#if $currentUserStore && $currentUserStore.Name != data.Link.Name}
        <FollowButton
            subjectName={data.Link.Name}
            type="user"
            isFollowedByCurrentUser={data.IsFollowedByCurrentUser}
            btnSize="sm"
        />
    {/if}
</div>