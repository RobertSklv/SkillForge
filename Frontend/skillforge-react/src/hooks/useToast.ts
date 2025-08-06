import type { ToastType } from "@/lib/types/ToastType";
import { create } from "zustand";

interface Toast {
    uid: string;
    message: string;
    type: ToastType;
}

interface Store {
    toasts: Toast[];
    addToast: (message: string, type?: ToastType, lifetime?: number) => void;
    removeToast: (uid: string) => void;
}

export const useToast = create<Store>((set) => ({
    toasts: [],
    addToast: (message: string, type: ToastType = 'success', lifetime: number = 5000) => {
        let toast: Toast = {
            uid: crypto.randomUUID(),
            message,
            type,
        };

        set(state => ({
            toasts: [
                ...state.toasts,
                toast
            ]
        }));

        setTimeout(() => {
            set(state => ({
                toasts: state.toasts.filter(t => t.uid !== toast.uid)
            }));
        }, lifetime);
    },
    removeToast: (uid: string) => {
        set(state => ({
            toasts: state.toasts.filter(t => t.uid !== uid)
        }));
    }
}));