'use client'

import { FormContext } from '../../context/FormContext';
import React, { useContext, useState, type MouseEvent, type MouseEventHandler } from 'react';
import type { BootstrapColor } from "@/lib/types/BootstrapColor";
import type { BootstrapSize } from "@/lib/types/BootstrapSize";

export interface IButtonProps {
    color?: BootstrapColor;
    size?: BootstrapSize;
    classes?: string;
    isOutline?: boolean;
    isSubmitButton?: boolean;
    disabled?: boolean;
    onClick?: MouseEventHandler<HTMLButtonElement>;
    children?: React.ReactNode;
}

export function Button(props: IButtonProps) {

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const colorClass = `btn${props.isOutline ? '-outline' : ''}-${props.color ?? 'primary'}`;

    let formContext = useContext(FormContext);

    async function onClickPrivate(event: MouseEvent<HTMLButtonElement>) {
        setIsLoading(true);

        if (props.isSubmitButton) {
            await formContext?.submit();
        }

        props.onClick?.(event);

        setIsLoading(false);
    }

    return (
        <button type="button"
            className={`btn ${colorClass} ${props.classes ?? ''} ${props.size ? 'btn-' + props.size : ''} ${props.disabled ? 'disabled' : ''}`}
            onClick={onClickPrivate}
            disabled={props.disabled || isLoading}>
            {isLoading ?
                <span>
                    <span className="spinner-border spinner-border-sm" aria-hidden="true"></span>
                    <span role="status">Processing...</span>
                </span>
                :
                <span>{props.children}</span>
            }
        </button>
    );
}
