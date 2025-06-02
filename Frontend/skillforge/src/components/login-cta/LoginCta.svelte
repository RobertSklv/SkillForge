<script lang="ts">
	import Block from "$components/layout/Block.svelte";
	import { currentUserStore } from "$lib/stores/currentUserStore";

    interface Props {
        ctaText: string,
        description: string
        inline?: boolean
    }

    let {
        ctaText,
        description,
        inline,
    }: Props = $props();
</script>

{#if !$currentUserStore}
    <Block>
        <div class="card-text p-4">
            <div class="text-center">
                <a href="/join" class="btn btn-primary text-decoration-none mb-2">{ctaText}</a>
                <p class="mb-0" class:d-inline={inline} class:ms-2={inline}>{description}</p>
            </div>
        </div>
    </Block>
{/if}