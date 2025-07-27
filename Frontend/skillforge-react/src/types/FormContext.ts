import type { FieldValues, UseFormReturn } from "react-hook-form";

export default interface FormContext {
    form: UseFormReturn<FieldValues, any, FieldValues>,
    submit: () => Promise<void>;
}