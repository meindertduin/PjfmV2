/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class AuthenticationClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
    }

    logout(): Observable<void> {
        let url_ = this.baseUrl + "/api/authentication/logout";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processLogout(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processLogout(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processLogout(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 302) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }
}

@Injectable()
export class PlaybackClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
    }

    groups(): Observable<PlaybackGroupDto[]> {
        let url_ = this.baseUrl + "/api/playback/groups";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGroups(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGroups(<any>response_);
                } catch (e) {
                    return <Observable<PlaybackGroupDto[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<PlaybackGroupDto[]>><any>_observableThrow(response_);
        }));
    }

    protected processGroups(response: HttpResponseBase): Observable<PlaybackGroupDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(PlaybackGroupDto.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PlaybackGroupDto[]>(<any>null);
    }

    play(deviceId: string | null | undefined, groupId: string): Observable<FileResponse | null> {
        let url_ = this.baseUrl + "/api/playback/{groupId}/play?";
        if (groupId === undefined || groupId === null)
            throw new Error("The parameter 'groupId' must be defined.");
        url_ = url_.replace("{groupId}", encodeURIComponent("" + groupId));
        if (deviceId !== undefined && deviceId !== null)
            url_ += "deviceId=" + encodeURIComponent("" + deviceId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/octet-stream"
            })
        };

        return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processPlay(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processPlay(<any>response_);
                } catch (e) {
                    return <Observable<FileResponse | null>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileResponse | null>><any>_observableThrow(response_);
        }));
    }

    protected processPlay(response: HttpResponseBase): Observable<FileResponse | null> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200 || status === 206) {
            const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
            const fileNameMatch = contentDisposition ? /filename="?([^"]*?)"?(;|$)/g.exec(contentDisposition) : undefined;
            const fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[1] : undefined;
            return _observableOf({ fileName: fileName, data: <any>responseBlob, status: status, headers: _headers });
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileResponse | null>(<any>null);
    }

    stop(): Observable<FileResponse | null> {
        let url_ = this.baseUrl + "/api/playback/stop";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/octet-stream"
            })
        };

        return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processStop(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processStop(<any>response_);
                } catch (e) {
                    return <Observable<FileResponse | null>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileResponse | null>><any>_observableThrow(response_);
        }));
    }

    protected processStop(response: HttpResponseBase): Observable<FileResponse | null> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200 || status === 206) {
            const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
            const fileNameMatch = contentDisposition ? /filename="?([^"]*?)"?(;|$)/g.exec(contentDisposition) : undefined;
            const fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[1] : undefined;
            return _observableOf({ fileName: fileName, data: <any>responseBlob, status: status, headers: _headers });
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileResponse | null>(<any>null);
    }
}

@Injectable()
export class SpotifyAuthenticationClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
    }

    authenticate(): Observable<void> {
        let url_ = this.baseUrl + "/api/spotify/authenticate";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processAuthenticate(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAuthenticate(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processAuthenticate(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 302) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    callback(state: string | null | undefined, code: string | null | undefined): Observable<string> {
        let url_ = this.baseUrl + "/api/spotify/authenticate/callback?";
        if (state !== undefined && state !== null)
            url_ += "state=" + encodeURIComponent("" + state) + "&";
        if (code !== undefined && code !== null)
            url_ += "code=" + encodeURIComponent("" + code) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCallback(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCallback(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processCallback(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(<any>null);
    }
}

@Injectable()
export class SpotifyClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
    }

    update(): Observable<FileResponse | null> {
        let url_ = this.baseUrl + "/api/spotify/tracks/update";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/octet-stream"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processUpdate(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processUpdate(<any>response_);
                } catch (e) {
                    return <Observable<FileResponse | null>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileResponse | null>><any>_observableThrow(response_);
        }));
    }

    protected processUpdate(response: HttpResponseBase): Observable<FileResponse | null> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200 || status === 206) {
            const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
            const fileNameMatch = contentDisposition ? /filename="?([^"]*?)"?(;|$)/g.exec(contentDisposition) : undefined;
            const fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[1] : undefined;
            return _observableOf({ fileName: fileName, data: <any>responseBlob, status: status, headers: _headers });
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileResponse | null>(<any>null);
    }

    devices(): Observable<GetDevicesResponse> {
        let url_ = this.baseUrl + "/api/spotify/devices";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processDevices(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processDevices(<any>response_);
                } catch (e) {
                    return <Observable<GetDevicesResponse>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetDevicesResponse>><any>_observableThrow(response_);
        }));
    }

    protected processDevices(response: HttpResponseBase): Observable<GetDevicesResponse> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = GetDevicesResponse.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetDevicesResponse>(<any>null);
    }
}

@Injectable()
export class UserClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
    }

    me(): Observable<GetCurrentUserResponseModel> {
        let url_ = this.baseUrl + "/api/users/me";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processMe(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processMe(<any>response_);
                } catch (e) {
                    return <Observable<GetCurrentUserResponseModel>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetCurrentUserResponseModel>><any>_observableThrow(response_);
        }));
    }

    protected processMe(response: HttpResponseBase): Observable<GetCurrentUserResponseModel> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = GetCurrentUserResponseModel.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetCurrentUserResponseModel>(<any>null);
    }
}

export class PlaybackGroupDto implements IPlaybackGroupDto {
    groupId!: string;
    groupName!: string;
    currentlyPlayingTrack?: SpotifyTrackDto | undefined;
    listenersCount!: number;

    constructor(data?: IPlaybackGroupDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.groupId = _data["groupId"];
            this.groupName = _data["groupName"];
            this.currentlyPlayingTrack = _data["currentlyPlayingTrack"] ? SpotifyTrackDto.fromJS(_data["currentlyPlayingTrack"]) : <any>undefined;
            this.listenersCount = _data["listenersCount"];
        }
    }

    static fromJS(data: any): PlaybackGroupDto {
        data = typeof data === 'object' ? data : {};
        let result = new PlaybackGroupDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["groupId"] = this.groupId;
        data["groupName"] = this.groupName;
        data["currentlyPlayingTrack"] = this.currentlyPlayingTrack ? this.currentlyPlayingTrack.toJSON() : <any>undefined;
        data["listenersCount"] = this.listenersCount;
        return data; 
    }
}

export interface IPlaybackGroupDto {
    groupId: string;
    groupName: string;
    currentlyPlayingTrack?: SpotifyTrackDto | undefined;
    listenersCount: number;
}

export class SpotifyTrackDto implements ISpotifyTrackDto {
    title!: string;
    spotifyTrackId!: string;
    artists!: string[];
    trackTerm!: TrackTerm;
    trackDurationMs!: number;
    trackStartDate!: Date;

    constructor(data?: ISpotifyTrackDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.artists = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.title = _data["title"];
            this.spotifyTrackId = _data["spotifyTrackId"];
            if (Array.isArray(_data["artists"])) {
                this.artists = [] as any;
                for (let item of _data["artists"])
                    this.artists!.push(item);
            }
            this.trackTerm = _data["trackTerm"];
            this.trackDurationMs = _data["trackDurationMs"];
            this.trackStartDate = _data["trackStartDate"] ? new Date(_data["trackStartDate"].toString()) : <any>undefined;
        }
    }

    static fromJS(data: any): SpotifyTrackDto {
        data = typeof data === 'object' ? data : {};
        let result = new SpotifyTrackDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["spotifyTrackId"] = this.spotifyTrackId;
        if (Array.isArray(this.artists)) {
            data["artists"] = [];
            for (let item of this.artists)
                data["artists"].push(item);
        }
        data["trackTerm"] = this.trackTerm;
        data["trackDurationMs"] = this.trackDurationMs;
        data["trackStartDate"] = this.trackStartDate ? this.trackStartDate.toISOString() : <any>undefined;
        return data; 
    }
}

export interface ISpotifyTrackDto {
    title: string;
    spotifyTrackId: string;
    artists: string[];
    trackTerm: TrackTerm;
    trackDurationMs: number;
    trackStartDate: Date;
}

export enum TrackTerm {
    Short = 0,
    Medium = 1,
    Long = 2,
}

export class GetDevicesResponse implements IGetDevicesResponse {
    devices!: DeviceModel[];

    constructor(data?: IGetDevicesResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.devices = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            if (Array.isArray(_data["devices"])) {
                this.devices = [] as any;
                for (let item of _data["devices"])
                    this.devices!.push(DeviceModel.fromJS(item));
            }
        }
    }

    static fromJS(data: any): GetDevicesResponse {
        data = typeof data === 'object' ? data : {};
        let result = new GetDevicesResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.devices)) {
            data["devices"] = [];
            for (let item of this.devices)
                data["devices"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IGetDevicesResponse {
    devices: DeviceModel[];
}

export class DeviceModel implements IDeviceModel {
    deviceId!: string;
    deviceName!: string;
    isActive!: boolean;

    constructor(data?: IDeviceModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.deviceId = _data["deviceId"];
            this.deviceName = _data["deviceName"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): DeviceModel {
        data = typeof data === 'object' ? data : {};
        let result = new DeviceModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["deviceId"] = this.deviceId;
        data["deviceName"] = this.deviceName;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IDeviceModel {
    deviceId: string;
    deviceName: string;
    isActive: boolean;
}

export class GetCurrentUserResponseModel implements IGetCurrentUserResponseModel {
    userId?: string | undefined;
    userName?: string | undefined;
    roles?: UserRole[] | undefined;

    constructor(data?: IGetCurrentUserResponseModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.userId = _data["userId"];
            this.userName = _data["userName"];
            if (Array.isArray(_data["roles"])) {
                this.roles = [] as any;
                for (let item of _data["roles"])
                    this.roles!.push(item);
            }
        }
    }

    static fromJS(data: any): GetCurrentUserResponseModel {
        data = typeof data === 'object' ? data : {};
        let result = new GetCurrentUserResponseModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userId"] = this.userId;
        data["userName"] = this.userName;
        if (Array.isArray(this.roles)) {
            data["roles"] = [];
            for (let item of this.roles)
                data["roles"].push(item);
        }
        return data; 
    }
}

export interface IGetCurrentUserResponseModel {
    userId?: string | undefined;
    userName?: string | undefined;
    roles?: UserRole[] | undefined;
}

export enum UserRole {
    User = 0,
    Dj = 1,
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}