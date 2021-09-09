import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'pjfm-start-listen-dialog',
  templateUrl: './start-listen-dialog.component.html',
  styleUrls: ['./start-listen-dialog.component.scss'],
})
export class StartListenDialogComponent {
  @Input() showDialog = false;
  @Output() closeDialog = new EventEmitter();

  onCloseDialog(): void {
    this.closeDialog.emit();
  }
}
