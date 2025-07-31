import type { ToastType } from "$lib/types/ToastType";
import { writable } from "svelte/store";

interface Toast {
    uid: string,
    message: string;
    type: ToastType;
}

interface Store {
    toasts: Toast[],
}

export var toastStore = writable<Store>({
    toasts: [],
});

export function addToast(message: string, type: ToastType = 'success', lifetime: number = 5000) {
    let toast: Toast = {
        uid: crypto.randomUUID(),
        message,
        type,
    };

    toastStore.update(state => {
        state.toasts.push(toast);

        return state;
    });

    setTimeout(() => {
        toastStore.update(state => {
            state.toasts.splice(state.toasts.indexOf(toast), 1);

            return state;
        });
    }, lifetime);
}

export function removeToast(uid: string) {
    toastStore.update(state => {
        state.toasts = state.toasts.filter(t => t.uid !== uid);

        return state;
    });
}