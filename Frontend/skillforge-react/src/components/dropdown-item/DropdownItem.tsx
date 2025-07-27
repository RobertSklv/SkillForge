import * as React from 'react';
import Link from 'next/link';

export interface IDropdownItemProps {
    href?: string;
    type?: 'link' | 'button' | 'text';
    onClick?: React.MouseEventHandler<HTMLButtonElement>;
    active?: boolean;
    disabled?: boolean;
    children: React.ReactNode;
}

export function DropdownItem(props: IDropdownItemProps) {
    return (
        <li>
            {props.type === 'button' ? (
                <button onClick={props.onClick} className={`dropdown-item ${props.active ? 'active' : ''} ${props.disabled ? 'disabled' : ''}`}>
                    {props.children}
                </button>
            ) : props.type === 'link' ? (
                <Link href={props.href ?? ''} className={`dropdown-item ${props.active ? 'active' : ''} ${props.disabled ? 'disabled' : ''}`}>
                    {props.children}
                </Link>
            ) : props.type === 'text' ? (
                <span className="dropdown-item-text">
                    {props.children}
                </span>
            ) : ''}
        </li>
    );
}
