'use client'

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
    let quillEditor = useRef<any>(null);

    const privateContent = useRef<string | undefined>(undefined);

    function onTextChange() {
        privateContent.current = quillEditor.current.getSemanticHTML();
        onContentChange?.(privateContent.current);

        formContext.form.setValue(name, privateContent.current, {
            shouldValidate: true
        });
    }

    function updateEditorContent() {
        if (!content) {
            content = '';
        }

        privateContent.current = content;
        let convertedContent = quillEditor.current.clipboard.convert({
            html: privateContent.current
        });
        quillEditor.current.setContents(convertedContent);

        formContext.form.setValue(name, privateContent.current, {
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

        quillEditor.current = new Quill('#' + id, {
            modules: {
                toolbar: toolbarOptions
            },
            theme: 'snow',
        });

        privateContent.current = content;
        let convertedContent = quillEditor.current.clipboard.convert({
            html: privateContent.current
        });
        quillEditor.current.setContents(convertedContent);

        quillEditor.current.on('text-change', onTextChange);
        quillEditor.current.on('selection-change', onSelectionChange);

        quillEditor.current.getModule('toolbar').addHandler('image', () => {
            const input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', '.jpg, .jpeg, .png');
            input.click();

            input.onchange = async () => {
                const file = input.files?.[0];
                if (file) {
                    const url = process.env.NEXT_PUBLIC_BACKEND_DOMAIN + (await uploadImage(file, imageUploadType));

                    const range = quillEditor.current.getSelection();
                    if (range) {
                        quillEditor.current.insertEmbed(range.index, 'image', url);
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

    useEffect(() => {
        if (quillEditor && content !== privateContent.current) {
            updateEditorContent();
        }
    }, [quillEditor, content]);

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
