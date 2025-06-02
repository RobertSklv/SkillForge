<script lang="ts">
	import type UserLink from "$lib/types/UserLinkType";

    interface Props {
        data: UserLink
    }

    let {
        data
    }: Props = $props();
</script>

<a href="/user/{data.Name}" class="user-link btn btn-outline-primary rounded-pill shadow-none py-1">
    <div class="d-flex align-items-center">
        <img src="{data.AvatarImage ?? '/user.png'}" class="user-link__image border border-1 rounded-circle me-1" alt="{data.Name} avatar">
        <span>{data.Name}</span>
    </div>
</a>

<style lang="scss">
    .user-link {
        &__image {
            height: 20px;
        }
    }
</style>