import { Component, Inject, OnInit } from '@angular/core';
import { DialogRef, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';

@Component({
  selector: 'pjfm-select-track-dialog',
  templateUrl: './select-track-dialog.component.html',
  styleUrls: ['./select-track-dialog.component.scss'],
})
export class SelectTrackDialogComponent implements OnInit {
  constructor(@Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef) {}

  ngOnInit(): void {}

  closeDialog(): void {
    this._dialogRef.closeDialog(undefined);
  }
}
