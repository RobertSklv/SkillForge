<script lang="ts">
	import { writable } from 'svelte/store';
	import type ArticleUpsertFormData from '$lib/types/ArticleUpsertFormData';
	import Form from '../form/Form.svelte';
	import Button from '../button/Button.svelte';
	import InputField from '../form/input-field/InputField.svelte';
	import TextEditor from '../form/text-editor/TextEditor.svelte';
	import ImageLoader from '../form/image-loader/ImageLoader.svelte';
	import { getContext } from 'svelte';
	import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';
	import TagComboBox from '$components/form/tag-combo-box/TagComboBox.svelte';
	import type ValidationRules from '$lib/types/ValidationRules';
	import { regex, required } from '$lib/validation/rules';
	import { addToast } from '$lib/stores/toastStore';
	import Icon from '$components/icon/Icon.svelte';

	let pageContext = getContext<ArticleUpsertPageModel>('page');

	let formData = writable<ArticleUpsertFormData>(
		pageContext.CurrentState?.Model ?? {
			CategoryId: 0,
			Title: '',
			Content: '',
			Tags: []
		}
	);

	let validationRules: ValidationRules = {
		CategoryId: [
			{
				validate: (v) => v != 0,
				message: (label) => `The ${label} field is required.`
			}
		],
		Title: [
			{
				validate: required,
				message: (label) => `The ${label} field is required.`
			}
		],
		Content: [
			{
				validate: required,
				message: (label) => `The ${label} field is required.`
			}
		],
		Tags: [
			{
				validate: (tags) => {
					let isValid = true;

					for (let tagName of tags) {
						if (!regex(tagName, /^(?:[a-z][a-z0-9]*(?:_[a-z][a-z0-9]*)*)$/)) {
							isValid = false;
							break;
						}
					}

					return isValid;
				},
				message: () => `Invalid tag name.`
			}
		]
	};

	function onSuccess() {
		if (pageContext.CurrentState?.Model.Id == 0) {
			addToast('Article successfully created. Pending moderator approval.', 'info');
		} else {
			addToast('Article successfully updated. Pending moderator approval.', 'info');
		}
	}
</script>

<h1 class="text-center">{!pageContext.CurrentState?.Model?.Id ? 'Create' : 'Edit'} article</h1>

{#if pageContext.CurrentState && !pageContext.CurrentState.IsApproved}
	<div class="alert alert-info" role="alert">
		<Icon type="info-circle-fill" mod="me-2" />
		This article is pending approval by the moderators. Please wait.
	</div>
{/if}

<Form
	action="/Article/Upsert"
	method="POST"
	formData={$formData}
	{validationRules}
	{onSuccess}
	resetMode={pageContext.CurrentState?.Model.Id == 0 ? 'onsuccess' : 'never'}
>
	<input type="hidden" name="Id" bind:value={$formData.Id} />

	<div class="row">
		<div class="col-12 col-lg-3">
			<ImageLoader
				id="Image"
				label="Cover image"
				bind:url={$formData.Image}
				imageUploadType="article"
				imageAlt="Article cover image"
			/>
		</div>
		<div class="col-12 col-lg-9">
			<InputField id="Title" type="text" bind:value={$formData.Title} />

			<TagComboBox id="Tags" label="Tags:" bind:tags={$formData.Tags} />
		</div>
	</div>

	<TextEditor
		id="Content"
		height={600}
		bind:content={$formData.Content}
		imageUploadType="article"
	/>

	<div class="text-center">
		<Button isSubmitButton={true} size="lg">Publish</Button>
	</div>
</Form>
