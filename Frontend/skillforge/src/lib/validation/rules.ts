import { requestApi } from "$lib/api/client";


export function required(value: any): boolean {
    return !!value;
}

export function maxLength(value: any, max: number): boolean {
    if (typeof value === 'string') {
        return value.length <= max;
    }

    return true;
}

export function compare(value1: any, value2: any): boolean {
    return value1 == value2;
}

export function email(value: any): boolean {
    return regex(value, /^[\w\.-]+@[\w\.-]+\.\w+$/);
}

export function password(value: any): boolean {
    return regex(value, /^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$/);
}

export function regex(value: any, pattern: RegExp): boolean {
    return !!value.match(pattern);
}

export async function remote(url: string, paramName: any, value: any): Promise<boolean> {
    if (!value) {
        return true;
    }

    let res = await requestApi(url, {
        query: {
            [paramName]: value
        }
    });

    return res;
}