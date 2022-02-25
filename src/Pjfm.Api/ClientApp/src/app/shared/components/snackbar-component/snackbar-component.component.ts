import { Component, Inject } from '@angular/core';
import { PJFM_SNACKBAR_DATA, PJFM_SNACKBAR_REF, SnackBarRef } from '../../services/snackbar.service';

@Component({
  selector: 'pjfm-snackbar-component',
  templateUrl: './snackbar-component.component.html',
  styleUrls: ['./snackbar-component.component.scss'],
})
export class SnackbarComponentComponent {
  constructor(
    @Inject(PJFM_SNACKBAR_DATA) private readonly _snackBarData: SnackBarData,
    @Inject(PJFM_SNACKBAR_REF) private readonly _snackBarRef: SnackBarRef,
  ) {}

  onOkButtonClicked(): void {
    this._snackBarRef.closeSnackBar();
  }
}

export interface SnackBarData {
  message: string;
}
