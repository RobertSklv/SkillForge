import { type ReactNode } from 'react';
import { Link as ReactLink } from 'react-router-dom';
import type { BootstrapColor } from 'skillforge-common/types/BootstrapColor';

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

export function Link(props: ILinkProps) {

    const fullClass = `${props.classes} ${props.size === 'normal' ? 'ps-2 py-1' : 'p-0 pe-1'} btn btn${props.background === 'outline' ? '-outline' : ''}-${props.color} rounded-${props.borderRadius} border-1 shadow-none ${props.muted ? 'opacity-50' : ''}`;

    return (
        <ReactLink to={props.href} className={fullClass}>
            {props.children}
        </ReactLink>
    );
}
