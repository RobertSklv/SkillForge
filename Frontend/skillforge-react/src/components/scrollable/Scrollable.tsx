import { ScrollableContext } from "@/context/ScrollableContext";
import ScrollableStore from "@/lib/types/ScrollableStore";
import { useRef } from "react";

export function Scrollable(props: any) {
    const scrollableElement = useRef<HTMLDivElement>(null);
    const onScrollCallbacks = useRef<(() => void)[]>(null);
    const scrollableContext = useRef<ScrollableStore>({
        getClientHeight: () => {
            return scrollableElement.current
                ? scrollableElement.current.getBoundingClientRect().top + scrollableElement.current.clientHeight
                : 0;
        },
        onScroll: (callback: () => void) => {
            onScrollCallbacks.current?.push(callback);
        }
    });

    function onScroll() {
        onScrollCallbacks.current?.forEach((cb) => cb());
    }

    return (
        <ScrollableContext.Provider value={scrollableContext.current}>
            <div {...props} ref={scrollableElement} onScroll={onScroll}>
                {props.children}
            </div>
        </ScrollableContext.Provider>
    );
}
