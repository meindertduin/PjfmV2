import { Component, Inject, OnInit } from '@angular/core';
import { DialogRef, PJFM_DIALOG_DATA, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';
import { SelectTrackDialogData } from '../select-track-dialog/select-track-dialog.component';
import { ApplicationUserDto, PlaybackClient, UserClient } from '../../../../core/services/api-client.service';
import { finalize } from 'rxjs/operators';
import { AutoCompleteValue } from '../../../../core/form-inputs/autocomplete/autocomplete.component';

@Component({
  selector: 'pjfm-settings-dialog',
  templateUrl: './settings-dialog.component.html',
  styleUrls: ['./settings-dialog.component.scss'],
})
export class SettingsDialogComponent implements OnInit {
  private _isRequesting = false;
  autoCompleteValues: AutoCompleteValue[] = [];
  selectedUsers: ApplicationUserDto[] = [];

  constructor(
    @Inject(PJFM_DIALOG_DATA) readonly dialogData: SelectTrackDialogData,
    @Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef,
    private readonly _playbackClient: PlaybackClient,
    private readonly _userClient: UserClient,
  ) {}

  ngOnInit(): void {}

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

  onQueryChange(query: string): void {
    if (query.length > 2) {
      this._userClient.autocomplete(query, 20).subscribe((result) => {
        this.autoCompleteValues = result.map((a) => {
          return {
            value: a.userId,
            text: a.userName,
          };
        });
      });
    }
  }

  onSearchValueSelect(value: unknown): void {
    const autoCompleteValue = this.autoCompleteValues.find((s) => s.value === (value as string));

    if (autoCompleteValue == null || this.selectedUsers.findIndex((s) => s.userId == (value as string)) !== -1) {
      return;
    }

    this.selectedUsers.push({
      userId: autoCompleteValue.value as string,
      userName: autoCompleteValue.text,
    });
  }

  removeSelectedUser(index: number): void {
    this.selectedUsers.splice(index, 1);
  }
}
