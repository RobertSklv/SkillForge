import { useFormContext } from '@/context/FormContext';
import { useEffect, useRef } from 'react';
import { uploadImage } from '@/lib/api/client';
import { ImageUploadType } from '@/lib/types/ImageUploadType';
import Quill from 'quill';
import 'quill/dist/quill.snow.css';
import { FieldValues, RegisterOptions } from 'react-hook-form';

export interface ITextEditorProps {
    id: string;
    name?: string;
    label?: string | null;
    content?: string;
    onContentChange?: (content: string | undefined) => void;
    options?: RegisterOptions<FieldValues, string> | undefined;
    height?: number;
    imageUploadType: ImageUploadType;
}

export function TextEditor({
    id,
    name = id,
    label = name,
    content,
    onContentChange,
    options,
    height = 300,
    imageUploadType,
}: ITextEditorProps) {
    const formContext = useFormContext();
    const isInvalid = !!formContext?.form.formState.errors[id];

    let editorWrapperElement = useRef<HTMLDivElement>(null);
    let quillEditor: any;

    function onTextChange() {
        content = quillEditor.getSemanticHTML();

        onContentChange?.(content);

        formContext.form.setValue(name, content, {
            shouldValidate: true
        });
    }

    function onSelectionChange() {
        formContext.form.getFieldState(name).isDirty = true;
    }

    useEffect(() => {
        formContext.form.register(name, options);

        const toolbarOptions = [
            ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
            ['blockquote', 'code-block'],
            ['link', 'image', 'formula'],

            [{ 'header': 1 }, { 'header': 2 }],               // custom button values
            [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'list': 'check' }],
            [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
            [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
            [{ 'direction': 'rtl' }],                         // text direction

            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

            [{ 'color': [] }],                                // dropdown with defaults from theme

            ['clean']                                         // remove formatting button
        ];

        quillEditor = new Quill('#' + id, {
            modules: {
                toolbar: toolbarOptions
            },
            theme: 'snow',
        });

        let convertedContent = quillEditor.clipboard.convert({
            html: content
        });
        quillEditor.setContents(convertedContent);

        quillEditor.on('text-change', onTextChange);
        quillEditor.on('selection-change', onSelectionChange);

        quillEditor.getModule('toolbar').addHandler('image', () => {
            const input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', '.jpg, .jpeg, .png');
            input.click();

            input.onchange = async () => {
                const file = input.files?.[0];
                if (file) {
                    const url = process.env.NEXT_PUBLIC_BACKEND_DOMAIN + (await uploadImage(file, imageUploadType));

                    const range = quillEditor.getSelection();
                    if (range) {
                        quillEditor.insertEmbed(range.index, 'image', url);
                    }
                }
            };
        });

        return () => {
            formContext.form.unregister(name);
            let editorElement = document.getElementById(id);

            if (editorElement && editorElement.parentElement) {
                editorElement.parentElement.innerHTML = `<div id="${id}" style="height: ${height}px"></div>`;
            }
        };
    }, []);

    return (
        <div className="mb-4">
            {label && <p className="form-label">{label}:</p>}
            <div className={`quill-editor-wrapper ${isInvalid ? 'is-invalid' : ''}`} ref={editorWrapperElement}>
                <div id={id} style={{ height: `${height}px` }}></div>
            </div>
            <div className="invalid-feedback">
                {isInvalid && formContext?.form.formState.errors[name]?.message as string}
            </div>
        </div>
    );
}
