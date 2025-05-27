<script lang="ts">
    import Form from "../form/Form.svelte";
    import FormField from "../form/FormField.svelte";
	import type ValidationRules from "$lib/types/ValidationRules";
	import { compare, email, password, remote, required } from "$lib/validation/rules";
	import type UserRegisterCredentials from "$lib/types/UserRegisterCredentials";
	import { writable } from "svelte/store";
	import Button from "../button/Button.svelte";
	import type UserInfo from "$lib/types/UserInfo";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import { goto } from "$app/navigation";

    let formData = writable<UserRegisterCredentials>({
        Username: '',
        Email: '',
        Password: '',
        ConfirmPassword: '',
    });

    let validationRules: ValidationRules = {
        Username: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
            {
                validate: (val) => remote('/User/VerifyUniqueUsername', 'name', val),
                message: () => 'Username is already taken.'
            }
        ],
        Email: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
            {
                validate: email,
                message: () => 'Invalid e-mail format.'
            },
            {
                validate: (val) => remote('/User/VerifyUniqueEmail', 'email', val),
                message: () => 'E-mail is already taken.'
            }
        ],
        Password: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
            {
                validate: password,
                message: () => 'The password must be at least 8 characters long, contain at least one lowercase and letter, and at least one symbol.'
            },
        ],
        ConfirmPassword: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
            {
                validate: (val) => compare(val, $formData.Password),
                message: () => `Passwords don't match.`
            }
        ],
    };

    function onRegisterSuccess() {
        let userInfo: UserInfo = {
            Name: $formData.Username,
            Email: $formData.Email,
            AvatarPath: ''
        };

        currentUserStore.set(userInfo);

        //TODO goto account page
        goto('/');
    }
</script>

<h2>Sign up</h2>
<Form action="/User/Register"
    method="POST"
    formData={$formData}
    onSuccess={onRegisterSuccess}
    {validationRules}>
    <div class="row">
        <div class="col-12">
            <FormField id="Username" type="text" label="Username" bind:value={$formData.Username} />
        </div>
        <div class="col-12">
            <FormField id="Email" type="text" label="E-mail" bind:value={$formData.Email} />
        </div>
        <div class="col-12">
            <FormField id="RegistrationPassword" name="Password" type="password" label="Password" bind:value={$formData.Password} validateTogether={["ConfirmPassword"]} />
        </div>
        <div class="col-12">
            <FormField id="ConfirmPassword" type="password" label="Confirm password" bind:value={$formData.ConfirmPassword} />
        </div>
    </div>

    <Button size="lg" isSubmitButton={true}>Sign up</Button>
</Form>