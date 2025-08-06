<script lang="ts">
    interface Props {
        mod?: string,
        size?: 'normal' | 'small',
        indent?: boolean
    }

    let {
        mod,
        size = 'normal',
        indent = true,
    }: Props = $props();
</script>

<div class="author-box d-flex gap-2 size-{size} w-100 {mod}" class:ms-2={indent}>
	<div class="author-box__image-wrapper d-flex flex-column justify-content-start align-items-end" class:w-auto={!indent}>
        <span class="author-box__image rounded-circle placeholder"></span>
	</div>
	<div class="author-box__info-col d-flex flex-column w-50">
        <span class="{size == 'normal' ? 'fs-4' : 'fs-5'} placeholder col-4"></span>
        <div class="small text-tertiary">
            <span class="fs-6 placeholder col-5"></span>
            <span class="text-muted ms-2 placeholder col-3"></span>
        </div>
	</div>
</div>

<style lang="scss">
	.author-box {
        &__image {
            &-wrapper {
                width: 48px;
                height: 45px;
            }
        }

        &.size-normal {
            .author-box__image {
                width: 40px;
                height: 40px;
            }
        }

        &.size-small {
            font-size: 14px;

            .author-box__image {
                width: 30px;
                height: 30px;
            }
        }
	}
</style>
