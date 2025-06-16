<script lang="ts">
	import InfiniteScroll from '$components/infinite-scroll/InfiniteScroll.svelte';
	import Modal from '$components/modal/Modal.svelte';
	import ModalHeader from '$components/modal/ModalHeader.svelte';
	import UserListItem from '$components/user/UserListItem.svelte';
	import { getRating, rate } from '$lib/api/client';
	import type RatingData from '$lib/types/RatingData';
	import './rate-buttons.scss';

	const RATE_ITEMS_BATCH_SIZE = 15;

	interface Props {
		data: RatingData;
		subjectId: number;
		type: 'article' | 'comment';
		size?: 'normal' | 'small';
		readonly?: boolean;
	}

	let {
		data,
		subjectId,
		type,
		size = 'normal',
		readonly
	}: Props = $props();

	let thumbsUp = $state<number>(data.ThumbsUp);
	let thumbsDown = $state<number>(data.ThumbsDown);
	let currentUserRate = $state(data.UserRating);
	let isThumbsUpModalOpen = $state<boolean>(false);
	let isThumbsDownModalOpen = $state<boolean>(false);

	function toggleThumbsUp() {
		let newRate: -1 | 0 | 1 = 0;

		if (currentUserRate != 1) {
			newRate = 1;
		}

		rate(subjectId, newRate, type).then(() => {
			if (currentUserRate == 1) {
				thumbsUp--;
			} else {
				thumbsUp++;
			}

			if (currentUserRate == -1) {
				thumbsDown--;
			}

			currentUserRate = newRate;
		});
	}

	function toggleThumbsDown() {
		let newRate: -1 | 0 | 1 = 0;

		if (currentUserRate != -1) {
			newRate = -1;
		}

		rate(subjectId, newRate, type).then(() => {
			if (currentUserRate == -1) {
				thumbsDown--;
			} else {
				thumbsDown++;
			}

			if (currentUserRate == 1) {
				thumbsUp--;
			}

			currentUserRate = newRate;
		});
	}

	function loadMoreRates(batchIndex: number, rateType: 'positive' | 'negative') {
		return getRating(subjectId, rateType, type, batchIndex, RATE_ITEMS_BATCH_SIZE);
	}
</script>

<div class="text-end">
	<button
		type="button"
		class="rate-btn rate-btn__positive bg-transparent border-0 text-primary pe-0"
		aria-label="Thumbs up"
		title="Thumbs up"
		onclick={toggleThumbsUp}
		disabled={readonly}
	>
		<i
			class="bi bi-hand-thumbs-up{currentUserRate == 1 ? '-fill' : ''} {size === 'normal'
				? 'fs-3'
				: 'fs-6'}"
		></i>
	</button>
	<button
		type="button"
		aria-label="View positive raters"
		title="View positive raters"
		onclick={() => (isThumbsUpModalOpen = true)}
		class="bg-transparent border-0 text-primary {size === 'normal' ? 'fs-5' : 'fs-6 p-0'}"
		disabled={data.ThumbsUp === 0 || readonly}
	>
		{thumbsUp}
	</button>
	{#if data.ThumbsUp > 0 && !readonly}
		<Modal bind:show={isThumbsUpModalOpen} verticallyCentered scrollable>
			{#snippet header()}
				<ModalHeader title="Thumbs up"></ModalHeader>
			{/snippet}
			<InfiniteScroll
				batchSize={RATE_ITEMS_BATCH_SIZE}
				loadMore={(i) => loadMoreRates(i, 'positive')}
				autoLoadOnMount
			>
				{#snippet itemSnippet(item)}
					<UserListItem data={item} />
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
	{/if}

	<button
		type="button"
		class="rate-btn rate-btn__negative bg-transparent border-0 text-primary pe-0 ms-2"
		aria-label="Thumbs down"
		title="Thumbs down"
		onclick={toggleThumbsDown}
		disabled={readonly}
	>
		<i
			class="bi bi-hand-thumbs-down{currentUserRate == -1 ? '-fill' : ''} {size === 'normal'
				? 'fs-3'
				: 'fs-6'}"
		></i>
	</button>
	<button
		type="button"
		aria-label="View negative raters"
		title="View negative raters"
		onclick={() => (isThumbsDownModalOpen = true)}
		class="bg-transparent border-0 text-primary {size === 'normal' ? 'fs-5' : 'fs-6 p-0'}"
		disabled={data.ThumbsDown === 0 || readonly}
	>
		{thumbsDown}
	</button>
	{#if data.ThumbsDown > 0 && !readonly}
		<Modal bind:show={isThumbsDownModalOpen} verticallyCentered scrollable>
			{#snippet header()}
				<ModalHeader title="Thumbs down"></ModalHeader>
			{/snippet}
			<InfiniteScroll
				batchSize={RATE_ITEMS_BATCH_SIZE}
				loadMore={(i) => loadMoreRates(i, 'negative')}
				autoLoadOnMount
			>
				{#snippet itemSnippet(item)}
					<UserListItem data={item} />
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
	{/if}
</div>
