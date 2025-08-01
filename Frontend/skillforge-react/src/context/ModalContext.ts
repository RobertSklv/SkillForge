import type ModalContext from "../lib/types/ModalContext";
import { createContext, useContext } from "react";

export const ModalContext = createContext<ModalContext | undefined>(undefined);

export function useModalContext() {
    const context = useContext(ModalContext);

    if (!context) throw new Error('useModalContext must be used inside ModalContextProvider');

    return context;
}