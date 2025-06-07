<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import UserLink from "$components/link/UserLink.svelte";
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

<li class="d-flex justify-content-between align-items-center {mod}">
    <UserLink data={data.Link} />
    {#if data.IsFollowedByCurrentUser}
        <Button size="sm" isOutline>Unfollow</Button>
    {:else}
        <Button size="sm">Follow</Button>
    {/if}
</li>