import axios from 'axios';
import { createAuthHeader, getApiTokenForRequest } from './tokenStorage';

export enum FetchTypes {
    GET = "GET",
    POST = "POST",
    PUT = "PUT",
    DELETE = "DELETE"
}

export class ConnectivityError extends Error {
    constructor() {
        super("Failed to connect");
    }
}

export class UnauthenticatedError extends Error {
    constructor() {
        super("Authentication missing");
    }
}

export class ValidationError extends Error {
    public caughtErrors: { [property: string]: any };

    constructor(errors: any) {
        super("Validation failed")
        this.caughtErrors = errors;
    }
}

export function apiFetch<T>(url: string, method: FetchTypes = FetchTypes.GET, body: any = undefined, extraParams: any = undefined): Promise<T> {
    let headers = { "content-type": "application/json" };

    const auth = (
        extraParams?.noAuth ?
            {} :
            (extraParams?.authToken ?
                { "Authorization": createAuthHeader(extraParams.authToken) } :
                { "Authorization": getApiTokenForRequest() }))
    headers = { ...headers, ...auth };

    let params = {
        ...extraParams,
        url,
        headers
    } as any;

    if (method !== FetchTypes.GET) {
        params = { ...params, method: method as string, data: body };
    }
    return axios.request<T>(params)
        .then(({ data }) => data)
        .catch(error => {
            if (!error.response) {
                throw new ConnectivityError();
            } else if (error.response.status === 401) {
                throw new UnauthenticatedError();
            } else if (error.response?.data?.errors) {
                throw new ValidationError(error.response.data.errors);
            }
            else {
                throw error;
            }
        });
}

export function fetchGet<T>(url: string): Promise<T> {
    return apiFetch<T>(url, FetchTypes.GET);
}

export function fetchPost<T>(url: string, body: any) {
    return apiFetch<T>(url, FetchTypes.POST, body);
}