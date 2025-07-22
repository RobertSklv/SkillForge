import type FormContext from "lib/types/FormContext";
import { createContext, useContext } from "react";

export const FormContext = createContext<FormContext | undefined>(undefined);

export function useFormContext() {
    const context = useContext(FormContext);

    if (!context) throw new Error('useFormContext must be used inside FormContextProvider');

    return context;
}