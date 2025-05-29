<script lang="ts">
	import { writable } from "svelte/store";
	import type ArticleCreateFormData from "$lib/types/ArticleCreateFormData";
	import Form from "../form/Form.svelte";
	import FileInputField from "../form/FileInputField.svelte";
	import Button from "../button/Button.svelte";
	import InputField from "../form/InputField.svelte";
	import SelectField from "../form/SelectField.svelte";
	import Option from "../form/Option.svelte";
	import TextEditor from "../form/TextEditor.svelte";
	import ImageLoader from "../form/ImageLoader.svelte";
	import { getContext } from "svelte";
	import type ArticleCreatePageModel from "$lib/types/ArticleCreatePageModel";

    let pageContext = getContext<ArticleCreatePageModel>('page');

    let formData = writable<ArticleCreateFormData>({
        CategoryId: 0,
        Title: '',
        Content: '',
    });

    function onSuccess() {
        console.log('Article created successfully');
    }
</script>

<h1 class="text-center">Create article</h1>

<Form action="/Article/Create" method="POST" formData={$formData} {onSuccess} isMultipartFormData={true}>
    <div class="row">
        <div class="col-12 col-lg-3">
            <ImageLoader id="Image" label="Cover image" bind:url={$formData.Image} imageUploadType="article" imageAlt="Article cover image" />
        </div>
        <div class="col-12 col-lg-9">
            <SelectField id="CategoryId" label="Category" options={pageContext.CategoryOptions} bind:value={$formData.CategoryId}>
                <Option value="0">Please select</Option>
            </SelectField>

            <InputField id="Title" type="text" bind:value={$formData.Title} />
        </div>
    </div>

    <TextEditor id="Content" height={600} bind:content={$formData.Content} imageUploadType="article" />

    <div class="text-center">
        <Button isSubmitButton={true} size="lg">Publish</Button>
    </div>
</Form>