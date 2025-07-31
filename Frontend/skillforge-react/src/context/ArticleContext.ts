import type ArticleContext from "@/lib/types/ArticleContext";
import { createContext, useContext } from "react";

export const ArticleContext = createContext<ArticleContext | undefined>(undefined);

export function useArticleContext() {
    const context = useContext(ArticleContext);

    if (!context) throw new Error('useArticleContext must be used inside ArticleContextProvider');

    return context;
}