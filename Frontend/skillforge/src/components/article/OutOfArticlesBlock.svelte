<script lang="ts">
	import Block from "$components/layout/Block.svelte";

    interface Props {
        message?: string,
    }

    let {
        message = "You've reached the end. No more articles to show."
    }: Props = $props();
</script>

<Block>
    <div class="card-body d-flex align-items-center p-4 px-5">
        <span class="display-1 me-4" style:margin-top="-14px">:(</span>
        <span>{message}</span>
    </div>
</Block>