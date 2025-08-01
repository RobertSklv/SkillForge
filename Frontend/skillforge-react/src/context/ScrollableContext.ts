import type ScrollableStore from "../lib/types/ScrollableStore";
import { createContext, useContext } from "react";

export const ScrollableContext = createContext<ScrollableStore | undefined>(undefined);

export function useScrollableContext() {
    return useContext(ScrollableContext);
}