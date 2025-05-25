

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