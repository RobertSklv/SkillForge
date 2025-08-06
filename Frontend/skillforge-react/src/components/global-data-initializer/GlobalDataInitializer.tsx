'use client';

import { useCurrentUser } from "@/hooks/useCurrentUser";
import { useEffect } from "react";
import UserInfo from "@/lib/types/UserInfo";
import { useReportFormOptions } from "@/hooks/useReportFormOptions";
import ReportFormOptions from "@/lib/types/ReportFormOptions";

export interface IGlobalDataInitializerProps {
    currentUserInfo?: UserInfo;
    reportFromOptions: ReportFormOptions;
}

export function GlobalDataInitializer({ currentUserInfo, reportFromOptions }: IGlobalDataInitializerProps) {
    const { setCurrentUser } = useCurrentUser();
    const { setReportOptions } = useReportFormOptions();

    useEffect(() => {
        setCurrentUser(currentUserInfo);
        setReportOptions(reportFromOptions);
    }, []);

    return null;
}
