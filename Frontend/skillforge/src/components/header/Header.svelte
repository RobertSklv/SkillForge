<script lang="ts">
	import Icon from '../../icon/Icon.svelte';
	import Navbar from '../navbar/Navbar.svelte';
	import NavLink from '../navbar/NavLink.svelte';
	import { currentUserStore, logoutUser } from '$lib/stores/currentUserStore';
	import Dropdown from '$components/dropdown/Dropdown.svelte';
	import DropdownItem from '$components/dropdown/DropdownItem.svelte';
	import { goto } from '$app/navigation';

	async function logout() {
		await logoutUser();

		goto('/join');
	}
</script>

<header>
	<Navbar logoLink="/">
		{#snippet logoSnippet()}
			SkillForge
		{/snippet}

		<NavLink href="#" isActive={true}>Articles</NavLink>
		<NavLink href="#" isActive={false}>About us</NavLink>
		<NavLink href="#" isActive={false}>Contact us</NavLink>
		{#if $currentUserStore}
			<Dropdown isNav={true}>
				{#snippet buttonSnippet()}
					<span>{$currentUserStore.Name}</span>
					<Icon type="person-circle" />
				{/snippet}
				<DropdownItem href="/account">Account</DropdownItem>
				<DropdownItem type="button" onclick={logout}>Logout</DropdownItem>
			</Dropdown>
		{:else}
			<NavLink href="/join" isActive={false}>
				<span>Join us</span>
				<Icon type="person-circle" />
			</NavLink>
		{/if}
	</Navbar>
</header>
