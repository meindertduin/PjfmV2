import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { requiredValidator } from '../../../../core/utils/custom-form-validators';

@Component({
  selector: 'pjfm-start-listen-dialog',
  templateUrl: './start-listen-dialog.component.html',
  styleUrls: ['./start-listen-dialog.component.scss'],
})
export class StartListenDialogComponent implements OnInit {
  @Input() showDialog = false;
  @Output() closeDialog = new EventEmitter();

  listenSettingsFormGroup!: FormGroup;

  constructor(private readonly _formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.listenSettingsFormGroup = this._formBuilder.group({
      deviceId: ['', requiredValidator],
    });
  }

  onCloseDialog(): void {
    this.closeDialog.emit();
  }
}
