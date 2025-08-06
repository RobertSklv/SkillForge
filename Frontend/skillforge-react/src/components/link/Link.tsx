import { type ReactNode } from 'react';
import { default as ReactLink } from 'next/link';
import type { BootstrapColor } from '@/lib/types/BootstrapColor';

export interface ILinkProps {
    href: string;
    size?: 'normal' | 'small';
    background?: 'outline' | 'fill';
    classes?: string;
    color?: BootstrapColor;
    borderRadius?: 0 | 1 | 2 | 3 | 4 | 5 | 'pill' | 'circle';
    muted?: boolean;
    children: ReactNode;
}

export function Link({
    href,
    size = 'normal',
    background = 'outline',
    classes,
    color = 'primary',
    borderRadius = 3,
    muted,
    children
}: ILinkProps) {

    const fullClass = `${classes} ${size === 'normal' ? 'ps-2 py-1' : 'p-0 pe-1'} btn btn${background === 'outline' ? '-outline' : ''}-${color} rounded-${borderRadius} border-1 shadow-none ${muted ? 'opacity-50' : ''}`;

    return (
        <ReactLink href={href} className={fullClass}>
            {children}
        </ReactLink>
    );
}
