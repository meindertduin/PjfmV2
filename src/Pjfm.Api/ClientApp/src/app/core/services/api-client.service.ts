/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.7.0 (NJsonSchema v10.6.7.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming
// @ts-nocheck

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
    let url_ = this.baseUrl + "/authentication/logout";
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
          return this.processLogout(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processLogout(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

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
    return _observableOf<void>(null as any);
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
          return this.processGroups(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<PlaybackGroupDto[]>;
        }
      } else
        return _observableThrow(response_) as any as Observable<PlaybackGroupDto[]>;
    }));
  }

  protected processGroups(response: HttpResponseBase): Observable<PlaybackGroupDto[]> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as PlaybackGroupDto[];
        return _observableOf(result200);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<PlaybackGroupDto[]>(null as any);
  }

  play(deviceId: string | null | undefined, groupId: string): Observable<void> {
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
      })
    };

    return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processPlay(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processPlay(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processPlay(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return _observableOf<void>(null as any);
      }));
    } else if (status === 409) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result409: any = null;
        result409 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
        return throwException("A server side error occurred.", status, _responseText, _headers, result409);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<void>(null as any);
  }

  stop(): Observable<void> {
    let url_ = this.baseUrl + "/api/playback/stop";
    url_ = url_.replace(/[?&]$/, "");

    let options_ : any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
      })
    };

    return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processStop(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processStop(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processStop(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return _observableOf<void>(null as any);
      }));
    } else if (status === 409) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result409: any = null;
        result409 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
        return throwException("A server side error occurred.", status, _responseText, _headers, result409);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<void>(null as any);
  }

  skip(): Observable<void> {
    let url_ = this.baseUrl + "/api/playback/skip";
    url_ = url_.replace(/[?&]$/, "");

    let options_ : any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
      })
    };

    return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processSkip(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processSkip(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processSkip(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return _observableOf<void>(null as any);
      }));
    } else if (status === 409) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result409: any = null;
        result409 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
        return throwException("A server side error occurred.", status, _responseText, _headers, result409);
      }));
    } else if (status === 404) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result404: any = null;
        result404 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
        return throwException("A server side error occurred.", status, _responseText, _headers, result404);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<void>(null as any);
  }

  trackRequest(trackRequest: PlaybackTrackRequest): Observable<void> {
    let url_ = this.baseUrl + "/api/playback/track-request";
    url_ = url_.replace(/[?&]$/, "");

    const content_ = JSON.stringify(trackRequest);

    let options_ : any = {
      body: content_,
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    };

    return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processTrackRequest(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processTrackRequest(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processTrackRequest(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return _observableOf<void>(null as any);
      }));
    } else if (status === 409) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result409: any = null;
        result409 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ProblemDetails;
        return throwException("A server side error occurred.", status, _responseText, _headers, result409);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<void>(null as any);
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
          return this.processAuthenticate(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processAuthenticate(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

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
    return _observableOf<void>(null as any);
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
          return this.processCallback(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<string>;
        }
      } else
        return _observableThrow(response_) as any as Observable<string>;
    }));
  }

  protected processCallback(response: HttpResponseBase): Observable<string> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as string;
        return _observableOf(result200);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<string>(null as any);
  }

  login(): Observable<void> {
    let url_ = this.baseUrl + "/api/spotify/authenticate/login";
    url_ = url_.replace(/[?&]$/, "");

    let options_ : any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
      })
    };

    return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processLogin(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processLogin(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processLogin(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

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
    return _observableOf<void>(null as any);
  }

  logincallback(code: string | null | undefined): Observable<FileResponse | null> {
    let url_ = this.baseUrl + "/api/spotify/authenticate/logincallback?";
    if (code !== undefined && code !== null)
      url_ += "code=" + encodeURIComponent("" + code) + "&";
    url_ = url_.replace(/[?&]$/, "");

    let options_ : any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        "Accept": "application/octet-stream"
      })
    };

    return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processLogincallback(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processLogincallback(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<FileResponse | null>;
        }
      } else
        return _observableThrow(response_) as any as Observable<FileResponse | null>;
    }));
  }

  protected processLogincallback(response: HttpResponseBase): Observable<FileResponse | null> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200 || status === 206) {
      const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
      const fileNameMatch = contentDisposition ? /filename[^;=\n]*=(?:(\\?['"])(.*?)\1|(?:[^\s]+'.*?')?([^;\n]*))/g.exec(contentDisposition) : undefined;
      const fileName = fileNameMatch && fileNameMatch.length > 1 ? decodeURIComponent(fileNameMatch[fileNameMatch.length - 1]) : undefined;
      return _observableOf({ fileName: fileName, data: responseBlob as any, status: status, headers: _headers });
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<FileResponse | null>(null as any);
  }
}

@Injectable()
export class SpotifyPlayerClient {
  private http: HttpClient;
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
    this.http = http;
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
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
          return this.processDevices(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<GetDevicesResponse>;
        }
      } else
        return _observableThrow(response_) as any as Observable<GetDevicesResponse>;
    }));
  }

  protected processDevices(response: HttpResponseBase): Observable<GetDevicesResponse> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetDevicesResponse;
        return _observableOf(result200);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<GetDevicesResponse>(null as any);
  }
}

@Injectable()
export class SpotifyTrackClient {
  private http: HttpClient;
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
    this.http = http;
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "https://localhost:5004";
  }

  search(query: string | null | undefined): Observable<SearchTracksResult> {
    let url_ = this.baseUrl + "/api/spotify/tracks/search?";
    if (query !== undefined && query !== null)
      url_ += "query=" + encodeURIComponent("" + query) + "&";
    url_ = url_.replace(/[?&]$/, "");

    let options_ : any = {
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
        "Accept": "application/json"
      })
    };

    return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
      return this.processSearch(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processSearch(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<SearchTracksResult>;
        }
      } else
        return _observableThrow(response_) as any as Observable<SearchTracksResult>;
    }));
  }

  protected processSearch(response: HttpResponseBase): Observable<SearchTracksResult> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as SearchTracksResult;
        return _observableOf(result200);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<SearchTracksResult>(null as any);
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
          return this.processMe(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<GetCurrentUserResponseModel>;
        }
      } else
        return _observableThrow(response_) as any as Observable<GetCurrentUserResponseModel>;
    }));
  }

  protected processMe(response: HttpResponseBase): Observable<GetCurrentUserResponseModel> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetCurrentUserResponseModel;
        return _observableOf(result200);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf<GetCurrentUserResponseModel>(null as any);
  }
}

export interface PlaybackGroupDto {
  groupId: string;
  groupName: string;
  currentlyPlayingTrack?: SpotifyTrackDto | null;
  queuedTracks: SpotifyTrackDto[];
  listenersCount: number;
}

export interface SpotifyTrackDto {
  title: string;
  spotifyTrackId: string;
  artists: string[];
  trackTerm: TrackTerm;
  trackDurationMs: number;
  trackStartDate: Date;
  spotifyAlbum: SpotifyAlbumDto;
  trackType: TrackType;
  user?: ApplicationUserDto | null;
}

export enum TrackTerm {
  Short = 0,
  Medium = 1,
  Long = 2,
  None = 3,
}

export interface SpotifyAlbumDto {
  albumId: string;
  albumImage: SpotifyAlbumImageDto;
  title: string;
  releaseDate: string;
}

export interface SpotifyAlbumImageDto {
  url: string;
  width: number;
  height: number;
}

export enum TrackType {
  Request = 0,
  Filler = 1,
  ModRequest = 2,
}

export interface ApplicationUserDto {
  userName: string;
  userId: string;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
}

export interface PlaybackTrackRequest {
  trackIds: string[];
}

export interface GetDevicesResponse {
  devices: DeviceModel[];
}

export interface DeviceModel {
  deviceId: string;
  deviceName: string;
  isActive: boolean;
}

export interface SearchTracksResult {
  tracks: SpotifyTrackDto[];
}

export interface GetCurrentUserResponseModel {
  userId: string;
  userName: string;
  roles: UserRole[];
}

export enum UserRole {
  User = 0,
  SpotifyAuth = 1,
  Dj = 2,
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
        observer.next((event.target as any).result);
        observer.complete();
      };
      reader.readAsText(blob);
    }
  });
}
