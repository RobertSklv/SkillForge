<script lang="ts">
	import Block from "$components/block/Block.svelte";

    interface Props {
        message?: string,
    }

    let {
        message = "You've reached the end. No more articles to show."
    }: Props = $props();
</script>

<Block>
    <div class="card-body d-flex align-items-center p-3 px-4 p-md-4 px-md-5">
        <span class="display-1 me-4" style:margin-top="-14px" style:min-width="30px">:(</span>
        <span>{message}</span>
    </div>
</Block>