'use client'

import * as React from 'react';
import { Block } from '../block/Block';
import Link from 'next/link';
import { useCurrentUser } from '@/hooks/useCurrentUser';

export interface ILoginCtaProps {
	ctaText: string;
	description: string;
	inline?: boolean;
}

export function LoginCta({
	ctaText,
	description,
	inline,
}: ILoginCtaProps) {
	const currentUser = useCurrentUser();

	if (!!currentUser) return '';

	return (
		<Block>
			<div className="card-text p-4">
				<div className="text-center">
					<Link href="/join" className="btn btn-primary text-decoration-none mb-2">{ctaText}</Link>
					<p className={`mb-0 ${inline ? 'd-inline ms-2' : ''}`}>{description}</p>
				</div>
			</div>
		</Block>
	);
}
