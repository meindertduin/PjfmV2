import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionRoutingModule } from './session-routing.module';
import { SessionComponent } from './features/session.component';
import { AsciiSliderComponent } from './components/ascii-slider/ascii-slider.component';
import { TrackProgressionBarComponent } from './components/track-progression-bar/track-progression-bar.component';
import { ConvertMsToTimePipe } from './pipes/convert-ms-to-time.pipe';

@NgModule({
  declarations: [SessionComponent, AsciiSliderComponent, TrackProgressionBarComponent, ConvertMsToTimePipe],
  imports: [CommonModule, SessionRoutingModule],
})
export class SessionModule {}
