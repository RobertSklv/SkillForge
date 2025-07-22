export interface AppEnvironment {
    baseUrl: string;
    backendUrl: string;
    onAuthError?: () => void;
    isBrowser?: boolean;
    assetsRelativePath: string;
}