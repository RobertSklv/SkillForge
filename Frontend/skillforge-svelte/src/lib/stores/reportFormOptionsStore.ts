import type ReportFormOptions from "$lib/types/ReportFormOptions";
import { writable } from "svelte/store";


export let reportFormOptionsStore = writable<ReportFormOptions>();