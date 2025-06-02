<script lang="ts">
	import type TagLink from "$lib/types/TagLinkType";

    interface Props {
        data: TagLink
    }

    let {
        data
    }: Props = $props();
</script>

<a href="/tag/{data.Name}" class="btn btn-outline-primary rounded-3 shadow-none ps-2 py-1">
    <i class="bi bi-hash"></i>{data.Name}
</a>