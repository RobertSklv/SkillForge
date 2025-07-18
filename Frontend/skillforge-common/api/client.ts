import type ArticleCardType from "../types/ArticleCardType";
import type ArticleUpsertPageModel from "../types/ArticleUpsertPageModel";
import type ArticlePageModel from "../types/ArticlePageModel";
import type HomePageData from "../types/HomePageData";
import type { ImageUploadType } from "../types/ImageUploadType";
import type SvelteFetch from "../types/SvelteFetch";
import type UserInfo from "../types/UserInfo";
import type TagLinkType from "../types/TagLinkType";
import type QueryParams from "../types/QueryParams";
import type FetchData from "../types/FetchData";
import type TagPageData from "../types/TagPageData";
import type UserListItemType from "../types/UserListItemType";
import type UserPageData from "../types/UserPageData";
import type TagListItemType from "../types/TagListItemType";
import type ArticleSearchItemType from "../types/ArticleSearchItemType";
import type GridState from "../types/GridState";
import type PaginationResponse from "../types/PaginationResponse";
import type ReportFormOptions from "../types/ReportFormOptions";
import { deleteAuthToken, getAuthTokenFromBrowserCookie } from "../auth";
import { getEnv } from "../env";

export async function getCurrentUser(fetch?: SvelteFetch, authToken?: string): Promise<UserInfo | undefined> {
	try {
		return await requestApi('/User/Me', {
			fetchFunction: fetch,
			authToken
		});
	} catch {
		return undefined;
	}
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

export function deleteComment(id: number): Promise<any> {
	return requestApi(`/Comment/Delete/${id}`, {
		init: {
			method: 'DELETE',
		}
	})
}

export function rate(id: number, rate: -1 | 0 | 1, type: 'article' | 'comment') {
	const controllers = {
		article: 'Article',
		comment: 'Comment'
	};

	return requestApi(`/${controllers[type]}/Rate/${id}`, {
		init: {
			method: 'POST',
			body: JSON.stringify({
				rate
			})
		}
	});
}

export function getRating(id: number, rateType: 'positive' | 'negative', type: 'article' | 'comment', batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	const controllers = {
		article: 'Article',
		comment: 'Comment'
	};
	const actions = {
		positive: 'PositiveRates',
		negative: 'NegativeRates'
	};

	return requestApi(`/${controllers[type]}/${actions[rateType]}/${id}`, {
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function getLatestArticles(batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
	return requestApi('/Article/Latest', {
		query: {
			batchIndex,
			batchSize,
		}
	});
}

export function getLatestArticlesByTag(tag: string, batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
	return requestApi('/Article/LatestByTag', {
		query: {
			tag,
			batchIndex,
			batchSize,
		}
	});
}

export function getLatestArticlesByAuthor(authorName: string, batchIndex: number, batchSize: number): Promise<ArticleCardType[]> {
	return requestApi('/Article/LatestByAuthor', {
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
		fetchFunction: fetch
	});
}

export function loadArticleUpsertPage(fetch: SvelteFetch, authToken?: string, id?: number): Promise<ArticleUpsertPageModel> {
	return requestApi('/Article/LoadUpsertPage', {
		fetchFunction: fetch,
		authToken,
		query: {
			id: id?.toString()
		}
	});
}

export function getReportFormOptions(fetch?: SvelteFetch, authToken?: string): Promise<ReportFormOptions> {
	return requestApi('/Report/FormOptions', {
		fetchFunction: fetch,
		authToken
	});
}

export function loadHomePage(fetch: SvelteFetch): Promise<HomePageData> {
	return requestApi('/Home/Load', {
		fetchFunction: fetch
	});
}

export function loadUserPage(name: string, fetch?: SvelteFetch, authToken?: string): Promise<UserPageData> {
	return requestApi(`/User/Load/${name}`, {
		fetchFunction: fetch,
		authToken
	});
}

export function getUserFollowers(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/User/Followers/${name}`, {
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function getUserFollowings(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/User/Followings/${name}`, {
		query: {
			batchIndex,
			batchSize
		}
	});
}

export function getUserTagFollowings(name: string, batchIndex: number, batchSize: number): Promise<TagListItemType[]> {
	return requestApi(`/User/TagFollowings/${name}`, {
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
			body: JSON.stringify({
				user
			})
		}
	});
}

export function loadAccountInfoForm(authToken?: string) {
	return requestApi('/User/AccountInfoForm', {
		authToken
	});
}

export function loadTagPage(name: string, fetch?: SvelteFetch, authToken?: string): Promise<TagPageData> {
	return requestApi(`/Tag/Load/${name}`, {
		fetchFunction: fetch,
		authToken
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

export function followTag(tag: string): Promise<boolean> {
	return requestApi('/Tag/Follow', {
		init: {
			method: 'POST',
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
			body: JSON.stringify({
				tag
			})
		}
	});
}

export function getTagFollowers(name: string, batchIndex: number, batchSize: number): Promise<UserListItemType[]> {
	return requestApi(`/Tag/Followers/${name}`, {
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
	return request(getEnv().backendUrl + '/Api' + url, data);
}

export async function request(url: string, data?: FetchData): Promise<any> {
	data ??= {};
	data.init ??= {};
	data.init.headers ??= {};

	let headersToAdd: HeadersInit = {};

	if (typeof data.contentTypeApplicationJson === 'undefined' || data.contentTypeApplicationJson) {
		if (!Object.keys(data.init.headers).includes('Content-Type')) {
			headersToAdd['Content-Type'] = 'application/json';
		}
	}

	headersToAdd['Accept'] = 'application/json';

	let authToken = data.authToken ?? getAuthTokenFromBrowserCookie();
	if (authToken) {
		headersToAdd['Authorization'] = `Bearer ${authToken}`;
	}

	data.init.headers = {
		...data.init.headers,
		...headersToAdd,
	};

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

	if (response.status === 401) {
		deleteAuthToken();
        getEnv().onAuthError?.();
	}

	return responseData;
}