import { ChangeEventHandler } from "react";

export interface IFileInputFieldProps {
    id: string;
    name?: string;
    label?: string;
    onFilesChange?: (files: FileList | null) => void,
    value?: string;
    onValueChange?: (value: string) => void;
    accept?: string;
    classes?: string;
    disabled?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
}

export function FileInputField({
    id,
    name = id,
    label = name,
    onFilesChange,
    value,
    onValueChange,
    accept,
    classes,
    disabled,
    onChange,
}: IFileInputFieldProps) {
    return (
        <div className="mb-4">
            <label htmlFor={id} className="form-label">{label}:</label>
            <input
                className={`form-control ${classes}`}
                type="file"
                id={id}
                name={name}
                accept={accept}
                value={value}
                onChange={e => {
                    onChange?.(e);
                    onValueChange?.(e.target.value);
                    onFilesChange?.(e.target.files);
                }}
                disabled={disabled}
            />
        </div>
    );
}
