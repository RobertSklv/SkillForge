import React from 'react';
import { useForm } from 'react-hook-form';
import { FormContext } from '../../context/FormContext';
import { requestApi } from "@/lib/api/client";
import type ErrorResponse from "@/lib/types/ErrorResponse";

export interface IFormProps<T> {
    action: string;
    method?: 'GET' | 'POST' | 'DIALOG';
    isMultipartFormData?: boolean;
    onSubmit?: (data: any) => any;
    onSuccess: (response: T) => void;
    children: React.ReactNode;
}

export function Form<T>({
    action,
    method = 'POST',
    isMultipartFormData = false,
    onSubmit = data => data,
    onSuccess,
    children,
}: IFormProps<T>) {
    const form = useForm({
        mode: 'onTouched'
    });

    async function submit(data: any) {
        let init: RequestInit = {
            method: method,
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
            }
        };

        if (!isMultipartFormData) {
            init.headers = {
                ...init.headers,
                'Content-Type': 'application/json',
            };

            init.body = JSON.stringify(data);
        } else {
            let fd = new FormData();
            for (let fieldName in data) {
                fd.append(fieldName, data[fieldName] as string | Blob);
            }

            init.body = fd;
        }

        return requestApi(action, {
            init
        })
            .then(r => {
                onSuccess(r);
            })
            .catch((e: ErrorResponse) => {
                for (let fieldName in e.errors) {
                    let errors = e.errors[fieldName];

                    if (errors.length > 0) {
                        form.setError(fieldName, {
                            type: "backend",
                            message: errors[0]
                        });
                    }
                }

                if (e.detail || e.title) {
                    console.error(e.detail ?? e.title);
                }
            });
    }

    return (
        <form onSubmit={form.handleSubmit(data => submit(onSubmit(data)))}>
            <FormContext.Provider value={{
                form,
                submit: () => form.handleSubmit(data => submit(onSubmit(data)))()
            }}>
                {children}
            </FormContext.Provider>
        </form>
    );
}
