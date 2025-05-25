<script lang="ts">
    import Form from "../../components/form/Form.svelte";
    import FormField from "../../components/form/FormField.svelte";
	import type ValidationRules from "$lib/types/ValidationRules";
	import { required } from "$lib/validation/rules";
	import type UserLoginCredentials from "$lib/types/UserLoginCredentials";
	import { writable } from "svelte/store";
	import Button from "../../components/button/Button.svelte";

    let formData = writable<UserLoginCredentials>({
        UsernameOrEmail: '',
        Password: ''
    });

    let formComponent = null;

    let validationRules: ValidationRules = {
        usernameOrEmail: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            }
        ],
        password: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            }
        ]
    };
</script>

<h1>Sign in</h1>

<Form action="/User/Auth" method="POST" formData={$formData} {validationRules} bind:this={formComponent}>
    <div class="row">
        <div class="col-12 col-md-6">
            <FormField id="UsernameOrEmail" type="text" label="Username or e-mail" bind:value={$formData.UsernameOrEmail} />
        </div>
        <div class="col-12 col-md-6">
            <FormField id="Password" type="password" label="Password" bind:value={$formData.Password} />
        </div>
    </div>

    <Button size="lg" onclick={formComponent?.submit}>Sign in</Button>
</Form>