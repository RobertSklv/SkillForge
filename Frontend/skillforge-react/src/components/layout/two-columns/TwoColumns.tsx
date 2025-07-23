import { type ReactNode } from 'react';
import './_two-columns.scss';

export interface ITwoColumnsProps {
    classes?: string;
    leftColumn?: ReactNode;
    children: ReactNode;
}

export function TwoColumns(props: ITwoColumnsProps) {
    return (
        <div className={`row ${props.classes}`}>
            <div className="d-none d-lg-block left-column">
                {props.leftColumn}
            </div>
            <div className="col-12 main-column">
                {props.children}
            </div>
        </div>
    );
}
