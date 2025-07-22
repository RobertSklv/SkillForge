import { type ReactNode } from 'react';
import { Link } from 'react-router-dom';

export interface INavLinkProps {
    href: string;
    active?: boolean;
    children: ReactNode;
}

export function NavLink (props: INavLinkProps) {
  return (
    <li className="nav-item">
        <Link className={`nav-link ${props.active ? 'active' : ''}`} to={props.href}>
            {props.children}
        </Link>
    </li>
  );
}
