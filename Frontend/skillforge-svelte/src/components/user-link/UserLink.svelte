<script lang="ts">
	import type { BootstrapColor } from "skillforge-common/types/BootstrapColor";
	import type UserLink from "skillforge-common/types/UserLinkType";
	import { getImagePath } from "skillforge-common/util";
	import Link from "../link/Link.svelte";
	import './_user-link.scss';

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

<Link href="/user/{data.Name}" {size} {background} {color} borderRadius="pill" mod="user-link" {muted}>
    <div class="d-flex align-items-center">
        <img src="{getImagePath(data.AvatarImage)}" class="user-link__image border border-1 round-image me-1" alt="{data.Name} avatar">
        <span class="text-truncate">{data.Name}</span>
    </div>
</Link>