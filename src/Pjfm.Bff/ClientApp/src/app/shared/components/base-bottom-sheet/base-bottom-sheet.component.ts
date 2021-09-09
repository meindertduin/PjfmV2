import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'pjfm-base-bottom-sheet',
  templateUrl: './base-bottom-sheet.component.html',
  styleUrls: ['./base-bottom-sheet.component.scss'],
})
export class BaseBottomSheetComponent {
  @Output() closeDialog = new EventEmitter();

  onCloseDialogClick(): void {
    this.closeDialog.emit();
  }
}
