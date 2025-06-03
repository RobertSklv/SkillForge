<script lang="ts">
	import moment from "moment";

    interface Props {
        name: string,
        date: string,
        size?: 'normal' | 'small',
        indent?: boolean
    }

    let {
        name,
        date,
        size = 'normal',
        indent = true,
    }: Props = $props();
</script>

<div class="author-box d-flex gap-2 size-{size}" class:ms-2={indent}>
	<div class="author-box__image-wrapper d-flex flex-column justify-content-start align-items-end" class:w-auto={!indent}>
        <a href="/user/{name}" class="text-decoration-none">
		    <img src="/user.png" class="author-box__image rounded-circle" alt="Robert profile" />
        </a>
	</div>
	<div class="author-box__info-col d-flex flex-column">
        <a href="/user/{name}" class="text-decoration-none">{name}</a>
        <div class="small text-tertiary">
            {moment(date).format('MMMM Do YYYY, HH:mm:ss')}
            <span class="text-muted ms-2">({moment(date).fromNow()})</span>
        </div>
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
