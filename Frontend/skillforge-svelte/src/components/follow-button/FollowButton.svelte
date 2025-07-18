<script lang="ts">
	import { followTag, followUser, unfollowTag, unfollowUser } from "skillforge-common/api/client";
	import Button from "../button/Button.svelte";

    interface Props {
        subjectName: string,
        type: 'user' | 'tag',
        isFollowedByCurrentUser: boolean,
        btnSize?: 'md' | 'sm'
    }

    let {
        subjectName,
        type,
        isFollowedByCurrentUser = $bindable<boolean>(),
        btnSize = 'md',
    }: Props = $props();

    const followFunctions = {
        user: followUser,
        tag: followTag
    }

    const unfollowFunctions = {
        user: unfollowUser,
        tag: unfollowTag
    }

	async function follow() {
		await followFunctions[type](subjectName);

        isFollowedByCurrentUser = true;
	}

	async function unfollow() {
		await unfollowFunctions[type](subjectName);

        isFollowedByCurrentUser = false;
	}
</script>

{#if isFollowedByCurrentUser}
    <Button onclick={unfollow} size={btnSize} mod="follow-button" isOutline>Unfollow</Button>
{:else}
    <Button onclick={follow} size={btnSize} mod="follow-button border border-2 border-primary">Follow</Button>
{/if}