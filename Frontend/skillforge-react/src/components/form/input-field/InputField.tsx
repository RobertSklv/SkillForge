import { useFormContext } from '../../../context/FormContext';
import type { FieldValues, RegisterOptions } from 'react-hook-form';
import type { InputType } from '@/lib/types/InputType';

export interface IInputFieldProps {
    id: string;
    name?: string;
    label?: string;
    type?: InputType;
    value?: any;
    onChange?: (value: any) => void;
    options?: RegisterOptions<FieldValues, string> | undefined,
    placeholder?: string;
    min?: any;
    max?: any;
    minLength?: any;
    maxLength?: any;
    step?: any;
    classes?: string;
    disabled?: boolean;
    validateTogether?: string[];
}

export function InputField({
    id,
    name = id,
    label = name,
    type = 'text',
    value,
    onChange,
    options,
    placeholder,
    min,
    max,
    minLength,
    maxLength,
    step,
    classes,
    disabled,
    validateTogether,
}: IInputFieldProps) {
    const formContext = useFormContext();

    const formClass = type === 'range' ? 'form-range' : 'form-control';

    const isInvalid = !!formContext?.form.formState.errors[name];

    function onChangePrivate(event: any) {
        onChange?.(event.target.value);

        validateTogether?.forEach(fieldName => {
            let state = formContext.form.getFieldState(fieldName);

            if (state.isDirty || state.isTouched) {
                formContext.form.trigger(fieldName);
            }
        });
    }

    return (
        <div className="mb-4">
            <label htmlFor={id} className="form-label">{label}:</label>
            <input
                {...formContext?.form.register(name, {
                    ...options,
                    onChange: onChangePrivate
                })}
                id={id}
                type={type}
                placeholder={placeholder}
                className={`
                        ${formClass}
                        ${classes ?? ''}
                        ${isInvalid ? 'is-invalid' : ''}`}
                value={value}
                min={min}
                max={max}
                minLength={minLength}
                maxLength={maxLength}
                step={step}
                disabled={disabled}
            />
            <div className="invalid-feedback">
                {isInvalid && formContext?.form.formState.errors[name]?.message as string}
            </div>
        </div>
    );
}
