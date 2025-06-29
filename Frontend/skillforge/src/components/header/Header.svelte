<script lang="ts">
	import Icon from '$components/icon/Icon.svelte';
	import Navbar from '../navbar/Navbar.svelte';
	import NavLink from '../navbar/NavLink.svelte';
	import { currentUserStore, logoutUser } from '$lib/stores/currentUserStore';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownItem from '$components/dropdown/DropdownItem.svelte';
	import { goto } from '$app/navigation';
	import HeaderSearchBar from './HeaderSearchBar.svelte';
	import { page } from '$app/state';
	import { getImagePath } from '$lib/util';

	async function logout() {
		await logoutUser();

		goto('/join');
	}
</script>

<header>
	<Navbar logoLink="/">
		{#snippet logoSnippet()}
			<div class="d-flex justify-content-center align-items-center">
				<img src="/logo.png" class="me-1" style:height="22px" alt="Logo">
				<span class="d-none d-md-block">SkillForge</span>
			</div>
		{/snippet}

		{#if page.url.pathname !== '/search'}
			<HeaderSearchBar />
		{/if}

		{#snippet linksSnippet()}
			<NavLink href="/article/create">Create Article</NavLink>
			{#if $currentUserStore}
				<Dropdown isNav={true}>
					{#snippet buttonSnippet()}
						<span>{$currentUserStore.Name}</span>
						<img src={getImagePath($currentUserStore.AvatarPath)} width="20" height="20" class="rounded-circle border-1 ms-1 object-fit-cover" alt="Your avatar" />
					{/snippet}
					<DropdownItem href="/user/{$currentUserStore.Name}">Account</DropdownItem>
					<DropdownItem href="/account">Edit Account</DropdownItem>
					<DropdownItem href="/article/create">Create Article</DropdownItem>
					<DropdownItem type="button" onclick={logout}>Logout</DropdownItem>
				</Dropdown>
			{:else}
				<NavLink href="/join">
					<span>Join us</span>
					<Icon type="person-circle" />
				</NavLink>
			{/if}
		{/snippet}
	</Navbar>
</header>