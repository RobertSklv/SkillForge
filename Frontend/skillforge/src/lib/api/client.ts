import { PUBLIC_BACKEND_DOMAIN } from "$env/static/public";
import type ArticleCardType from "$lib/types/ArticleCardType";
import type ArticleUpsertPageModel from "$lib/types/ArticleUpsertPageModel";
import type ArticlePageModel from "$lib/types/ArticlePageModel";
import type HomePageData from "$lib/types/HomePageData";
import type { ImageUploadType } from "$lib/types/ImageUploadType";
import type SvelteFetch from "$lib/types/SvelteFetch";
import type UserInfo from "$lib/types/UserInfo";
import type TagLinkType from "$lib/types/TagLinkType";
import type QueryParams from "$lib/types/QueryParams";
import type QueryParamsFiltered from "$lib/types/QueryParamsFiltered";
import type FetchData from "$lib/types/FetchData";
import type TagPageData from "$lib/types/TagPageData";

export async function getCurrentUser(): Promise<UserInfo | null> {
  try {
    return await requestApi<UserInfo | null>('/User/Me', {
      init: {
        credentials: 'include'
      }
    });
  } catch {
    return null;
  }
}

export async function logout(): Promise<string> {
  const data = await requestApi<string>('/User/Logout', {
    init: {
    method: 'POST',
    credentials: 'include'
  }
  });

  return data;
}

export async function uploadImage(file: File, type: ImageUploadType): Promise<string> {
  const formData = new FormData();
  formData.append('image', file);

  let url = await requestApi<string>('/Image/Upload', {
    init: {
      method: 'POST',
      body: formData
    },
    query: {
      type
    },
    contentTypeApplicationJson: false,
  });

  if (type === 'article') {
    return '/images/articles/uploads/' + url;
  } else {
    return '/images/comments/uploads/' + url;
  }
}

export function rate(id: number, rate: -1 | 0 | 1, type: 'article' | 'comment') {
  const controllers = {
    article: 'Article',
    comment: 'Comment'
  };

  return requestApi(`/${controllers[type]}/Rate/${id}`, {
    init: {
      method: 'POST',
      credentials: 'include',
      body: JSON.stringify({
        rate
      })
    }
  });
}

export function getLatestArticles(batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
  return requestApi<ArticleCardType[]>('/Article/Latest', {
    init: {
      credentials: 'include'
    },
    query: {
      batchIndex,
      batchSize,
    }
  });
}

export function getLatestArticlesByTag(tag: string, batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
  return requestApi<ArticleCardType[]>('/Article/Latest', {
    init: {
      credentials: 'include'
    },
    query: {
      tag,
      batchIndex,
      batchSize,
    }
  });
}

export function viewArticle(fetch: SvelteFetch, id: number): Promise<ArticlePageModel> {
  return requestApi<ArticlePageModel>(`/Article/View/${id}`, {
    fetchFunction: fetch,
    init: {
      credentials: 'include'
    }
  });
}

export function loadArticleUpsertPage(fetch: SvelteFetch, id?: number): Promise<ArticleUpsertPageModel> {
  return requestApi<ArticleUpsertPageModel>('/Article/LoadUpsertPage', {
    fetchFunction: fetch,
    init: {
      credentials: 'include'
    },
    query: {
      id: id?.toString()
    }
  });
}

export function loadHomePage(fetch: SvelteFetch): Promise<HomePageData> {
  return requestApi('/Home/Load', {
    fetchFunction: fetch
  });
}

export function loadTagPage(fetch: SvelteFetch, name: string): Promise<TagPageData> {
  return requestApi(`/Tag/Load/${name}`, {
    fetchFunction: fetch
  });
}

export function searchTags(phrase?: string, exclude?: string[]): Promise<TagLinkType[]> {
  return requestApi(`/Tag/Search`, {
    query: {
      phrase,
      exclude
    }
  });
}

function isString(val: any): val is string {
  return typeof val === 'string';
}

function toString(val: any): string {
  if (isString(val)) {
    return val;
  }

  return val.toString();
}

function createURLSearchParams(params: QueryParams): URLSearchParams {
  const filtered: {
    [key: string]: string
  } = {};

  let usp = new URLSearchParams(filtered);

  for (const key in params) {
    const value = params[key];
    if (value !== undefined) {
      if (isString(value)) {
        usp.append(key, value);
      } else if (Array.isArray(value)) {
        for (let v of value) {
          usp.append(key, toString(v));
        }
      }
    }
  }

  return usp;
}

export async function requestApi<T>(url: string, data?: FetchData): Promise<T> {
  return request<T>(PUBLIC_BACKEND_DOMAIN + '/Api' + url, data);
}

export async function request<T>(url: string, data?: FetchData): Promise<T> {

  if (data) {
    data.init ??= {};
    data.init.headers ??= {};

    if (typeof data.contentTypeApplicationJson === 'undefined' || data.contentTypeApplicationJson) {
      if (!Object.keys(data.init.headers).includes('Content-Type')) {
        data.init.headers = {
          ...data.init.headers,
          'Content-Type': 'application/json',
        }
      }
    }

    data.init.headers = {
      ...data.init.headers,
      'Accept': 'application/json',
    }
  }

  let _url = new URL(url);
  if (data?.query) {
    _url.search = createURLSearchParams(data?.query).toString();
  }

  const response = await (data?.fetchFunction ?? fetch)(_url, data?.init);
  var text = await response.text();
  var responseData: any;

  try {
    responseData = JSON.parse(text);
  } catch {
    responseData = text;
  }

  if (!response.ok) {
    throw responseData;
  }

  return responseData;
}