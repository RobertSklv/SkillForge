<script lang="ts">
	import Icon from "../../icon/Icon.svelte";
	import Navbar from "../navbar/Navbar.svelte";
	import NavLink from "../navbar/NavLink.svelte";
	import { currentUserStore, logoutUser } from "$lib/stores/currentUserStore";
	import NavDropdown from "../navbar/NavDropdown.svelte";
	import NavDropdownLink from "../navbar/NavDropdownLink.svelte";
	import Form from "../form/Form.svelte";
	import Button from "../button/Button.svelte";

  function logout() {
    logoutUser();
  }
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
  {#if $currentUserStore}
    <NavDropdown>
      {#snippet buttonSnippet()}
        <span>{$currentUserStore.Name}</span>
        <Icon type="person-circle" />
      {/snippet}
      <NavDropdownLink href="/account">
        Account
      </NavDropdownLink>
      <NavDropdownLink isButton={true} onclick={logout}>
          Logout
      </NavDropdownLink>
    </NavDropdown>
  {:else}
    <NavLink href="/join" isActive={false}>
        <span>Join us</span>
        <Icon type="person-circle" />
    </NavLink>
  {/if}
</Navbar>