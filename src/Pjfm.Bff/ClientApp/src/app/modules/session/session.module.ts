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
import { PlaybackButtonsComponent } from './components/playback-buttons/playback-buttons.component';

const COMPONENTS = [SessionComponent, AsciiSliderComponent, TrackProgressionBarComponent];
const PIPES = [ConvertMsToTimePipe];

@NgModule({
  declarations: [COMPONENTS, PIPES, QueuedTrackComponent, PlaybackButtonsComponent],
  imports: [CommonModule, SessionRoutingModule, InlineSVGModule, HttpClientModule],
})
export class SessionModule {}
