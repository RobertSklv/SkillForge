import { create } from 'zustand';
import type UserInfo from '@/lib/types/UserInfo';

interface CurrentUserState {
    currentUser: UserInfo | undefined;
    setCurrentUser: (user: UserInfo | undefined) => void;
    logoutUser: () => void;
}

export const useCurrentUser = create<CurrentUserState>((set) => ({
    currentUser: undefined,
    setCurrentUser: (user) => set({ currentUser: user }),
    logoutUser: () => set({ currentUser: undefined })
}));