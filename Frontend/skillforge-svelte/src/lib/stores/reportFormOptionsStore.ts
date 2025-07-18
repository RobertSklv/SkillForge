import type ReportFormOptions from "skillforge-common/types/ReportFormOptions";
import { writable } from "svelte/store";


export let reportFormOptionsStore = writable<ReportFormOptions>();