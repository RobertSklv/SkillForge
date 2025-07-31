'use client';

import { useCurrentUser } from "@/hooks/useCurrentUser";
import { ReactNode, useEffect, useState } from "react";
import UserInfo from "@/lib/types/UserInfo";

export interface IGlobalDataInitializerProps {
    children: ReactNode;
    currentUserInfo?: UserInfo;
}

export function GlobalDataInitializer({ children, currentUserInfo }: IGlobalDataInitializerProps) {
    const [ready, setReady] = useState(false);
    const { setCurrentUser } = useCurrentUser();

    useEffect(() => {
        setCurrentUser(currentUserInfo);

        setReady(true);
    }, []);

    if (!ready) return null;

    return <>{children}</>;
}
