import { UnauthenticatedError } from "./apiFetch";
import jwt_decode from 'jwt-decode';
import { DecodedToken } from "../typings/decodedToken";

const apiTokenStorageKey = 'apitoken';

export const getApiToken = () => localStorage.getItem(apiTokenStorageKey);

export const getApiTokenThrowing = () => {
    const token = getApiToken();
    if (!token) {
        throw new UnauthenticatedError();
    }
    return token;
}

export const createAuthHeader = (token: string) => `Bearer ${token}`;

export const getApiTokenForRequest = () => createAuthHeader(getApiTokenThrowing());

export const storeApiToken = (token: string) => localStorage.setItem(apiTokenStorageKey, token);
export const clearApiToken = () => localStorage.removeItem(apiTokenStorageKey);
export const isAuthenticated = () => getApiToken() ? true : false;
export const decodeToken = (token:string) => jwt_decode(token) as DecodedToken;