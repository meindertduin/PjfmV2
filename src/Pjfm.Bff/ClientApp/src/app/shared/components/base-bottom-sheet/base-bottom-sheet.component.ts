import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'pjfm-base-bottom-sheet',
  templateUrl: './base-bottom-sheet.component.html',
  styleUrls: ['./base-bottom-sheet.component.scss'],
})
export class BaseBottomSheetComponent implements OnInit, OnDestroy {
  @Output() closeDialog = new EventEmitter();

  onCloseDialogClick(): void {
    this.closeDialog.emit();
  }

  ngOnInit(): void {
    const winX = window.scrollX;
    const winY = window.scrollY;

    window.onscroll = () => {
      window.scrollTo(winX, winY);
    };
  }

  ngOnDestroy(): void {
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    window.onscroll = () => {};
  }
}
