'use client'

import { Form } from '@/components/form/Form';
import { Button } from '@/components/button/Button';
import { InputField } from '@/components/form/input-field/InputField';
import { Icon } from '@/components/icon/Icon';
import { TagComboBox } from '@/components/form/tag-combo-box/TagComboBox';
import { ImageLoader } from '@/components/form/image-loader/ImageLoader';
import { regex } from '@/lib/validation/rules';
import { useState } from 'react';
import ArticleUpsertFormData from '@/lib/types/ArticleUpsertFormData';
import { TextEditor } from '@/components/form/text-editor/TextEditor';
import { useToast } from '@/hooks/useToast';
import ArticleUpsertPageModel from '@/lib/types/ArticleUpsertPageModel';

export interface IArticleUpsertFormProps {
    page: ArticleUpsertPageModel
}

export function ArticleUpsertForm({ page }: IArticleUpsertFormProps) {
    const { addToast } = useToast();

    const [id, setId] = useState<number>(page.CurrentState?.Model.Id ?? 0);
    const [image, setImage] = useState<string | undefined>(page.CurrentState?.Model.Image);
    const [title, setTitle] = useState<string>(page.CurrentState?.Model.Title ?? '');
    const [content, setContent] = useState<string | undefined>(page.CurrentState?.Model.Content);
    const [tags, setTags] = useState<string[]>(page.CurrentState?.Model.Tags ?? []);

    function validateTags(tags: string[]) {
        let isValid = true;

        for (let tagName of tags) {
            if (!regex(tagName, /^(?:[a-z][a-z0-9]*(?:_[a-z][a-z0-9]*)*)$/)) {
                isValid = false;
                break;
            }
        }

        return isValid;
    }

	function onSuccess() {
		if (page.CurrentState?.Model.Id == 0) {
			addToast('Article successfully created. Pending moderator approval.', 'info');
		} else {
			addToast('Article successfully updated. Pending moderator approval.', 'info');
		}
	}

    return (
        <>
            <h1 className="text-center">{!page.CurrentState?.Model?.Id ? 'Create' : 'Edit'} article</h1>

            {(page.CurrentState && !page.CurrentState.IsApproved) &&
                <div className="alert alert-info" role="alert">
                    <Icon type="info-circle-fill" classes="me-2" />
                    This article is pending approval by the moderators. Please wait.
                </div>
            }

            <Form<ArticleUpsertFormData>
                action="/Article/Upsert"
                method="POST"
                onSuccess={onSuccess}
            >
                <input type="hidden" name="Id" value={id} />

                <div className="row">
                    <div className="col-12 col-lg-3">
                        <ImageLoader
                            id="Image"
                            label="Cover image"
                            url={image}
                            onUrlChange={setImage}
                            imageUploadType="article"
                            imageAlt="Article cover image"
                        />
                    </div>
                    <div className="col-12 col-lg-9">
                        <InputField id="Title" type="text" value={title} onChange={setTitle} />

                        <TagComboBox id="Tags" label="Tags:" tags={tags} onTagsChange={setTags} options={{
                            validate: validateTags
                        }} />
                    </div>
                </div>

                <TextEditor
                    id="Content"
                    height={600}
                    content={content}
                    imageUploadType="article"
                />

                <div className="text-center">
                    <Button isSubmitButton={true} size="lg">Publish</Button>
                </div>
            </Form>
        </>
    );
}
