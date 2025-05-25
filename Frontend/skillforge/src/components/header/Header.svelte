<script lang="ts">
	import { getContext } from "svelte";
	import Icon from "../../icon/Icon.svelte";
	import Navbar from "../navbar/Navbar.svelte";
	import NavDropdown from "../navbar/NavDropdown.svelte";
	import NavDropdownLink from "../navbar/NavDropdownLink.svelte";
	import NavLink from "../navbar/NavLink.svelte";
	import type UserInfo from "$lib/types/UserInfo";

  const currentUser: UserInfo | null = getContext('currentUser');
</script>

<Navbar logoLink="/">
  {#snippet logoSnippet()}
    SkillForge
  {/snippet}

  <NavLink href="#" isActive={true}>
    Articles
  </NavLink>
  <NavLink href="#" isActive={false}>
    About us
  </NavLink>
  <NavLink href="#" isActive={false}>
    Contact us
  </NavLink>
  {#if !!currentUser}
    <NavDropdown>
      {#snippet buttonSnippet()}
        <span>{currentUser?.Name}</span>
        <Icon type="person-circle" />
      {/snippet}
      <NavDropdownLink href="#">
        Account
      </NavDropdownLink>
      <NavDropdownLink href="#">
        Logout
      </NavDropdownLink>
    </NavDropdown>
  {:else}
    <NavDropdown>
      {#snippet buttonSnippet()}
        <span>Sign in</span>
        <Icon type="person-circle" />
      {/snippet}
      <NavDropdownLink href="/signin">
        Sign in
      </NavDropdownLink>
      <NavDropdownLink href="/register">
        Register
      </NavDropdownLink>
    </NavDropdown>
  {/if}
</Navbar>