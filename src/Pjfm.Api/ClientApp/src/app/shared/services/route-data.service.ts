import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RouteDataService {
  private readonly _isDetailPage = new BehaviorSubject<boolean>(false);
  private readonly _isDetailPage$: Observable<boolean> = this._isDetailPage.asObservable();

  setIsDetailPage(isDetailPage: boolean): void {
    this._isDetailPage.next(isDetailPage);
  }

  getIsDetailPage(): Observable<boolean> {
    return this._isDetailPage$;
  }
}
