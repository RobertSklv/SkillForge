import { BootstrapColor } from '@/lib/types/BootstrapColor';
import * as React from 'react';
import { LinkPlaceholder } from '../link-placeholder/LinkPlaceholder';

export interface ITagLinkPlaceholderProps {
	size?: 'normal' | 'small';
	background?: 'outline' | 'fill';
	color?: BootstrapColor;
}

export function TagLinkPlaceholder({
	size,
	background,
	color,
}: ITagLinkPlaceholderProps) {
	return (
		<LinkPlaceholder size={size} background={background} color={color} borderRadius={3} />
	);
}
