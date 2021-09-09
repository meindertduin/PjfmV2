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

  constructor(
    private readonly _apiSocketClient: ApiSocketClientService,
    private readonly _activatedRoute: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this._apiSocketClient.initializeConnection();
    this._apiSocketClient.onConnectionEstablished.pipe(takeUntil(this._destroyed$)).subscribe(() => {
      const groupId = this._activatedRoute.snapshot.paramMap.get('id');

      if (groupId != null) {
        this._apiSocketClient.connectToGroup(groupId);
      }
    });

    this._apiSocketClient
      .getPlaybackData()
      .pipe(takeUntil(this._destroyed$))
      .subscribe((playbackData) => {
        this.loadedPlaybackData = playbackData;
        if (this.loadedPlaybackData != null) {
          this.trackStartTimeMs = new Date().getTime() - new Date(this.loadedPlaybackData.currentlyPlayingTrack.trackStartDate).getTime();
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
