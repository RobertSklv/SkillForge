import { useFormContext } from '@/context/FormContext';
import { FieldValues, RegisterOptions } from 'react-hook-form';

export interface IRadioFieldProps<T> {
    id: string;
    name: string;
    label?: string;
    value?: T;
    selectedOption: T;
    onChange?: (value: T) => void;
    options?: RegisterOptions<FieldValues, string> | undefined;
    classes?: string;
    disabled?: boolean;
    isSwitch?: boolean;
    isInline?: boolean;
    isReverse?: boolean;
}

export function RadioField<T extends string | number | readonly string[] | undefined>({
    id,
    name,
    label = name,
    value,
    selectedOption,
    onChange,
    options,
    classes = '',
    disabled,
    isSwitch,
    isInline,
    isReverse,
}: IRadioFieldProps<T>) {
    const formContext = useFormContext();

    return (
        <div className={`form-check ${classes} ${isSwitch ? 'form-switch' : ''} ${isInline ? 'form-check-inline' : ''} ${isReverse ? 'form-check-reverse' : ''}`}>
            <input id={id}
                {...formContext?.form.register(name, {
                    ...options,
                    onChange: event => onChange?.(event.target.value)
                })}
                value={value}
                checked={selectedOption == value}
                type="radio"
                className="form-check-input"
                disabled={disabled}
            />
            <label htmlFor={id} className="form-check-label">{label}</label>
        </div>
    );
}
