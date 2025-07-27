import { FormContext } from '../../../context/FormContext';
import { useContext } from 'react';
import type { FieldValues, RegisterOptions } from 'react-hook-form';
import type { InputType } from 'skillforge-common/types/InputType';

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
}

export function InputField(props: IInputFieldProps) {
    const formContext = useContext(FormContext);

    const formClass = props.type === 'range' ? 'form-range' : 'form-control';
    const formName: string = props.name ?? props.id;

    return (
        <div className="mb-4">
            <label htmlFor={props.id} className="form-label">{props.label}:</label>
            <input
                    {...formContext?.form.register(formName, {
                        ...props.options,
                        onChange: e => props.onChange?.(e.target.value)
                    })}
                    id={props.id}
                    type={props.type}
                    placeholder={props.placeholder}
                    className={`
                        ${formClass}
                        ${props.classes ?? ''}
                        ${(!!formContext?.form.formState.errors[formName]) ? 'is-invalid' : ''}`}
                    value={props.value}
                    min={props.min}
                    max={props.max}
                    minLength={props.minLength}
                    maxLength={props.maxLength}
                    step={props.step}
                    disabled={props.disabled}
            />
            <div className="invalid-feedback">
                {formContext?.form.formState.errors[formName] && formContext?.form.formState.errors[formName]?.message as string}
            </div>
        </div>
    );
}
