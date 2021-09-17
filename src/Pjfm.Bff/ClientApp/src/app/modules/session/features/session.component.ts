import { Component, OnDestroy, OnInit, ViewContainerRef } from '@angular/core';
import { ApiSocketClientService, PlaybackUpdateMessageBody } from '../../../core/services/api-socket-client.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { PlaybackService } from '../../../shared/services/playback.service';
import { PlaybackClient } from '../../../core/services/api-client.service';
import { DialogService } from '../../../shared/services/dialog.service';
import { StartListenDialogComponent, StartListenDialogData } from '../components/start-listen-dialog/start-listen-dialog.component';

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
  playbackIsActive!: boolean;

  private _playDialogOpen = false;

  constructor(
    private readonly _apiSocketClient: ApiSocketClientService,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _playbackService: PlaybackService,
    private readonly _playbackClient: PlaybackClient,
    private readonly _viewContainerRef: ViewContainerRef,
    private readonly _dialogService: DialogService,
  ) {}

  ngOnInit(): void {
    this.connectToGroup();
    this.getPlaybackData();
    this.getPlaybackIsActive();

    this._dialogService.setRootViewContainer(this._viewContainerRef);
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

  private getPlaybackIsActive() {
    this._playbackService
      .getPlaybackIsActive()
      .pipe(takeUntil(this._destroyed$))
      .subscribe((isActive) => {
        this.playbackIsActive = isActive;
      });
  }

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }

  playClicked(): void {
    if (this.loadedPlaybackData?.groupId == null || this._playDialogOpen) {
      return;
    }

    this._playDialogOpen = true;
    this.showStartListenDialog = true;
    this.openDialog({ groupId: this.loadedPlaybackData.groupId });
  }

  private openDialog(dialogData: StartListenDialogData) {
    this._dialogService.openDialog(StartListenDialogComponent, dialogData).subscribe(() => {
      this._playDialogOpen = false;
    });
  }

  pauseClicked(): void {
    this._playbackClient.stop().subscribe(() => {
      this._playbackService.setPlaybackIsActive(false);
    });
  }
}
