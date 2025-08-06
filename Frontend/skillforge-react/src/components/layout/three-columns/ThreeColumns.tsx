import { type ReactNode } from 'react';
import './_three-columns.scss';

export interface IThreeColumnsProps {
    classes?: string;
    leftColumn?: ReactNode;
    children: ReactNode;
    rightColumn?: ReactNode;
    hideLeftColumnOnMobile?: boolean;
    hideRightColumnOnMobile?: boolean;
}

export function ThreeColumns (props: IThreeColumnsProps) {
  return (
    <div className={`row flex-column flex-xl-row ${props.classes}`}>
        <div className={`left-column ${props.hideLeftColumnOnMobile ? 'd-none d-xl-block' : ''}`}>
            {props.leftColumn}
        </div>
        <div className="col-12 middle-column">
            {props.children}
        </div>
        <div className={`right-column ${props.hideRightColumnOnMobile ? 'd-none d-xl-block' : ''}`}>
            {props.rightColumn}
        </div>
    </div>
  );
}
