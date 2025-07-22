<script lang="ts">
	import Icon from "$components/icon/Icon.svelte";
	import { formatRelativeTime, getImagePath } from "skillforge-common/util";
	import moment from "moment";

    interface Props {
        name: string,
        avatarImage: string | null | undefined,
        date: string,
        editedDate?: string,
        mod?: string,
        size?: 'normal' | 'small',
        indent?: boolean
    }

    let {
        name,
        avatarImage,
        date,
        editedDate,
        mod,
        size = 'normal',
        indent = true,
    }: Props = $props();
</script>

<div class="author-box d-flex gap-2 size-{size} {mod}" class:ms-2={indent}>
	<div class="author-box__image-wrapper d-flex flex-column justify-content-start align-items-end" class:w-auto={!indent}>
        <a href="/user/{name}" class="text-decoration-none">
		    <img src="{getImagePath(avatarImage)}" class="author-box__image round-image" alt="Robert profile" />
        </a>
	</div>
	<div class="author-box__info-col d-flex flex-column">
        <a href="/user/{name}" class="text-decoration-none">{name}</a>
        <div class="small text-tertiary">
            <time datetime={moment(date).format('YYYY-MM-DD HH:mm')}>
                {moment(date).format('ddd, D MMMM, YYYY HH:mm')}
            </time>
            <span class="text-muted ms-2">({formatRelativeTime(date)})</span>
        </div>
        {#if editedDate}
            <div class="small text-info-emphasis">
                <Icon type="pencil-fill" />
                <time datetime={moment(editedDate).format('YYYY-MM-DD HH:mm')}>
                    {moment(editedDate).format('ddd, D MMMM, YYYY HH:mm')}
                </time>
                <span class="ms-2">({formatRelativeTime(editedDate)})</span>
            </div>
        {/if}
	</div>
</div>

<style lang="scss">
	.author-box {
        &__image {
            &-wrapper {
                width: 48px;
            }
        }

        &.size-normal {
            .author-box__image {
                width: 40px;
            }
        }

        &.size-small {
            font-size: 14px;

            .author-box__image {
                width: 30px;
            }
        }
	}
</style>
