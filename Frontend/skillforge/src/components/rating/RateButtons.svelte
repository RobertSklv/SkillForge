<script lang="ts">
	import { rate } from "$lib/api/client";
	import type RatingData from "$lib/types/RatingData";
    import './rate-buttons.scss';

    interface Props {
        data: RatingData,
        subjectId: number,
        type: 'article' | 'comment',
        readonly?: boolean,
    }

    let {
        data,
        subjectId,
        type,
        readonly,
    }: Props = $props();

    let thumbsUp = $state<number>(data.ThumbsUp);
    let thumbsDown = $state<number>(data.ThumbsDown);
    let currentUserRate = $state(data.UserRating);

    function toggleThumbsUp() {
        let newRate: -1 | 0 | 1 = 0;

        if (currentUserRate != 1) {
            newRate = 1;

            if (currentUserRate == -1) {
                thumbsDown--;
            }
        }

        rate(subjectId, newRate, type)
            .then(() => {
                thumbsUp++;
                currentUserRate = newRate;
            });
    }

    function toggleThumbsDown() {
        let newRate: -1 | 0 | 1 = 0;

        if (currentUserRate != -1) {
            newRate = -1;

            if (currentUserRate == 1) {
                thumbsUp--;
            }
        }

        rate(subjectId, newRate, type)
            .then(() => {
                thumbsDown++;
                currentUserRate = newRate;
            });
    }
</script>

<div class="text-end">
    <button type="button" class="rate-btn rate-btn__positive bg-transparent border-0 text-primary" aria-label="Thumbs up" onclick={toggleThumbsUp} disabled={readonly}>
        <i class="bi bi-hand-thumbs-up{currentUserRate == 1 ? '-fill' : ''} fs-4"></i>
        <small>{thumbsUp}</small>
    </button>
    <button type="button" class="rate-btn rate-btn__negative bg-transparent border-0 text-primary" aria-label="Thumbs down" onclick={toggleThumbsDown} disabled={readonly}>
        <i class="bi bi-hand-thumbs-down{currentUserRate == -1 ? '-fill' : ''} fs-4"></i>
        <small>{thumbsDown}</small>
    </button>
</div>