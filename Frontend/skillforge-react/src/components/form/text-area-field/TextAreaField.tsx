import { useFormContext } from '@/context/FormContext';
import { FieldValues, RegisterOptions } from 'react-hook-form';

export interface ITextAreaFieldProps {
    id: string;
    name?: string;
    label?: string;
    value: string;
    onChange: (value: string) => void;
    options?: RegisterOptions<FieldValues, string> | undefined,
    placeholder?: string;
    rows?: any;
    cols?: any;
    minlength?: any;
    maxlength?: any;
    classes?: string;
    disabled?: boolean;
}

export function TextAreaField({
    id,
    name = id,
    label = name,
    value,
    onChange,
    options,
    placeholder,
    rows,
    cols,
    minlength,
    maxlength,
    classes = '',
    disabled,
}: ITextAreaFieldProps) {
    const formContext = useFormContext();
    const isInvalid = !!formContext?.form.formState.errors[name];

    return (
        <div className="mb-4">
            <label htmlFor={id} className="form-label">{label}:</label>
            <textarea id={id}
                {...formContext?.form.register(name, {
                    ...options,
                    onChange: event => onChange?.(event.target.value)
                })}
                placeholder={placeholder}
                className={`form-control ${classes} ${isInvalid ? 'is-invalid' : ''}`}
                value={value}
                rows={rows}
                cols={cols}
                minLength={minlength}
                maxLength={maxlength}
                disabled={disabled}></textarea>
            <div className="invalid-feedback">
                {isInvalid && formContext?.form.formState.errors[name]?.message as string}
            </div>
        </div>
    );
}
