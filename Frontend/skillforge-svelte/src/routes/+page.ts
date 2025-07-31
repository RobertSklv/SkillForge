import { loadHomePage } from "$lib/api/client";
import type HomePageData from "$lib/types/HomePageData";

export async function load({ fetch }): Promise<HomePageData> {
    let pageData: HomePageData = await loadHomePage(fetch);

    return pageData;
}