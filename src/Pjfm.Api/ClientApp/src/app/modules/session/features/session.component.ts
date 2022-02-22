import { Component, OnDestroy, OnInit, ViewContainerRef } from '@angular/core';
import { ApiSocketClientService, PlaybackUpdateMessageBody } from '../../../core/services/api-socket-client.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { PlaybackService } from '../../../shared/services/playback.service';
import { PlaybackClient, SpotifyTrackDto } from '../../../core/services/api-client.service';
import { DialogService } from '../../../shared/services/dialog.service';
import { StartListenDialogComponent, StartListenDialogData } from '../components/start-listen-dialog/start-listen-dialog.component';
import { SelectTrackDialogComponent, SelectTrackDialogData } from '../components/select-track-dialog/select-track-dialog.component';
import { UserService } from '../../../shared/services/user.service';

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
    private readonly _userService: UserService,
  ) {}

  ngOnInit(): void {
    this._dialogService.setRootViewContainer(this._viewContainerRef);
    this.connectToGroup();
    this.getPlaybackData();
    this.getPlaybackIsActive();
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
    this._dialogService
      .openDialog(StartListenDialogComponent, dialogData)
      .pipe(takeUntil(this._destroyed$))
      .subscribe(() => {
        this._playDialogOpen = false;
      });
  }

  pauseClicked(): void {
    this._playbackClient.stop().subscribe(() => {
      this._playbackService.setPlaybackIsActive(false);
    });
  }

  openRequestDialog(): void {
    const userId = this._userService.getUser()?.userId;
    if (userId == null) {
      throw new Error('User should not be able to open request dialog if no user value is available.');
    }

    const userRequestedAmount =
      this.loadedPlaybackData?.queuedTracks.reduce((sum: number, cur: SpotifyTrackDto) => {
        if (cur.user?.userId == userId) {
          return sum + 1;
        }
        return sum;
      }, 0) ?? 0;

    const dialogData: SelectTrackDialogData = {
      userRequestedAmount: userRequestedAmount,
    };

    this._dialogService.openDialog(SelectTrackDialogComponent, dialogData);
  }

  skipClicked(): void {
    this._playbackClient.skip();
  }
}
