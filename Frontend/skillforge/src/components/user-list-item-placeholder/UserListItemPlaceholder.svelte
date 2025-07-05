<script lang="ts">
	import Button from "$components/button/Button.svelte";
	import UserLinkPlaceholder from "$components/user-link-placeholder/UserLinkPlaceholder.svelte";

    interface Props {
        mod?: string
    }

    let {
        mod
    }: Props = $props();
</script>

<li class="d-flex justify-content-between align-items-center {mod}">
    <UserLinkPlaceholder />
    <Button size="sm" mod="placeholder col-3" disabled></Button>
</li>