<script lang="ts">
	import Block from "$components/layout/Block.svelte";
	import UserLink from "$components/link/UserLink.svelte";
	import RateButtons from "$components/rating/RateButtons.svelte";
	import AuthorBox from "$components/user/AuthorBox.svelte";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import type CommentModel from "$lib/types/CommentModel";

    interface Props {
        data: CommentModel
    }

    let {
        data
    }: Props = $props();
</script>

<AuthorBox name={data.User.Name} date={data.DateWritten} size="small" />
<div class="mx-5 p-3">
    <div class="content-wrapper rounded-3 p-3">
        <div class="content text-break">
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