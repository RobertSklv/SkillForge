<script lang="ts">
	import type { BootstrapColor } from "$lib/types/BootstrapColor";
	import type UserLink from "$lib/types/UserLinkType";
	import { getImagePath } from "$lib/util";
	import Link from "./Link.svelte";

	interface Props {
		data: UserLink;
		size?: 'normal' | 'small';
		background?: 'outline' | 'fill';
		color?: BootstrapColor;
		muted?: boolean;
	}

	let {
		data,
		size,
		background,
		color,
		muted
	}: Props = $props();
</script>

<Link href="/user/{data.Name}" {size} {background} {color} borderRadius="pill" {muted}>
    <div class="d-flex align-items-center">
        <img src="{getImagePath(data.AvatarImage)}" class="user-link__image border border-1 round-image me-1" alt="{data.Name} avatar">
        <span>{data.Name}</span>
    </div>
</Link>

<style lang="scss">
    .user-link {
        &__image {
            height: 20px;
        }
    }
</style>