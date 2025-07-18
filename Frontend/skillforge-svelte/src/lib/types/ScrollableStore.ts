export default interface ScrollableStore {
    getClientHeight?: () => number,
    onScroll: (callback: () => void) => void,
}