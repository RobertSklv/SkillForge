import { PUBLIC_BACKEND_DOMAIN, PUBLIC_BASE_URL } from "$env/static/public";


export function clickOutside(
	node: HTMLElement,
	callback: () => void
): { destroy(): void } {
	const handleClick = (event: MouseEvent) => {
		if (!node.contains(event.target as Node)) {
			callback();
		}
	};

	document.addEventListener("click", handleClick, true);

	return {
		destroy() {
			document.removeEventListener("click", handleClick, true);
		},
	};
}

export function formatRelativeTime(date: string): string {
	const now = new Date();
	const dateObj = new Date(date);
	const diff = dateObj.getTime() - now.getTime();
	const seconds = Math.round(diff / 1000);

	const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" });

	const divisions = [
		{ amount: 60, name: "minute" },
		{ amount: 60, name: "hour" },
		{ amount: 24, name: "day" },
		{ amount: 7, name: "week" },
		{ amount: 4.34524, name: "month" },
		{ amount: 12, name: "year" },
		{ amount: Number.POSITIVE_INFINITY, name: "year" },
	];

	let unit = "second";
	let value = seconds;

	for (const division of divisions) {
		if (Math.abs(value) < division.amount) {
			break;
		}
		value /= division.amount;
		unit = division.name;
	}

	return rtf.format(Math.round(value), unit as Intl.RelativeTimeFormatUnit);
}

export function getFrontendUrl(relativePath: string | null = null): string {
	if (!relativePath) {
		return PUBLIC_BASE_URL;
	}

	return PUBLIC_BASE_URL + relativePath;
}

export function getBackendUrl(relativePath: string | null = null): string {
	if (!relativePath) {
		return PUBLIC_BACKEND_DOMAIN;
	}

	return PUBLIC_BACKEND_DOMAIN + relativePath;
}

export function getImagePath(relativePath?: string | null) {
	if (!relativePath) {
		return '/user.png';
	}

	return PUBLIC_BACKEND_DOMAIN + relativePath;
}

export function clamp(value: number, lowerBound: number, upperBound: number) {
	if (value < lowerBound) {
		return lowerBound;
	} else if (value > upperBound) {
		return upperBound;
	}

	return value;
}

export function htmlToText(html: string) {
	return html
		.replace(/<\/?[^>]+(>|$)/g, '')
		.replace('&nbsp;', ' ')
		.replace('<br>', "\n");
}

export function truncateText(text: string, limit: number) {
	if (text.length > limit) {
		return text.substring(0, limit) + '...';
	}

	return text;
}