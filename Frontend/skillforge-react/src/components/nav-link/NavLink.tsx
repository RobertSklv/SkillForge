import Link from 'next/link';
import { type ReactNode } from 'react';

export interface INavLinkProps {
    href: string;
    active?: boolean;
    children: ReactNode;
}

export function NavLink (props: INavLinkProps) {
  return (
    <li className="nav-item">
        <Link className={`nav-link ${props.active ? 'active' : ''}`} href={props.href}>
            {props.children}
        </Link>
    </li>
  );
}
