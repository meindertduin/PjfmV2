import { Component, OnDestroy, OnInit } from '@angular/core';
import { ApiSocketClientService, PlaybackUpdateMessageBody } from '../../../core/services/api-socket-client.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'pjfm-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss'],
})
export class SessionComponent implements OnInit, OnDestroy {
  private readonly _destroyed$ = new Subject();
  loadedPlaybackData: PlaybackUpdateMessageBody | null = null;
  trackStartTimeMs = 0;
  showStartListenDialog = false;

  constructor(private readonly _apiSocketClient: ApiSocketClientService, private readonly _activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.connectToGroup();
    this.getPlaybackData();
  }

  private connectToGroup() {
    this._apiSocketClient
      .getIsConnected()
      .pipe(takeUntil(this._destroyed$))
      .subscribe((isConnected) => {
        if (isConnected) {
          const groupId = this._activatedRoute.snapshot.paramMap.get('id');

          if (groupId != null) {
            this._apiSocketClient.connectToGroup(groupId);
          }
        }
      });
  }

  private getPlaybackData() {
    this._apiSocketClient
      .getPlaybackData()
      .pipe(takeUntil(this._destroyed$))
      .subscribe((playbackData) => {
        this.loadedPlaybackData = playbackData;
        if (this.loadedPlaybackData != null) {
          const trackStartDate = new Date(this.loadedPlaybackData.currentlyPlayingTrack.trackStartDate);
          this.trackStartTimeMs = new Date().getTime() - trackStartDate.getTime();
        }
      });
  }

  ngOnDestroy(): void {
    this._destroyed$.complete();
    this._destroyed$.next();
  }

  playClicked(): void {
    this.showStartListenDialog = true;
  }
}
