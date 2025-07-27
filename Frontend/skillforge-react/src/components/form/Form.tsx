import React from 'react';
import { useForm } from 'react-hook-form';
import { FormContext } from '../../context/FormContext';
import { requestApi } from "skillforge-common/api/client";
import type ErrorResponse from "skillforge-common/types/ErrorResponse";

export interface IFormProps<T> {
    action: string;
    method?: 'GET' | 'POST' | 'DIALOG';
    isMultipartFormData?: boolean;
    onSuccess: (response: T) => void;
    children: React.ReactNode;
}

export function Form<T>(props: IFormProps<T>) {
    const form = useForm({
        mode: 'onTouched'
    });

    async function submit(data: any) {
        let init: RequestInit = {
            method: props.method,
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
            }
        };

        if (!props.isMultipartFormData) {
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

        return requestApi(props.action, {
            init
        })
            .then(r => {
                props.onSuccess(r);
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
        <form onSubmit={form.handleSubmit(submit)}>
            <FormContext.Provider value={{
                form,
                submit: () => form.handleSubmit(submit)()
            }}>
                {props.children}
            </FormContext.Provider>
        </form>
    );
}
