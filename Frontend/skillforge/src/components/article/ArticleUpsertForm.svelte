<script lang="ts">
	import { writable } from 'svelte/store';
	import type ArticleUpsertFormData from '$lib/types/ArticleUpsertFormData';
	import Form from '../form/Form.svelte';
	import Button from '../button/Button.svelte';
	import InputField from '../form/InputField.svelte';
	import SelectField from '../form/SelectField.svelte';
	import Option from '../form/Option.svelte';
	import TextEditor from '../form/TextEditor.svelte';
	import ImageLoader from '../form/ImageLoader.svelte';
	import { getContext } from 'svelte';
	import type ArticleUpsertPageModel from '$lib/types/ArticleUpsertPageModel';
	import TagComboBox from '$components/form/TagComboBox.svelte';
	import type ValidationRules from '$lib/types/ValidationRules';
	import { regex, required } from '$lib/validation/rules';

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
            },
		],
		Title: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
		],
		Content: [
            {
                validate: required,
                message: (label) => `The ${label} field is required.`
            },
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
				message: () => `Invalid tag name.`,
			}
		]
	};

	function onSuccess() {
		console.log('Article created successfully');
	}
</script>

<h1 class="text-center">{pageContext.CurrentState?.Model?.Id == 0 ? 'Create' : 'Edit'} article</h1>

{#if !pageContext.CurrentState?.IsApproved}
	<div class="alert alert-info" role="alert">
		<i class="bi bi-info-circle-fill me-2"></i>
		This article is pending approval by the moderators. Please wait.
	</div>
{/if}

<Form action="/Article/Upsert" method="POST" formData={$formData} {validationRules} {onSuccess}>
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
			<div class="row">
				<div class="col-12 col-lg-4">
					<SelectField
						id="CategoryId"
						label="Category"
						options={pageContext.CategoryOptions}
						bind:value={$formData.CategoryId}
					>
						<Option value="0">Please select</Option>
					</SelectField>
				</div>
				<div class="col-12 col-lg-8">
					<InputField id="Title" type="text" bind:value={$formData.Title} />
				</div>
			</div>

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
