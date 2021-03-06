import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionRoutingModule } from './session-routing.module';
import { SessionComponent } from './features/session.component';
import { AsciiSliderComponent } from './components/ascii-slider/ascii-slider.component';
import { TrackProgressionBarComponent } from './components/track-progression-bar/track-progression-bar.component';
import { ConvertMsToTimePipe } from './pipes/convert-ms-to-time.pipe';
import { InlineSVGModule } from 'ng-inline-svg';
import { HttpClientModule } from '@angular/common/http';
import { QueuedTrackComponent } from './components/queued-track/queued-track.component';
import { SharedModule } from '../../shared/shared.module';
import { StartListenDialogComponent } from './components/start-listen-dialog/start-listen-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormInputsModule } from '../../core/form-inputs/form-inputs.module';
import { NgxPermissionsModule } from 'ngx-permissions';
import { SelectTrackDialogComponent } from './components/select-track-dialog/select-track-dialog.component';
import { SettingsDialogComponent } from './components/settings-dialog/settings-dialog.component';

const COMPONENTS = [
  SessionComponent,
  AsciiSliderComponent,
  TrackProgressionBarComponent,
  QueuedTrackComponent,
  StartListenDialogComponent,
  SelectTrackDialogComponent,
];
const PIPES = [ConvertMsToTimePipe];

@NgModule({
  declarations: [COMPONENTS, PIPES, SettingsDialogComponent],
  imports: [
    CommonModule,
    SessionRoutingModule,
    InlineSVGModule,
    HttpClientModule,
    SharedModule,
    ReactiveFormsModule,
    FormInputsModule,
    NgxPermissionsModule,
  ],
})
export class SessionModule {}
