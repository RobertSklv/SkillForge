<script lang="ts">
	import Icon from "../../icon/Icon.svelte";
	import Navbar from "../navbar/Navbar.svelte";
	import NavDropdown from "../navbar/NavDropdown.svelte";
	import NavDropdownLink from "../navbar/NavDropdownLink.svelte";
	import NavLink from "../navbar/NavLink.svelte";

    
</script>

<Navbar logoLink="#">
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
  <NavDropdown>
    {#snippet buttonSnippet()}
      <Icon type="person-circle" />
    {/snippet}
    <NavDropdownLink href="#">
      Account
    </NavDropdownLink>
    <NavDropdownLink href="#">
      Logout
    </NavDropdownLink>
  </NavDropdown>
</Navbar>