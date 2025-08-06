import OptionType from "@/lib/types/OptionType";
import { ReactNode } from "react";
import { RadioField } from "../radio-field/RadioField";
import { useFormContext } from "@/context/FormContext";
import './_radio-group.scss';

export interface IRadioGroupProps {
    name: string;
    label?: string;
    options?: OptionType[];
    value?: any;
    onChange?: (value: any) => void;
    isSwitch?: boolean;
    isInline?: boolean;
    isReverse?: boolean;
    children?: ReactNode;
}

export function RadioGroup({
    name,
    label,
    options,
    value,
    onChange,
    isSwitch,
    isInline,
    isReverse,
    children,
}: IRadioGroupProps) {
    const formContext = useFormContext();
    const isInvalid = !!formContext?.form.formState.errors[name];

    return (
        <div className="mb-4">
            <div role="radiogroup" className={`radio-group ${isInvalid ? 'is-invalid' : ''}`}>
                {label && <strong className="d-inline-block mb-2">{label}:</strong>}
                {options && options?.map(({ Value, Label }) =>
                    <RadioField
                        id={`${name}-${Value}`}
                        key={Value}
                        name={name}
                        label={Label}
                        value={Value}
                        isSwitch={isSwitch}
                        isInline={isInline}
                        isReverse={isReverse}
                        selectedOption={value}
                        onChange={onChange}
                    />
                )}
                {children}
            </div>
            <div className="invalid-feedback">
                {isInvalid && formContext?.form.formState.errors[name]?.message as string}
            </div>
        </div>
    );
}
