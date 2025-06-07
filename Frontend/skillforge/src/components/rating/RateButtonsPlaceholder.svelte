<script lang="ts">
    interface Props {
        size?: 'normal' | 'small',
    }

    let {
        size = 'normal',
    }: Props = $props();
</script>

<div class="text-end d-flex">
    <div class="rate-btn text-primary d-flex align-items-center mx-1">
        <i class="bi bi-hand-thumbs-up {size === 'normal' ? 'fs-5' : 'fs-6'}"></i>
        <span class="placeholder placeholder-sm col-4 ms-1 h-auto"></span>
    </div>
    <div class="rate-btn text-primary d-flex align-items-center mx-1">
        <i class="bi bi-hand-thumbs-down {size === 'normal' ? 'fs-5' : 'fs-6'}"></i>
        <span class="placeholder placeholder-sm col-4 ms-1 h-auto"></span>
    </div>
</div>

<style>
    .rate-btn {
        padding-left: 6px;
        padding-right: 6px;
    }
</style>