<script lang="ts">
	import Button from '$components/button/Button.svelte';
	import Form from '$components/form/Form.svelte';
	import ImageLoader from '$components/form/ImageLoader.svelte';
	import InputField from '$components/form/InputField.svelte';
	import TextAreaField from '$components/form/TextAreaField.svelte';
	import Block from '$components/layout/Block.svelte';
	import ThreeColumns from '$components/layout/ThreeColumns.svelte';
	import { PUBLIC_BASE_URL } from '$env/static/public';
	import { loadAccountInfoForm } from '$lib/api/client';
	import { currentUserStore } from '$lib/stores/currentUserStore';
	import { addToast } from '$lib/stores/toastStore';
	import type AccountInfoFormData from '$lib/types/AccountInfoFormData';
	import type PasswordChangeFormData from '$lib/types/PasswordChangeFormData';
	import type ValidationRules from '$lib/types/ValidationRules';
	import { compare, email, maxLength, password, remote, required } from '$lib/validation/rules';
	import { onMount } from 'svelte';
	import { writable } from 'svelte/store';

	let loadPromise: Promise<any>;
	let accountInfoFormData = writable<AccountInfoFormData>();
	let passwordChangeFormData = writable<PasswordChangeFormData>({
		Password: '',
		ConfirmPassword: '',
		CurrentPassword: ''
	});

	async function onInfoUpdateSuccess() {
		addToast('Account info updated successfully');
	}

	async function onPasswordUpdateSuccess() {
		addToast('Password updated successfully');
	}

    let accountInfoValidationRules: ValidationRules = {
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
                validate: (val) => val == $currentUserStore?.Email || remote('/User/VerifyUniqueEmail', 'email', val),
                message: () => 'E-mail is already taken.'
            },
            {
                validate: (val) => maxLength(val, 32),
                message: () => 'The e-mail can be maximum 32 characters long.'
            },
        ],
        Bio: [
            {
                validate: required,
                message: () => `Please enter a valid e-mail.`
            },
            {
                validate: (val) => maxLength(val, 128),
                message: () => 'The biography can be maximum 128 characters long.'
            },
        ]
    };

    let changePasswordValidationRules: ValidationRules = {
        Password: [
            {
                validate: required,
                message: () => `Please enter a valid password.`
            },
            {
                validate: password,
                message: () => 'The password must be at least 8 characters long, contain at least one lowercase and uppercase letter, and at least one symbol.'
            },
        ],
        ConfirmPassword: [
            {
                validate: required,
                message: () => `Please confirm your new password.`
            },
            {
                validate: (val) => compare(val, $passwordChangeFormData.Password),
                message: () => `Passwords don't match.`
            }
        ],
        CurrentPassword: [
            {
                validate: required,
                message: () => `Please enter your current password.`
            },
        ],
    };

	onMount(async () => {
		loadPromise = loadAccountInfoForm();
		accountInfoFormData.set(await loadPromise);
	});
</script>

<svelte:head>
	<title>SkillForge | Edit account</title>
	<meta name="description" content="Edit account">
	<meta name="robots" content="noindex,nofollow">
	<link rel="canonical" href="{PUBLIC_BASE_URL}/user/{$currentUserStore?.Name}">
</svelte:head>

<ThreeColumns>
	{#if $accountInfoFormData}
		<Block mod="mb-5">
			<div class="card-body">
				<Form
					action="/User/UpdateInfo"
					formData={$accountInfoFormData}
					onSuccess={onInfoUpdateSuccess}
                    validationRules={accountInfoValidationRules}
				>
					<ImageLoader
						id="AvatarImage"
						imageAlt="{$currentUserStore?.Name} avatar"
						bind:url={$accountInfoFormData.AvatarImage}
						imageUploadType="avatar"
					/>

					<TextAreaField id="Bio" rows="5" bind:value={$accountInfoFormData.Bio} maxlength="128" />

					<InputField
						id="Email"
						label="E-mail"
						type="email"
						bind:value={$accountInfoFormData.Email}
					/>

					<InputField
						id="DateOfBirth"
						label="Date of birth"
						type="date"
						bind:value={$accountInfoFormData.DateOfBirth}
					/>

					<div class="text-center">
						<Button isSubmitButton>Update info</Button>
					</div>
				</Form>
			</div>
		</Block>
		<Block>
			<div class="card-body">
				<Form
					action="/User/UpdatePassword"
					formData={$passwordChangeFormData}
					onSuccess={onPasswordUpdateSuccess}
                    validationRules={changePasswordValidationRules}
                    resetMode="onsuccess"
				>
					<InputField id="Password" type="password" bind:value={$passwordChangeFormData.Password} />

					<InputField
						id="ConfirmPassword"
						label="Confirm password"
						type="password"
						bind:value={$passwordChangeFormData.ConfirmPassword}
					/>

					<InputField
						id="CurrentPassword"
						label="Current password"
						type="password"
						bind:value={$passwordChangeFormData.CurrentPassword}
					/>

					<div class="text-center">
						<Button isSubmitButton>Update info</Button>
					</div>
				</Form>
			</div>
		</Block>
	{:else}
		<div class="d-flex justify-content-center">
			<div class="spinner-border" role="status">
				<span class="visually-hidden">Loading...</span>
			</div>
		</div>
	{/if}
</ThreeColumns>
