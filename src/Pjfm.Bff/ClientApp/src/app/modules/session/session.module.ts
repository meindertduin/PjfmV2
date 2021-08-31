import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionRoutingModule } from './session-routing.module';
import { SessionComponent } from './features/session.component';
import { AsciiSliderComponent } from './components/ascii-slider/ascii-slider.component';

@NgModule({
  declarations: [SessionComponent, AsciiSliderComponent],
  imports: [CommonModule, SessionRoutingModule],
})
export class SessionModule {}
