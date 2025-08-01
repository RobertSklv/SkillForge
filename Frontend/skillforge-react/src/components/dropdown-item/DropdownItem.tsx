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

export function DropdownItem({
    href,
    type = 'link',
    onClick,
    active,
    disabled,
    children
}: IDropdownItemProps) {
    return (
        <li>
            {type === 'button' ? (
                <button onClick={onClick} className={`dropdown-item ${active ? 'active' : ''} ${disabled ? 'disabled' : ''}`}>
                    {children}
                </button>
            ) : type === 'link' ? (
                <Link href={href ?? ''} className={`dropdown-item ${active ? 'active' : ''} ${disabled ? 'disabled' : ''}`}>
                    {children}
                </Link>
            ) : type === 'text' ? (
                <span className="dropdown-item-text">
                    {children}
                </span>
            ) : ''}
        </li>
    );
}
