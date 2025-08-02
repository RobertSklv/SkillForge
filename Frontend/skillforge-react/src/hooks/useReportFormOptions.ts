import { create } from 'zustand';
import ReportFormOptions from '@/lib/types/ReportFormOptions';

interface ReportFormOptionsState {
    reportOptions: ReportFormOptions | undefined;
    setReportOptions: (reportOptions: ReportFormOptions | undefined) => void;
}

export const useReportFormOptions = create<ReportFormOptionsState>((set) => ({
    reportOptions: undefined,
    setReportOptions: (reportOptions: ReportFormOptions | undefined) => set({ reportOptions }),
}));