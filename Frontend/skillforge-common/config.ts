export interface AppEnvironment {
    backendUrl: string;
    onAuthError?: () => void;
    isBrowser?: boolean,
}