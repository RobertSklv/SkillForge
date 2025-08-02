'use client';

import { useCurrentUser } from "@/hooks/useCurrentUser";
import { ReactNode, useEffect, useState } from "react";
import UserInfo from "@/lib/types/UserInfo";
import { useReportFormOptions } from "@/hooks/useReportFormOptions";
import ReportFormOptions from "@/lib/types/ReportFormOptions";

export interface IGlobalDataInitializerProps {
    children: ReactNode;
    currentUserInfo?: UserInfo;
    reportFromOptions: ReportFormOptions;
}

export function GlobalDataInitializer({ children, currentUserInfo, reportFromOptions }: IGlobalDataInitializerProps) {
    const [ready, setReady] = useState(false);
    const { setCurrentUser } = useCurrentUser();
    const { setReportOptions } = useReportFormOptions();

    useEffect(() => {
        setCurrentUser(currentUserInfo);
        setReportOptions(reportFromOptions);

        setReady(true);
    }, []);

    if (!ready) return null;

    return <>{children}</>;
}
