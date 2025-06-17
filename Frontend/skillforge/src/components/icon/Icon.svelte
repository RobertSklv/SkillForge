<script lang="ts">
    const {
        type,
        mod
    }: {
        type: string,
        mod?: string
    } = $props();
</script>

<i class="bi bi-{type} {mod}"></i>