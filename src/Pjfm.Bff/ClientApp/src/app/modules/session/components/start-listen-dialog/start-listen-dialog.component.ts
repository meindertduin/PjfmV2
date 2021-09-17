import { Component, Input, Output, EventEmitter, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { requiredValidator } from '../../../../core/utils/custom-form-validators';
import { DeviceModel, PlaybackClient, SpotifyClient } from '../../../../core/services/api-client.service';
import { PlaybackService } from '../../../../shared/services/playback.service';
import { DialogRef, PJFM_DIALOG_DATA, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';

@Component({
  selector: 'pjfm-start-listen-dialog',
  templateUrl: './start-listen-dialog.component.html',
  styleUrls: ['./start-listen-dialog.component.scss'],
})
export class StartListenDialogComponent implements OnInit {
  @Input() groupId!: string;
  @Output() closeDialog = new EventEmitter();

  listenSettingsFormGroup!: FormGroup;
  devices: DeviceModel[] = [];
  private readonly _devicesSort = (a: DeviceModel, b: DeviceModel): number => {
    if (a.isActive) return -1;
    if (b.isActive) return 1;
    if (a.deviceName.toLowerCase() < b.deviceName.toLowerCase()) return -1;
    if (a.deviceName.toLowerCase() > b.deviceName.toLowerCase()) return 1;
    return 0;
  };

  constructor(
    private readonly _formBuilder: FormBuilder,
    private readonly _spotifyClient: SpotifyClient,
    private readonly _playbackClient: PlaybackClient,
    private readonly _playbackService: PlaybackService,
    @Inject(PJFM_DIALOG_DATA) private readonly data: unknown,
    @Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef,
  ) {}

  ngOnInit(): void {
    this._dialogRef.closeDialog(this.data);
    this.createFormGroup();
    this.loadDevices();
  }

  get deviceIdFormControl(): FormControl {
    return this.listenSettingsFormGroup.get('deviceId') as FormControl;
  }

  private createFormGroup() {
    this.listenSettingsFormGroup = this._formBuilder.group({
      deviceId: ['', requiredValidator],
    });
  }

  private loadDevices(): void {
    this._spotifyClient.devices().subscribe((devicesResponse) => {
      this.devices = devicesResponse.devices.sort(this._devicesSort);
      if (this.devices.length === 0) {
        return;
      }

      const activeDevice = this.devices.find((x) => x.isActive);
      if (activeDevice != null) {
        this.deviceIdFormControl.setValue(activeDevice.deviceId);
      } else {
        this.deviceIdFormControl.setValue(this.devices[0].deviceId);
      }
    });
  }

  onCloseDialog(): void {
    this.closeDialog.emit();
  }

  onPlayClicked(): void {
    const deviceId = this.deviceIdFormControl.value as string;
    if (deviceId != null && deviceId) {
      this._playbackClient.play(deviceId, this.groupId).subscribe(() => {
        this._playbackService.setPlaybackIsActive(true);
        this.closeDialog.emit();
      });
    }
  }
}
