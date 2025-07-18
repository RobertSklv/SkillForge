import { loadHomePage } from "skillforge-common/api/client";
import type HomePageData from "skillforge-common/types/HomePageData";

export async function load({ fetch }): Promise<HomePageData> {
    let pageData: HomePageData = await loadHomePage(fetch);

    return pageData;
}