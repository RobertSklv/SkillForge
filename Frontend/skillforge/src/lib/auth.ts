import { browser } from "$app/environment";

export const JWT_TOKEN_COOKIE_NAME = 'auth_jwt';

export function storeAuthToken(token: string) {
    document.cookie = `${JWT_TOKEN_COOKIE_NAME}=${token}; Path=/; SameSite=None; Secure`;
}

export function deleteAuthToken() {
    document.cookie = `${JWT_TOKEN_COOKIE_NAME}=; Path=/; Max-Age=0; SameSite=None; Secure`;
}

export function getAuthTokenFromBrowserCookie(): string | undefined {
	if (!browser) {
		return undefined;
	}

	const cookies = document.cookie;
    const parsedCookies = Object.fromEntries(
        cookies.split('; ').map(c => c.split('='))
    );
    const token = parsedCookies[JWT_TOKEN_COOKIE_NAME];

	return token;
}