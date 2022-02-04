import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PlaybackService {
  private readonly _playbackIsActive = new BehaviorSubject<boolean>(false);
  private readonly _playbackIsActive$ = this._playbackIsActive.asObservable();

  setPlaybackIsActive(isActive: boolean): void {
    this._playbackIsActive.next(isActive);
  }

  getPlaybackIsActive(): Observable<boolean> {
    return this._playbackIsActive$;
  }
}
