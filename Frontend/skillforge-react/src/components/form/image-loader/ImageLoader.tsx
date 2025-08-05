import { Button } from '@/components/button/Button';
import { Icon } from '@/components/icon/Icon';
import { useState } from 'react';
import { uploadImage } from '@/lib/api/client';
import { ImageUploadType } from '@/lib/types/ImageUploadType';
import { getBackendUrl } from '@/lib/util';
import { FileInputField } from '../file-input-field/FileInputField';
import { useFormContext } from '@/context/FormContext';
import Image from 'next/image';

export interface IImageLoaderProps {
    id: string;
    name?: string;
    label?: string;
    imageAlt: string;
    url: string | undefined;
    onUrlChange: (url?: string) => void;
    imageUploadType: ImageUploadType;
}

export function ImageLoader({
    id,
    name = id,
    label = name,
    imageAlt,
    url,
    onUrlChange,
    imageUploadType,
}: IImageLoaderProps) {
    const formContext = useFormContext();

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const hasFile = !!url;
    const [files, setFiles] = useState<FileList | null>();

    async function onFilesChange(files: FileList | null) {
        let file: File | undefined = files?.[0];

        if (file) {
            setIsLoading(true);
            url = await uploadImage(file, imageUploadType);
            setIsLoading(false);

            onUrlChange?.(url);
            formContext.form.setValue(name, url);
        }

        setFiles?.(files);
    }

    function onRemoveClick() {
        url = undefined;
        onUrlChange?.(url);
        formContext.form.setValue(name, url);
    }

    return (
        <div className="mb-4">
            <FileInputField
                id={id}
                name={name}
                label={label}
                accept=".jpg, .jpeg, .png"
                onFilesChange={onFilesChange}
            />
            {isLoading ?
                <div className="position-relative m-4">
                    <div className="d-flex justify-content-center align-items-center" style={{
                        height: '150px'
                    }}>
                        <strong role="status">Loading...</strong>
                        <div className="spinner-border ms-3" aria-hidden="true"></div>
                    </div>
                </div>
                : hasFile ?
                    <div className="m-4 d-flex justify-content-center">
                        <div className="position-relative">
                            <Image
                                src={getBackendUrl(url)}
                                alt={imageAlt}
                                className="rounded preview-image"
                                style={{ maxWidth: '250px' }}
                            />
                            <div className="d-flex justify-content-end position-absolute start-0 top-0 end-0">
                                <Button color="danger" onClick={onRemoveClick}>
                                    <Icon type="trash-fill" />
                                </Button>
                            </div>
                        </div>
                    </div>
                    : ''}
        </div>
    );
}
