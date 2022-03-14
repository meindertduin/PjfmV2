import { Component, Inject } from '@angular/core';
import { DialogRef, PJFM_DIALOG_DATA, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';
import { SelectTrackDialogData } from '../select-track-dialog/select-track-dialog.component';
import { PlaybackClient } from '../../../../core/services/api-client.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'pjfm-settings-dialog',
  templateUrl: './settings-dialog.component.html',
  styleUrls: ['./settings-dialog.component.scss'],
})
export class SettingsDialogComponent {
  private _isRequesting = false;

  constructor(
    @Inject(PJFM_DIALOG_DATA) readonly dialogData: SelectTrackDialogData,
    @Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef,
    private readonly _playbackClient: PlaybackClient,
  ) {}

  closeDialog(): void {
    this._dialogRef.closeDialog(undefined);
  }

  onResetSessionClicked(): void {
    if (this._isRequesting) return;

    this._isRequesting = true;

    this._playbackClient
      .reset()
      .pipe(
        finalize(() => {
          this._isRequesting = false;
        }),
      )
      .subscribe(() => {
        this.closeDialog();
      });
  }
}
