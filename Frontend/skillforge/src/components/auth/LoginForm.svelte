<script lang="ts">
    import Form from "../../components/form/Form.svelte";
    import InputField from "../form/InputField.svelte";
	import type ValidationRules from "$lib/types/ValidationRules";
	import { required } from "$lib/validation/rules";
	import type UserLoginCredentials from "$lib/types/UserLoginCredentials";
	import { writable } from "svelte/store";
	import Button from "../../components/button/Button.svelte";
	import type UserInfo from "$lib/types/UserInfo";
	import { currentUserStore } from "$lib/stores/currentUserStore";
	import { goto } from "$app/navigation";

    let formData = writable<UserLoginCredentials>({
        UsernameOrEmail: '',
        Password: ''
    });

    let validationRules: ValidationRules = {
        UsernameOrEmail: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            }
        ],
        Password: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            }
        ]
    };

    function onLoginSuccess(userInfo: UserInfo) {
        currentUserStore.set(userInfo);

        goto('/user/' + userInfo.Name);
    }
</script>

<h2>Sign in</h2>
<Form action="/User/Auth"
    method="POST"
    formData={$formData}
    onSuccess={onLoginSuccess}
    {validationRules}
    resetMode="onsubmit">
    <div class="row">
        <div class="col-12">
            <InputField id="UsernameOrEmail" type="text" label="Username or e-mail" bind:value={$formData.UsernameOrEmail} />
        </div>
        <div class="col-12">
            <InputField id="Password" type="password" label="Password" bind:value={$formData.Password} />
        </div>
    </div>

    <Button size="lg" isSubmitButton={true}>Sign in</Button>
</Form>