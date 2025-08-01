import { BootstrapColor } from '@/lib/types/BootstrapColor';
import * as React from 'react';

export interface ILinkPlaceholderProps {
	size?: 'normal' | 'small';
	background?: 'outline' | 'fill';
	color?: BootstrapColor;
	borderRadius?: 0 | 1 | 2 | 3 | 4 | 5 | 'pill' | 'circle';
}

export function LinkPlaceholder({
	size = 'normal',
	background = 'outline',
	color = 'primary',
	borderRadius = 3,
}: ILinkPlaceholderProps) {

	const _class = `placeholder col-5 ${size === 'normal' ? 'ps-2 py-1' : 'p-0 pe-1'} btn btn${background === 'outline' ? '-outline' : ''}-${color} rounded-${borderRadius} border-1 shadow-none`;

	return (
		<span className={_class}></span>
	);
}
