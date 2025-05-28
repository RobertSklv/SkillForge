import { PUBLIC_BACKEND_DOMAIN } from "$env/static/public";
import type UserInfo from "$lib/types/UserInfo";

export async function getCurrentUser(): Promise<UserInfo | null> {
  try {
    return await requestApi<UserInfo | null>('/User/Me', {
      credentials: 'include',
    });
  } catch {
    return null;
  }
}

export async function logout(): Promise<string> {
  const data = await requestApi<string>('/User/Logout', {
    method: 'POST',
    credentials: 'include'
  });

  return data;
}

export async function requestApi<T>(url: string, init?: RequestInit): Promise<T> {
  return request<T>(PUBLIC_BACKEND_DOMAIN + '/Api' + url, init);
}

export async function request<T>(url: string, init?: RequestInit): Promise<T> {
  const response = await fetch(url, init);
  var text = await response.text();
  var data: any;

  try {
    data = JSON.parse(text);
  } catch {
    data = text;
  }

  if (!response.ok) {
    throw data;
  }

  return data;
}