import { Injectable } from '@angular/core';
import { GetCurrentUserResponseModel, UserClient } from '../../core/services/api-client.service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _user?: GetCurrentUserResponseModel;

  constructor(private readonly _userClient: UserClient) {}

  initialize(): Promise<GetCurrentUserResponseModel | undefined> {
    return this._userClient
      .me()
      .pipe(
        tap((user) => {
          this._user = user;
        }),
      )
      .toPromise()
      .catch(() => {
        return undefined;
      });
  }

  getUser(): GetCurrentUserResponseModel | undefined {
    return this._user;
  }

  isAuthenticated(): boolean {
    return this._user != undefined;
  }
}
