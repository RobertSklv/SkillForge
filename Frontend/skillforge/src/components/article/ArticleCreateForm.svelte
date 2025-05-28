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

    let formData = writable<ArticleCreateFormData>({
        CategoryId: 0,
        Title: '',
        Content: '',
    });

</script>

<h1 class="text-center">Create article</h1>

<Form action="/Article/Create" method="POST" formData={$formData} onSuccess={() => {}} isMultipartFormData={true}>
    <div class="row">
        <div class="col-12 col-lg-8">
            <SelectField id="CategoryId" label="Category" bind:value={$formData.CategoryId}>
                <Option value="0">Please select</Option>
            </SelectField>
        </div>
        <div class="col-12 col-lg-4">
            <FileInputField id="Image" accept=".jpg, .jpeg, .png" bind:files={$formData.Image} />
        </div>
    </div>

    <InputField id="Title" type="text" bind:value={$formData.Title} />

    <TextEditor id="Content" height={600} bind:content={$formData.Content} />

    <div class="text-center">
        <Button isSubmitButton={true} size="lg">Create</Button>
    </div>
</Form>