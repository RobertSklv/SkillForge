export interface AppEnvironment {
    baseUrl: string;
    backendUrl: string;
    onAuthError?: () => void;
    isBrowser?: boolean,
}