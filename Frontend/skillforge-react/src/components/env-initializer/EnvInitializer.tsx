'use client'

import { useRouter } from "next/navigation";
import { ReactNode, useEffect, useState } from "react";
import { getEnv, initEnv, isEnvInitialized } from "skillforge-common/env";

export interface IEnvInitializerProps {
    children: ReactNode;
}

export function EnvInitializer({ children }: IEnvInitializerProps) {
    const router = useRouter();
    const [ready, setReady] = useState(false);

    useEffect(() => {
        initEnv({
            baseUrl: process.env.NEXT_PUBLIC_API_BASE_URL as string,
            backendUrl: process.env.NEXT_PUBLIC_BACKEND_DOMAIN as string,
            onAuthError: () => {
                router.push('/join');
            },
            assetsRelativePath: ''
        });

        setReady(true);
    }, []);

    if (!ready) return null;

    return <>{children}</>;
}
