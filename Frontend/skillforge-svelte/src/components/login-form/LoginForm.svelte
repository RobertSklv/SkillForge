<script lang="ts">
	import Form from '../form/Form.svelte';
	import InputField from '../form/input-field/InputField.svelte';
	import type ValidationRules from 'skillforge-common/types/ValidationRules';
	import { required } from 'skillforge-common/validation/rules';
	import type UserLoginCredentials from 'skillforge-common/types/UserLoginCredentials';
	import { writable } from 'svelte/store';
	import Button from '../button/Button.svelte';
	import type UserInfo from 'skillforge-common/types/UserInfo';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import { goto, invalidate } from '$app/navigation';
	import { addToast } from '$lib/stores/toastStore';
	import type LoginResponse from 'skillforge-common/types/LoginResponse';
	import { storeAuthToken } from 'skillforge-common/auth';

	let formData = writable<UserLoginCredentials>({
		UsernameOrEmail: '',
		Password: ''
	});

	let validationRules: ValidationRules = {
		UsernameOrEmail: [
			{
				validate: required,
				message: () => `Please enter a valid username or e-mail address.`
			}
		],
		Password: [
			{
				validate: required,
				message: () => `Please enter your password.`
			}
		]
	};

	async function onLoginSuccess(response: LoginResponse) {
		currentUserStore.set(response.CurrentUserInfo);
		storeAuthToken(response.AuthToken);
		await invalidate('app:auth');

		addToast('Successfully logged in');

		goto('/user/' + response.CurrentUserInfo.Name);
	}
</script>

<section class="mb-5">
	<h2>Sign in</h2>
	<Form
		action="/User/Auth"
		method="POST"
		formData={$formData}
		onSuccess={onLoginSuccess}
		{validationRules}
		resetMode="onsubmit"
	>
		<div class="row">
			<div class="col-12">
				<InputField
					id="UsernameOrEmail"
					type="text"
					label="Username or e-mail"
					bind:value={$formData.UsernameOrEmail}
				/>
			</div>
			<div class="col-12">
				<InputField
					id="Password"
					type="password"
					label="Password"
					bind:value={$formData.Password}
				/>
			</div>
		</div>

        <div class="text-center text-lg-start">
		    <Button size="lg" isSubmitButton={true}>Sign in</Button>
        </div>
	</Form>
</section>
