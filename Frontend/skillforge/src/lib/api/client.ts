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
import type UserListItemType from "$lib/types/UserListItemType";
import type UserPageData from "$lib/types/UserPageData";
import type TagListItemType from "$lib/types/TagListItemType";
import type ArticleSearchItemType from "$lib/types/ArticleSearchItemType";
import type GridState from "$lib/types/GridState";
import type PaginationResponse from "$lib/types/PaginationResponse";

export async function getCurrentUser(): Promise<UserInfo | null> {
	try {
		return await requestApi('/User/Me', {
			init: {
				credentials: 'include'
			}
		});
	} catch {
		return null;
	}
}

export async function logout(): Promise<string> {
	const data = await requestApi('/User/Logout', {
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

	let url = await requestApi('/Image/Upload', {
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
	} else if (type === 'comment') {
		return '/images/comments/uploads/' + url;
	} else if (type === 'avatar') {
		return '/images/avatars/uploads/' + url;
	} else throw new Error('Unsupported image type: ' + type);
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
	return requestApi('/Article/Latest', {
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
	return requestApi('/Article/LatestByTag', {
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

export function getLatestArticlesByAuthor(authorName: string, batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
	return requestApi('/Article/LatestByAuthor', {
		init: {
			credentials: 'include'
		},
		query: {
			authorName,
			batchIndex,
			batchSize,
		}
	});
}

export function searchArticles(p: string): Promise<ArticleSearchItemType[]> {
	return requestApi('/Article/Search', {
		query: {
			p
		}
	});
}

export function searchArticlesAdvanced(gridState: GridState, fetch?: SvelteFetch): Promise<PaginationResponse<ArticleCardType>> {
	return requestApi('/Article/SearchAdvanced', {
		fetchFunction: fetch,
		query: gridState
	});
}

export function viewArticle(fetch: SvelteFetch, id: number): Promise<ArticlePageModel> {
	return requestApi(`/Article/View/${id}`, {
		fetchFunction: fetch,
		init: {
			credentials: 'include'
		}
	});
}

export function loadArticleUpsertPage(fetch: SvelteFetch, id?: number): Promise<ArticleUpsertPageModel> {
	return requestApi('/Article/LoadUpsertPage', {
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

export function loadUserPage(name: string): Promise<UserPageData> {
	return requestApi(`/User/Load/${name}`, {
		init: {
			credentials: 'include'
		}
	});
}

export function getUserFollowers(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/User/Followers/${name}`, {
		init: {
			credentials: 'include'
		},
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function getUserFollowings(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/User/Followings/${name}`, {
		init: {
			credentials: 'include'
		},
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function getUserTagFollowings(name: string, batchIndex: number, batchSize: number): Promise<TagListItemType[]> {
	return requestApi(`/User/TagFollowings/${name}`, {
		init: {
			credentials: 'include'
		},
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function followUser(user: string): Promise<boolean> {
	return requestApi('/User/Follow', {
		init: {
			method: 'POST',
			credentials: 'include',
			body: JSON.stringify({
				user
			})
		}
	});
}

export function unfollowUser(user: string): Promise<boolean> {
	return requestApi('/User/Unfollow', {
		init: {
			method: 'DELETE',
			credentials: 'include',
			body: JSON.stringify({
				user
			})
		}
	});
}

export function loadAccountInfoForm() {
	return requestApi('/User/AccountInfoForm', {
		init: {
			credentials: 'include'
		}
	});
}

export function loadTagPage(name: string): Promise<TagPageData> {
	return requestApi(`/Tag/Load/${name}`, {
		init: {
			credentials: 'include'
		}
	});
}

export function searchTags(phrase?: string, exclude?: string[]): Promise<TagLinkType[]> {
	return requestApi(`/Tag/Search`, {
		init: {
			credentials: 'include'
		},
		query: {
			phrase,
			exclude
		}
	});
}

export function followTag(tag: string): Promise<boolean> {
	return requestApi('/Tag/Follow', {
		init: {
			method: 'POST',
			credentials: 'include',
			body: JSON.stringify({
				tag
			})
		}
	});
}

export function unfollowTag(tag: string): Promise<boolean> {
	return requestApi('/Tag/Unfollow', {
		init: {
			method: 'DELETE',
			credentials: 'include',
			body: JSON.stringify({
				tag
			})
		}
	});
}

export function getTagFollowers(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/Tag/Followers/${name}`, {
		init: {
			credentials: 'include'
		},
		query: {
			batchIndex,
			batchSize
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

export function createURLSearchParams(params: QueryParams): URLSearchParams {
	const filtered: {
		[key: string]: string;
	} = {};

	let usp = new URLSearchParams(filtered);

	for (const key in params) {
		const value = params[key];
		if (value) {
			if (isString(value)) {
				usp.append(key, value);
			} else if (Array.isArray(value)) {
				for (let v of value) {
					if (v) {
						usp.append(key, toString(v));
					}
				}
			} else {
				usp.append(key, toString(value));
			}
		}
	}

	return usp;
}

export async function requestApi(url: string, data?: FetchData): Promise<any> {
	return request(PUBLIC_BACKEND_DOMAIN + '/Api' + url, data);
}

export async function request(url: string, data?: FetchData): Promise<any> {

	if (data) {
		data.init ??= {};
		data.init.headers ??= {};

		if (typeof data.contentTypeApplicationJson === 'undefined' || data.contentTypeApplicationJson) {
			if (!Object.keys(data.init.headers).includes('Content-Type')) {
				data.init.headers = {
					...data.init.headers,
					'Content-Type': 'application/json',
				};
			}
		}

		data.init.headers = {
			...data.init.headers,
			'Accept': 'application/json',
		};
	}

	let _url = new URL(url);
	if (data?.query) {
		_url.search = createURLSearchParams(data.query).toString();
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