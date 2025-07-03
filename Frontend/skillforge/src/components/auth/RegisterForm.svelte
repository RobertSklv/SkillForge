<script lang="ts">
	import Form from '../form/Form.svelte';
	import InputField from '../form/InputField.svelte';
	import type ValidationRules from '$lib/types/ValidationRules';
	import { compare, email, password, remote, required } from '$lib/validation/rules';
	import type UserRegisterCredentials from '$lib/types/UserRegisterCredentials';
	import { writable } from 'svelte/store';
	import Button from '../button/Button.svelte';
	import type UserInfo from '$lib/types/UserInfo';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import { goto, invalidate } from '$app/navigation';
	import { addToast } from '$lib/stores/toastStore';
	import { storeAuthToken } from '$lib/auth';
	import type RegisterResponse from '$lib/types/RegisterResponse';

	let formData = writable<UserRegisterCredentials>({
		Username: '',
		Email: '',
		Password: '',
		ConfirmPassword: ''
	});

	let validationRules: ValidationRules = {
		Username: [
			{
				validate: required,
				message: () => `Please enter a valid username.`
			},
			{
				validate: (val) => remote('/User/VerifyUniqueUsername', 'name', val),
				message: () => 'Username is already taken.'
			}
		],
		Email: [
			{
				validate: required,
				message: () => `Please enter a valid e-mail.`
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
				message: () => `Please enter a valid password.`
			},
			{
				validate: password,
				message: () =>
					'The password must be at least 8 characters long, contain at least one lowercase and uppercase letter, and at least one symbol.'
			}
		],
		ConfirmPassword: [
			{
				validate: required,
				message: () => `Please confirm your password.`
			},
			{
				validate: (val) => compare(val, $formData.Password),
				message: () => `Passwords don't match.`
			}
		]
	};

	async function onRegisterSuccess(response: RegisterResponse) {
		let userInfo: UserInfo = {
			Name: $formData.Username,
			Email: $formData.Email,
			AvatarPath: ''
		};

		currentUserStore.set(userInfo);
		storeAuthToken(response.AuthToken);
		await invalidate('app:auth');

		addToast('Registration successful');

		goto('/user/' + userInfo.Name);
	}
</script>

<section class="mb-5">
	<h2>Sign up</h2>
	<Form
		action="/User/Register"
		method="POST"
		formData={$formData}
		onSuccess={onRegisterSuccess}
		{validationRules}
		resetMode="onsuccess"
	>
		<div class="row">
			<div class="col-12">
				<InputField id="Username" type="text" label="Username" bind:value={$formData.Username} />
			</div>
			<div class="col-12">
				<InputField id="Email" type="email" label="E-mail" bind:value={$formData.Email} />
			</div>
			<div class="col-12">
				<InputField
					id="RegistrationPassword"
					name="Password"
					type="password"
					label="Password"
					bind:value={$formData.Password}
					validateTogether={['ConfirmPassword']}
				/>
			</div>
			<div class="col-12">
				<InputField
					id="ConfirmPassword"
					type="password"
					label="Confirm password"
					bind:value={$formData.ConfirmPassword}
				/>
			</div>
		</div>

        <div class="text-center text-lg-start">
		    <Button size="lg" isSubmitButton={true}>Sign up</Button>
        </div>
	</Form>
</section>
