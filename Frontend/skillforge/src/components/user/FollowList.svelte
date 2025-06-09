<script lang="ts" generics="T">
	import Button from "$components/button/Button.svelte";
	import InfiniteScroll from "$components/infinite-scroll/InfiniteScroll.svelte";
	import Modal from "$components/modal/Modal.svelte";
	import ModalHeader from "$components/modal/ModalHeader.svelte";
	import type { Snippet } from "svelte";

    interface Props {
        title: string,
        totalCount: number,
        batchSize?: number,
        initiallyVisibleItems: T[],
        loadMore: (batchIndex: number) => Promise<T[]>,
        itemSnippet: Snippet<[T]>,
    }

    let {
        title,
        totalCount,
        batchSize = 15,
        initiallyVisibleItems,
        loadMore,
        itemSnippet
    }: Props = $props();

    let isModalOpen = $state<boolean>(false);
</script>

{#if totalCount > 0}
    <div class="mb-5">
        <div class="mb-4">
            <h2 class="h4">{title}</h2>
        </div>
        <ul class="ps-0 mb-4">
            {#each initiallyVisibleItems as item}
                {@render itemSnippet(item)}
            {/each}
        </ul>
        {#if totalCount > initiallyVisibleItems.length}
            <div class="text-center">
                <Button mod="px-4" onclick={() => isModalOpen = true}>View all</Button>
            </div>
        {/if}
        <Modal bind:show={isModalOpen} verticallyCentered scrollable>
            <ModalHeader {title} />
            <InfiniteScroll {batchSize} {loadMore}>
                {#snippet itemSnippet(item)}
                    {@render itemSnippet(item)}
                {/snippet}
                {#snippet placeholderSnippet()}
                    <div class="d-flex justify-content-center">
                        <div class="spinner-border" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                    </div>
                {/snippet}
            </InfiniteScroll>
        </Modal>
    </div>
{/if}