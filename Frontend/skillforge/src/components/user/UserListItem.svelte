<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import UserLink from "$components/link/UserLink.svelte";
	import type UserListItemType from "$lib/types/UserListItemType";

    interface Props {
        data: UserListItemType
    }

    let {
        data
    }: Props = $props();
</script>

<li class="d-flex justify-content-between align-items-center">
    <UserLink data={data.Link} />
    {#if data.IsFollowedByCurrentUser}
        <Button isOutline={true}>Unfollow</Button>
    {:else}
        <Button>Follow</Button>
    {/if}
</li>