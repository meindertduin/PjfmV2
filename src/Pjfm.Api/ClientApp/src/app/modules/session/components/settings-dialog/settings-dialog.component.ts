import { Component, Inject } from '@angular/core';
import { DialogRef, PJFM_DIALOG_DATA, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';
import { SelectTrackDialogData } from '../select-track-dialog/select-track-dialog.component';

@Component({
  selector: 'pjfm-settings-dialog',
  templateUrl: './settings-dialog.component.html',
  styleUrls: ['./settings-dialog.component.scss'],
})
export class SettingsDialogComponent {
  constructor(
    @Inject(PJFM_DIALOG_DATA) readonly dialogData: SelectTrackDialogData,
    @Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef,
  ) {}

  closeDialog(): void {
    this._dialogRef.closeDialog(undefined);
  }

  onResetSessionClicked(): void {}
}
