import type { AppEnvironment } from './config';

let env: AppEnvironment;

export function initEnv(config: AppEnvironment) {
    env = config;
}

export function getEnv(): AppEnvironment {
    if (!env) throw new Error("Environment not initialized");
    
    return env;
}
