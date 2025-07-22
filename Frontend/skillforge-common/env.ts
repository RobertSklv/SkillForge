import type { AppEnvironment } from './config';

let env: AppEnvironment;

export function initEnv(config: AppEnvironment) {
    env = config;
}

export function getEnv(): AppEnvironment {
    if (!isEnvInitialized()) throw new Error("Environment not initialized");
    
    return env;
}

export function isEnvInitialized(): boolean {
    return !!env;
}