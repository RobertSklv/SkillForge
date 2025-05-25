import { PUBLIC_BACKEND_DOMAIN } from "$env/static/public";
import { currentUserStore } from "$lib/stores/currentUserStore";
import type SvelteFetch from "$lib/types/SvelteFetch";
import type UserInfo from "$lib/types/UserInfo";

export async function getCurrentUser(fetch: SvelteFetch): Promise<UserInfo | null> {
  const res = await fetch(`${PUBLIC_BACKEND_DOMAIN}/User/Me`, {
    credentials: 'include',
  });

  if (res.ok) {
    let parsed = await res.json();
    currentUserStore.set(parsed);

    return parsed;
  }

  return null;
}

export async function fetchApi(url: string, init?: RequestInit): Promise<Response> {
  const response = await fetch(PUBLIC_BACKEND_DOMAIN + url, init);
  const data = await response.json();

  if (!response.ok) {
    throw data;
  }

  return data;
}